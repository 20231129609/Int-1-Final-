using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace deneme135.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);  // ID'ye göre veri çekme
        Task<IEnumerable<T>> GetAllAsync();  // Tüm verileri çekme
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);  // Filtreleme işlemi
        Task AddAsync(T entity);  // Yeni veri ekleme
        Task UpdateAsync(T entity);  // Veri güncelleme
        Task DeleteAsync(int id);  // Veri silme
    }
}
