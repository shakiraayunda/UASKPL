using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using KatalogProduk.Models;

namespace KatalogProduk.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        public string fullname { get; private set; } = null;
        public string email { get; private set; } = null;
        public string id2 { get; private set; } = null;

        public async Task<IActionResult> OnGetAsync(string? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = new HttpClient();
            var tokenData = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData);
            string url = "https://localhost:7197/api/User/" + id;
            string response = await client.GetStringAsync(url);

            if (response != null)
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                data.TryGetValue("fullName", out var fullname2);
                data.TryGetValue("email", out var email2);
                fullname = fullname2.ToString();
                email = email2.ToString();
                id2 = id;
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = new HttpClient();
            string tokenData = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData);
            string url = "https://localhost:7197/api/User/" + id;
            HttpResponseMessage response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return Notif("Sukses! User berhasil di hapus.", "success");
            }
            else
            {
                //bikin kodingan kalau kamu bukan admin
                return Notif("Error! Anda bukan Admin, user gagal di hapus.", "danger");
            }
            
        }

        public IActionResult Notif(string message, string tipe)
        {
            TempData["Message"] = message;
            TempData["Tipe"] = tipe;
            return RedirectToPage("./User");
        }


    }
}
