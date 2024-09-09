using NewSolution.Model;
using NewSolution.Repository;
using NewSolution.Repository.Common;
using NewSolution.Service.Common;
using System;
using System.Collections.Generic;

namespace NewSolution.Service
{
    public class FootballerService : IFootballerService
    {
        public async Task<bool> InsertFootballerAsync(Footballer footballer)
        {
            FootballerRepository footballerRepository = new FootballerRepository();
            return await footballerRepository.InsertFootballerAsync(footballer);
        }

        public async Task<bool> DeleteFootballerByIdAsync(Guid id)
        {
            IFootballerRepository footballerRepository = new FootballerRepository();
            return await footballerRepository.DeleteFootballerByIdAsync(id);
        }

        public async Task<bool> UpdateFootballerByIdAsync(Guid id, Footballer footballer)
        {
            IFootballerRepository footballerRepository = new FootballerRepository();
            return await footballerRepository.UpdateFootballerByIdAsync(id, footballer);
        }

        public async Task<Footballer> GetFootballerByIdAsync(Guid id)
        {
            IFootballerRepository footballerRepository = new FootballerRepository();
            return await footballerRepository.GetFootballerByIdAsync(id);
        }

        public async Task<List<Footballer>> GetFootballersAsync()
        {
            IFootballerRepository footballerRepository = new FootballerRepository();
            return await footballerRepository.GetFootballersAsync();
        }
    }
}
