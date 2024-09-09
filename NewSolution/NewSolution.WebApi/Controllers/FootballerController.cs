using Microsoft.AspNetCore.Mvc;

namespace NewSolution.WebApi.Controllers
{
    public class FootballerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
