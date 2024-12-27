using System;
using System.ComponentModel.DataAnnotations;

namespace deneme135.Models
{
    public class Test
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Test adı zorunludur")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Test açıklaması zorunludur")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Test içeriği zorunludur")]
        public string Content { get; set; }  // Test soruları ve cevapları burada saklanacak

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
} 