using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using KatalogProduk.Models;

namespace KatalogProduk.Pages.Admin
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public RegisterModel (Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
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
                return Notif("Sukses! Akun anda berhasil register, Silahkan Login!", "success", "/Admin/Login");
            }

            return Notif("Error! Email sudah terdaftar, Silahkan Registrasi Ulang!", "danger", "/Admin/Register");
        }

        public IActionResult Notif(string message, string tipe, string link)
        {
            TempData["Message"] = message;
            TempData["Tipe"] = tipe;
            return RedirectToPage(link);
        }
    }
}
