﻿using NewSolution.Common;
using NewSolution.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSolution.Service.Common
{
    public interface IFootballerService
    {
        Task<bool> InsertFootballerAsync(Footballer footballer);
        Task<bool> DeleteFootballerByIdAsync(Guid id);
        Task<bool> UpdateFootballerByIdAsync(Guid id, Footballer footballer);
        Task<Footballer> GetFootballerByIdAsync(Guid id);
        Task<List<Footballer>> GetFootballersAsync(FootballerFilter footballerFilter, Paging paging, Sorting sorting);
    }
}
