# Implementacija eksternog identity provider-a (Google)

* Forma u ```_ExternalAuthentication``` partial view vec gadja ```ExternalLogin``` akciju. Zato moramo kreirati akciju u ```AccountController.cs```. 
  ```
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult ExternalLogin(string provider, string returnUrl = null)
  {
      var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
      var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
      return Challenge(properties, provider);
  }
  ```
* Sada moramo  implementirati ```ExternalLoginCallback``` akciju u ```AccountController.cs```.
  ```
  [HttpGet]
  public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
  {
      var info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
          return RedirectToAction(nameof(Login));
      }
      var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
      if(signInResult.Succeeded)
      {
          return RedirectToLocal(returnUrl);
      }
      if(signInResult.IsLockedOut)
      {
          return RedirectToAction(nameof(ForgotPassword));
      }
      else
      {
          ViewData["ReturnUrl"] = returnUrl;
          ViewData["Provider"] = info.LoginProvider;
          var email = info.Principal.FindFirstValue(ClaimTypes.Email);
          return View("ExternalLogin", new ExternalLoginModel { Email = email });
      }
  }
  ```
  Sa ```GetExternalLoginInfoAsync``` metodom mi prikupimo login informacije kao sto su provader, ime, prezime email, ...
  
## Imlementacija ExternalLogin and ExternalLoginConfirmation
* Prvo dodamo ```ExternalLoginModel``` klasu u ```Models``` folderu:
  ```
  public class ExternalLoginModel
  {
      [Required]
      [EmailAddress]
      public string Email { get; set; }
      public ClaimsPrincipal Principal { get; set; }
  }
  ```
* Zatim moramo kreirati view ```ExternalLogin.cshtml``` u ```Views/Account``` folderu
  ```
  @model IdentityByExamples.Models.ExternalLoginModel
  <h4>External provider registration</h4>
  <hr />
  <p class="text-info">
      Enter an email to associate your <strong>@ViewData["Provider"]</strong> account.
  </p>
  <div class="row">
      <div class="col-md-4">
          <form asp-action="ExternalLoginConfirmation" asp-route-returnurl="@ViewData["ReturnUrl"]">
              <div asp-validation-summary="All" class="text-danger"></div>
              <div class="form-group">
                  <label asp-for="Email" class="control-label"></label>
                  <input asp-for="Email" class="form-control" />
                  <span asp-validation-for="Email" class="text-danger"></span>
              </div>
              <div class="form-group">
                  <input type="submit" value="Submit" class="btn btn-primary" />
              </div>
          </form>
      </div>
  </div>
  ```
  Vidimo kaka se klikne na submit button forma ce se poslati na ```ExternalLoginConfirmation``` akciju zato implementiramo datu akciju u ```AccountController.cs```.
  
  ```
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginModel model, string returnUrl = null)
  {
      if (!ModelState.IsValid)
          return View(model);

      var info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
          return View(nameof(Error));

      var user = await _userManager.FindByEmailAsync(model.Email);
      IdentityResult result;

      if(user != null)
      {

          await _signInManager.SignInAsync(user, isPersistent: false);
          return RedirectToLocal(returnUrl);

      }
      else
      {
          model.Principal = info.Principal;
          user = _mapper.Map<User>(model);
          result = await _userManager.CreateAsync(user);
          if (result.Succeeded)
          {
              result = await _userManager.AddLoginAsync(user, info);
              if (result.Succeeded)
              {
                  //TODO: Send an emal for the email confirmation and add a default role as in the Register action
                  await _signInManager.SignInAsync(user, isPersistent: false);
                  return RedirectToLocal(returnUrl);
              }
          }
      }

      foreach (var error in result.Errors)
      {
          ModelState.TryAddModelError(error.Code, error.Description);
      }

      return View(nameof(ExternalLogin), model);
  }
  ```
  
Sada mozemo testirati logovanje preko google OAuth-a
