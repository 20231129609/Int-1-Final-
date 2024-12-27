namespace deneme135.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; } // Cevap içeriği
        public bool IsCorrect { get; set; } // Doğru cevap olup olmadığı
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}