using System.Diagnostics;
using System.Collections.Generic;
using deneme135.Models;
using deneme135.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace deneme135.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExamRepository _examRepository;

        public HomeController(ILogger<HomeController> logger, ExamRepository examRepository)
        {
            _logger = logger;
            _examRepository = examRepository;
        }

        public IActionResult Index()
        {
            List<Exam> exams = new List<Exam>();

            // Sadece öğrenci rolündeki kullanıcılar için sınav listesini göster
            if (User.Identity.IsAuthenticated && User.IsInRole("Student"))
            {
                try
                {
                    exams = _examRepository.GetList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Sınavlar listelenirken hata oluştu");
                }
            }

            return View(exams);
        }

        public IActionResult Contact()
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
