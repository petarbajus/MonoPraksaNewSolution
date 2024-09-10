using NewSolution.Model;
using NewSolution.Repository;
using NewSolution.Repository.Common;
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
        private IClubRepository _clubRepository;
        public ClubService(IClubRepository clubRepository) {
            _clubRepository = clubRepository;
        }
        public async Task<bool> DeleteClubByIdAsync(Guid id)
        {

            return await _clubRepository.DeleteClubByIdAsync(id);
        }

        public async Task<Club> GetClubByIdAsync(Guid id)
        {
            
            return await _clubRepository.GetClubByIdAsync(id);
        }

        public async Task<List<Club>> GetClubsAsync()
        {
            
            return await _clubRepository.GetClubsAsync();
        }

        public async Task<bool> InsertClubAsync(Club club)
        {
            club.Id = Guid.NewGuid();
            return await _clubRepository.InsertClubAsync(club);
        }


        public async Task<bool> UpdateClubByIdAsync(Guid id, Club club)
        {
            
            if (Object.ReferenceEquals(_clubRepository.GetClubByIdAsync(id), null))
            {
                return false;
            }
            return await _clubRepository.UpdateClubByIdAsync(id, club);
        }
    }
}
