using NewSolution.Common;
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

        private IFootballerRepository _footballerRepository;

        public FootballerService(IFootballerRepository footballerRepository)
        {
            this._footballerRepository = footballerRepository;
        }

        public async Task<bool> InsertFootballerAsync(Footballer footballer)
        {
            footballer.Id = Guid.NewGuid();
            return await _footballerRepository.InsertFootballerAsync(footballer);
        }

        public async Task<bool> DeleteFootballerByIdAsync(Guid id)
        {
            return await _footballerRepository.DeleteFootballerByIdAsync(id);
        }

        public async Task<bool> UpdateFootballerByIdAsync(Guid id, Footballer footballer)
        {
            return await _footballerRepository.UpdateFootballerByIdAsync(id, footballer);
        }

        public async Task<Footballer> GetFootballerByIdAsync(Guid id)
        {
            return await _footballerRepository.GetFootballerByIdAsync(id);
        }

        public async Task<List<Footballer>> GetFootballersAsync(FootballerFilter footballerFilter, Paging paging, Sorting sorting)
        {
            return await _footballerRepository.GetFootballersAsync(footballerFilter, paging, sorting);
        }
    }
}
