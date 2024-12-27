using deneme135.Models;

namespace YourProjectNamespace.ViewModels
{
    public class ExamViewModel
    {
        public int ExamId { get; set; }  // Sınav ID'si
        public string ExamName { get; set; }  // Sınav Adı
        public DateTime ExamDate { get; set; }  // Sınav Tarihi

        public List<Question> Questions { get; set; }  // Sorular
    }
}
