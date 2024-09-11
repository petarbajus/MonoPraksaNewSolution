﻿using NewSolution.Common;
using NewSolution.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSolution.Repository.Common
{
    public interface IClubRepository
    {
        Task<bool> InsertClubAsync(Club club);
        Task<bool> DeleteClubByIdAsync(Guid id);
        Task<bool> UpdateClubByIdAsync(Guid id, Club club);
        Task<Club> GetClubByIdAsync(Guid id);
        Task<List<Club>> GetClubsAsync(ClubFilter clubFilter, Paging paging, Sorting sorting);
    }
}
