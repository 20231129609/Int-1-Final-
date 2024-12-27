namespace deneme135.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; } // Soru içeriği
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public ICollection<Answer> Answers { get; set; } // Soruya verilen cevaplar
    }
}