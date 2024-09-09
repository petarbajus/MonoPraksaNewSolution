using NewSolution.Model;
using NewSolution.Repository;
using NewSolution.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSolution.Service
{
    public class ClubService : IClubService
    {
        public ClubService() { }
        public async Task<bool> DeleteClubByIdAsync(Guid id)
        {
            ClubRepository clubRepository = new ClubRepository();
            return await clubRepository.DeleteClubByIdAsync(id);
        }

        public async Task<Club> GetClubByIdAsync(Guid id)
        {
            ClubRepository clubRepository = new ClubRepository();
            return await clubRepository.GetClubByIdAsync(id);
        }

        public async Task<List<Club>> GetClubsAsync()
        {
            ClubRepository clubRepository = new ClubRepository();
            return await clubRepository.GetClubsAsync();
        }

        public async Task<bool> InsertClubAsync(Club club)
        {
            ClubRepository clubRepository = new ClubRepository();
            return await clubRepository.InsertClubAsync(club);
        }


        public async Task<bool> UpdateClubByIdAsync(Guid id, Club club)
        {
            ClubRepository clubRepository = new ClubRepository();
            if (Object.ReferenceEquals(clubRepository.GetClubByIdAsync(id), null))
            {
                return false;
            }
            return await clubRepository.UpdateClubByIdAsync(id, club);
        }
    }
}
