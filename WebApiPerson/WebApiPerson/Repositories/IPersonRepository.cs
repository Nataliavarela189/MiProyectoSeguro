using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPerson.Models;

namespace WebApiPerson.Repositories
{
    public interface IPersonRepository
    {
        Task<Person?> GetByIdAsync(int id);
        Task<IEnumerable<Person>> GetAllAsync();
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(Person person);
        Task<bool> ExistsAsync(int id);
    }
}
