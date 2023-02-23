using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using KatalogProduk.Models;

namespace KatalogProduk.Pages.Admin
{
    public class EditModel : PageModel
    {

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public EditModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public UserApiDto Users { get; set; } = default!;

        [BindProperty]
        public IFormFile newProfile { get; set; } = default!;

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
                var data = JsonConvert.DeserializeObject<UserApiDto>(response);

                Users = data;
                TempData["ProfileUrl"] = Users.ProfileUrl;

                //var filePath = Path.Combine(_environment.WebRootPath, "images", Users.ProfileUrl);
                //var fileBytes = System.IO.File.ReadAllBytes(filePath);
                //using var memoryStream = new System.IO.MemoryStream(fileBytes);
                //FormFile formFile = new FormFile(memoryStream, 0, fileBytes.Length, "file", Users.ProfileUrl);

                //newProfile = formFile;

                string url2 = "https://localhost:7197/api/User/role/" + data.Email;
                string response2 = await client.GetStringAsync(url2);
                var data2 = JsonConvert.DeserializeObject<List<string>>(response2);

                foreach (string roles in data2)
                {
                    Users.role = roles;
                }             
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (newProfile != null)
            {
                var file = Path.Combine(_environment.WebRootPath, "images", newProfile.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await newProfile.CopyToAsync(fileStream);
                }
                Users.ProfileUrl = newProfile.FileName;
            }

            Users.ProfileUrl = TempData["ProfileUrl"].ToString();

            var data = JsonConvert.SerializeObject(Users);
            var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");


            var client = new HttpClient();
            string tokenData = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData);
            string url = "https://localhost:7197/api/User/" + id;
            HttpResponseMessage response = await client.PutAsync(url, content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return Notif("Sukses! User berhasil di edit.", "success", "./User");
            }
            else
            {
                //bikin kodingan kalau kamu bukan admin
                return Notif("Error! Anda bukan Admin, user gagal di edit.", "danger", "./User");
            }

        }

        public IActionResult Notif(string message, string tipe, string link)
        {
            TempData["Message"] = message;
            TempData["Tipe"] = tipe;
            return RedirectToPage(link);
        }
    }
}
