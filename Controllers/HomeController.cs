using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniPki.Models;

namespace MiniPki.Controllers;

public class HomeController : Controller
{
    public HomeController( )
    {   
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult NoLogin()
    {
        return View();
    }
}
