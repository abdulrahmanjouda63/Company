using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Company.PL.Models;
using Company.PL.Services;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Company.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace Company.PL.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IScopedService scopedService01;
    private readonly IScopedService scopedService02;
    private readonly ITransientService transientService01;
    private readonly ITransientService transientService02;
    private readonly ISingletonService singletonService01;
    private readonly ISingletonService singletonService02;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(
        ILogger<HomeController> logger,
        IScopedService scopedService01,
        IScopedService scopedService02,
        ITransientService transientService01,
        ITransientService transientService02,
        ISingletonService singletonService01,
        ISingletonService singletonService02,
        UserManager<AppUser> userManager
        )
    {
        _logger = logger;
        this.scopedService01 = scopedService01;
        this.scopedService02 = scopedService02;
        this.transientService01 = transientService01;
        this.transientService02 = transientService02;
        this.singletonService01 = singletonService01;
        this.singletonService02 = singletonService02;
        _userManager = userManager;
    }

    public string TestLifeTime()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"scopedService01 :: {scopedService01.GetGuid()}\n");
        builder.Append($"scopedService02 :: {scopedService02.GetGuid()}\n\n");
        builder.Append($"transientService01 :: {transientService01.GetGuid()}\n");
        builder.Append($"transientService02 :: {transientService02.GetGuid()}\n\n");
        builder.Append($"singletonService01 :: {singletonService01.GetGuid()}\n");
        builder.Append($"singletonService02 :: {singletonService02.GetGuid()}\n\n");

        return builder.ToString();
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        ViewBag.FirstName = user?.FirstName;
        ViewBag.LastName = user?.LastName;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
