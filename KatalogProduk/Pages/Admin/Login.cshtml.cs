using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KatalogProduk.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace KatalogProduk.Pages.Admin
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string ReturnUrl { get; set; }

        public string Message { get; private set; } = null;

        [BindProperty]
        public LoginDto LoginDto { get; set; }

        public void OnGet(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (returnUrl == null)
            {
                ReturnUrl = "~/index";
            } if (returnUrl == "/Admin/User")
            {
                Message = "Youre not authenticated to access User Page, Please Login!";
            }

            ReturnUrl = returnUrl;
        }



        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }


            var data = JsonConvert.SerializeObject(LoginDto);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            string url = "https://localhost:7197/api/User/login";
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(url, content).ConfigureAwait(false);
           

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                dictionary.TryGetValue("email", out var email);
                dictionary.TryGetValue("token", out var token);
                HttpContext.Session.SetString("token", token.ToString());

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, email.ToString()),
                        new Claim(ClaimTypes.Name, email.ToString())
                    };


                var identityUser = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identityUser);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return LocalRedirect(returnUrl);
            }
            else
            {
                TempData["Message"] = "Error! Email atau Password salah, Silahkan Coba Login lagi!";
                TempData["Tipe"] = "danger";
                return RedirectToPage("/Admin/Login");
            }

            return LocalRedirect(returnUrl);
        }
    }

}
