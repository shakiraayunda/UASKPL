using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using KatalogProduk.Models;

namespace KatalogProduk.Pages.Admin
{
    public class UserModel : PageModel
    {
        public IList<UserApi> Users { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync()
        {

            HttpClient client = new HttpClient();
            var tokenData = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData);
            try
            {
                string url = "https://localhost:7197/api/User";
                string response = await client.GetStringAsync(url);

                if (response != null)
                {
                    var data = JsonConvert.DeserializeObject<List<UserApi>>(response);
                    Users = data;
                }
                return Page();
            } catch (HttpRequestException ex)
            {
                TempData["Message"] = "Session sudah berakhir. Silahkan Login Ulang!";
                TempData["Tipe"] = "danger";
                return RedirectToPage("/Admin/Logout");
            }

            

        }
    }
}
