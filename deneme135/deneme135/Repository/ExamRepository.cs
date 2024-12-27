using deneme135.Data;
using deneme135.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace deneme135.Repositories
{
    public class ExamRepository
    {
        private readonly AppDbContext _context;

        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }

        // Tüm sınavları alır
        public List<Exam> GetList()
        {
            return _context.Exams.ToList(); // Exam tablosundaki tüm verileri alır
        }

        // Id ile sınavı bulur
        public Exam GetById(int id)
        {
            return _context.Exams.FirstOrDefault(e => e.Id == id); // Id'ye göre sınavı bulur
        }

        // Yeni bir sınav ekler
        public void Add(Exam exam)
        {
            _context.Exams.Add(exam);
            _context.SaveChanges(); // Veritabanına kaydeder
        }

        // Mevcut sınavı günceller
        public void Update(Exam exam)
        {
            var existingExam = _context.Exams.Find(exam.Id);
            if (existingExam != null)
            {
                _context.Entry(existingExam).CurrentValues.SetValues(exam);
                _context.SaveChanges();
            }
        }

        // Sınavı siler
        public void Delete(int id)
        {
            var exam = _context.Exams.Find(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                _context.SaveChanges(); // Silme işlemini veritabanında kaydeder
            }
        }

        internal void Delete(Exam exam)
        {
            throw new NotImplementedException();
        }
    }
}
