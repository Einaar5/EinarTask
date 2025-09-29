using System.Diagnostics;
using EinarTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace EinarTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DemoApp()
        {
            var taskTypes = new List<string> { "ToDo", "In Progress", "Done" };
            var tasks = new List<TaskModel>
    {
        new TaskModel { Id = 1, Title = "UI tasarýmý yap", TaskType = "ToDo" },
        new TaskModel { Id = 2, Title = "Backend API yaz", TaskType = "In Progress" },
        new TaskModel { Id = 3, Title = "Test et", TaskType = "Done" }
    };

            ViewBag.TaskTypes = taskTypes;
            ViewBag.Tasks = tasks;
            return View();
        }

        public class TaskModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string TaskType { get; set; }
        }

        public IActionResult DemoAppGoToLogin()
        {
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
}
