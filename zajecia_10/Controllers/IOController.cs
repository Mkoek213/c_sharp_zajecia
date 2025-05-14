// Controllers/IOController.cs
using Microsoft.AspNetCore.Mvc;
using LoginApp.Data;
using System.Linq;

public class IOController : Controller
{
    private const string LoginSessionKey = "IsLoggedIn";
    private readonly ApplicationDbContext _db;

    public IOController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Logowanie() => View();

    [HttpPost]
    public IActionResult Logowanie(string login, string haslo)
    {
        // Prosta weryfikacja „hashu” jako zwykłego porównania
        var user = _db.Loginy
            .FirstOrDefault(u => u.Username == login && u.PasswordHash == haslo);

        if (user != null)
        {
            HttpContext.Session.SetString(LoginSessionKey, "true");
            return RedirectToAction("Zalogowano");
        }

        ViewBag.Error = "Nieprawidłowy login lub hasło";
        return View();
    }

    public IActionResult Zalogowano()
    {
        if (HttpContext.Session.GetString(LoginSessionKey) != "true")
            return RedirectToAction("Logowanie");
        return View();
    }

    [HttpPost]
    public IActionResult Wyloguj()
    {
        HttpContext.Session.Remove(LoginSessionKey);
        return RedirectToAction("Logowanie");
    }
}
