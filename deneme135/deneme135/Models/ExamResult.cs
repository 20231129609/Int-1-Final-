using deneme135.Models;

namespace deneme135
{
    public class ExamResult
    {
        public int Id { get; set; }
        public int ExamId { get; set; }  // Hangi sınavı tamamladığını gösterir.
        public int UserId { get; set; }  // Hangi kullanıcı bu sınavı tamamladı.
        public double Score { get; set; }  // Kullanıcının sınavdan aldığı puan.
        public Exam Exam { get; set; }  // Sınav ile ilişkili.
        public ApplicationUser User { get; set; }  // Kullanıcı ile ilişkili.
    }
}