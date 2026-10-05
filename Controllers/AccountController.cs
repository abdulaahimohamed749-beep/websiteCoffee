using Microsoft.AspNetCore.Mvc;
using websiteCofee.Data;
using websiteCofee.Models;
using System.Linq;

namespace websiteCofee.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Waxaan halkan ku soo qaadanaynaa Database-ka si aan u isticmaalno
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Kani wuxuu furayaa bogga Login-ka (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Kani wuxuu hubinayaa xogta la soo diray marka badanka la riixo (POST)
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Waxaan ka dhex baaraynaa Database-ka qofka isku dayaya inuu galo
                var adminUser = _context.Admins
                    .FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);

                if (adminUser != null)
                {
                    // Haddii xogtu sax tahay, u kaxee bogga ugu weyn ee Dashboard-ka ama Home
                    return RedirectToAction("Index", "Home");
                }

                // Haddii xogtu khaldan tahay, fariintan cilada ah iska muuji
                ModelState.AddModelError(string.Empty, "Email-ka ama Password-ka aad qortay waa khaldan yahay!");
            }

            // Haddii validation-ku fashilmo, dib ugu celi bogga login-ka iyadoo khaladaadka la tusayo
            return View(model);
        }
    }
}