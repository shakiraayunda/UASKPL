using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using KatalogProduk.Models;

namespace KatalogProduk.Pages.Admin
{
    [AllowAnonymous]
    public class CreateModel : PageModel
    {

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public CreateModel (Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
           _environment = environment;
        }

        [BindProperty]
        public UserApiDto UserApiDto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                //return Page();
            }

            var file = Path.Combine(_environment.WebRootPath, "images", UserApiDto.ProfileFile.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await UserApiDto.ProfileFile.CopyToAsync(fileStream);
            }
            UserApiDto.ProfileUrl = UserApiDto.ProfileFile.FileName;

            var data = JsonConvert.SerializeObject(UserApiDto);
            var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
            string url = "https://localhost:7197/api/User/register";
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(url, content).ConfigureAwait(false);
            

            if (response.IsSuccessStatusCode)
            {
                return Notif("Sukses! User berhasil ditambahkan", "success", "/Admin/User");
            }

            return Notif("Error! Email User sudah terdaftar", "danger", "/Admin/User");
        }

        public IActionResult Notif(string message, string tipe, string link)
        {
            TempData["Message"] = message;
            TempData["Tipe"] = tipe;
            return RedirectToPage(link);
        }
    }
}
