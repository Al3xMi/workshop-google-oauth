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

