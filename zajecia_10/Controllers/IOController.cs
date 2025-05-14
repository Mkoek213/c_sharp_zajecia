using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class IOController : Controller
{
    private const string LoginSessionKey = "IsLoggedIn";

    // GET: /IO/Logowanie
    public IActionResult Logowanie()
    {
        return View();
    }

    // POST: /IO/Logowanie
    [HttpPost]
    public IActionResult Logowanie(string login, string haslo)
    {
        // Tymczasowe dane logowania
        string prawidlowyLogin = "admin";
        string prawidloweHaslo = "1234";

        if (login == prawidlowyLogin && haslo == prawidloweHaslo)
        {
            HttpContext.Session.SetString(LoginSessionKey, "true");
            return RedirectToAction("Zalogowano");
        }

        ViewBag.Error = "Błędny login lub hasło";
        return View();
    }

    public IActionResult Zalogowano()
    {
        if (HttpContext.Session.GetString(LoginSessionKey) != "true")
        {
            return RedirectToAction("Logowanie");
        }
        return View();
    }

    public IActionResult Wyloguj()
    {
        HttpContext.Session.Remove(LoginSessionKey);
        return RedirectToAction("Logowanie");
    }
}
