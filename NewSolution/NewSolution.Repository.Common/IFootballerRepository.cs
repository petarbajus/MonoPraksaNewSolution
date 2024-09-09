using NewSolution.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSolution.Repository.Common
{
    public interface IFootballerRepository
    {
        Task<bool> InsertFootballerAsync(Footballer footballer);
        Task<bool> DeleteFootballerByIdAsync(Guid id);
        Task<bool> UpdateFootballerByIdAsync(Guid id, Footballer footballer);
        Task<Footballer> GetFootballerByIdAsync(Guid id);
        Task<List<Footballer>> GetFootballersAsync();
    }
}
