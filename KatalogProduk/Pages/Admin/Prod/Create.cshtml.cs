using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KatalogProduk.Data;
using KatalogProduk.Models;

namespace KatalogProduk.Pages.Admin.Prod
{
    public class CreateModel : PageModel
    {
        private readonly KatalogProduk.Data.ApplicationDbContext _context;
        private IHostEnvironment _environment;
        public CreateModel(KatalogProduk.Data.ApplicationDbContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryID"] = new SelectList(_context.Category, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                //return Page();
            }

            var file = Path.Combine(_environment.ContentRootPath, "wwwroot//images", Product.ImageFile.FileName);
            using(var fileStream = new FileStream(file, FileMode.Create))
            {
                await Product.ImageFile.CopyToAsync(fileStream);
            }
            Product.ImageUrl = Product.ImageFile.FileName;
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
