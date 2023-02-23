using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KatalogProduk.Pages.Admin
{
    [AllowAnonymous]
    public class DeniedModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
