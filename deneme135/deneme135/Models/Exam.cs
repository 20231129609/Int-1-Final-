using System;
using System.ComponentModel.DataAnnotations;

namespace deneme135.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tarih alanı zorunludur.")]
        public DateTime Date { get; set; }
    }
}
