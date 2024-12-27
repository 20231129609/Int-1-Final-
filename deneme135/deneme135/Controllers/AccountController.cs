using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using deneme135.Models;
using System.Threading.Tasks;

namespace deneme135.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Register sayfası GET metodu
        public IActionResult Register()
        {
            return View();
        }

        // Register işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword, string role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(role))
            {
                ModelState.AddModelError(string.Empty, "Tüm alanları doldurunuz.");
                return View();
            }

            // Şifre ve onay şifresi eşleşiyor mu?
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler eşleşmiyor.");
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = username,
                FullName = username // İsteğe bağlı: Kullanıcı adını tam ad olarak atayabilirsiniz
            };

            // Kullanıcıyı oluşturma
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Rol yoksa oluştur
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                // Rol ata
                await _userManager.AddToRoleAsync(user, role);

                // Otomatik giriş yap
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Rol bazlı yönlendirme
                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (role == "Student")
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // Kayıt hataları varsa ekrana yansıt
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        // Login sayfası GET metodu
        public IActionResult Login()
        {
            return View();
        }

        // Login işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı ve şifre gereklidir.");
                return View();
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                // Kullanıcı rolünü kontrol et
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    // Admin kullanıcısı: Admin sayfasına yönlendir
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Admin");
                }
                else if (roles.Contains("Student"))
                {
                    // Öğrenci kullanıcısı: Öğrenci sayfasına yönlendir
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Kullanıcı rolü geçerli değilse, giriş iptal edilir
                    await _signInManager.SignOutAsync();
                    ModelState.AddModelError(string.Empty, "Bu kullanıcı geçerli bir role sahip değil.");
                    return View();
                }
            }

            // Geçersiz kullanıcı adı veya şifre
            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
            return View();
        }

        // Logout işlemi
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
