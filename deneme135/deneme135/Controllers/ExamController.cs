using deneme135.Models;
using deneme135.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using deneme135.Hubs;

namespace deneme135.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly ExamRepository _examRepository;
        private readonly IHubContext<ExamHub> _hubContext;

        public ExamController(ExamRepository examRepository, IHubContext<ExamHub> hubContext)
        {
            _examRepository = examRepository;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetExam(int id)
        {
            try
            {
                var exam = _examRepository.GetById(id);
                if (exam == null)
                    return Json(new { success = false, message = "Sınav bulunamadı." });

                return Json(new { 
                    success = true, 
                    id = exam.Id,
                    title = exam.Title,
                    description = exam.Description,
                    date = exam.Date.ToString("yyyy-MM-ddTHH:mm")
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveExam([FromBody] Exam exam)
        {
            try
            {
                if (exam == null)
                    return Json(new { success = false, message = "Geçersiz veri." });

                if (string.IsNullOrWhiteSpace(exam.Title))
                    return Json(new { success = false, message = "Sınav adı boş olamaz." });

                if (string.IsNullOrWhiteSpace(exam.Description))
                    return Json(new { success = false, message = "Sınav açıklaması boş olamaz." });

                var now = DateTime.Now;
                var today = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

                if (exam.Date < today)
                    return Json(new { success = false, message = "Sınav tarihi geçmiş bir tarih olamaz." });

                if (exam.Id == 0)
                {
                    _examRepository.Add(exam);
                    await _hubContext.Clients.All.SendAsync("ReceiveExamCreate", 
                        $"Yeni sınav eklendi: {exam.Title}");
                }
                else
                {
                    var existingExam = _examRepository.GetById(exam.Id);
                    if (existingExam == null)
                        return Json(new { success = false, message = "Düzenlenecek sınav bulunamadı." });

                    _examRepository.Update(exam);
                    await _hubContext.Clients.All.SendAsync("ReceiveExamUpdate", 
                        $"Sınav güncellendi: {exam.Title}");
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var exam = _examRepository.GetById(id);
                if (exam == null)
                    return Json(new { success = false, message = "Silinecek sınav bulunamadı." });

                _examRepository.Delete(id);
                await _hubContext.Clients.All.SendAsync("ReceiveExamDelete", 
                    $"Sınav silindi: {exam.Title}");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
