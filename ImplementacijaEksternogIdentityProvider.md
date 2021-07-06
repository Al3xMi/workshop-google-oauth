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

