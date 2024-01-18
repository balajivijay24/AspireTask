using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace RoleBasedAuthentication.Controllers;

[Authorize(Roles ="Admin")]
public class AdminController : Controller
{
     public IActionResult Index()
    {
        return View();
    }
}