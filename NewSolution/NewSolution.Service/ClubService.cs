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
        public bool DeleteClubById(Guid id)
        {
            ClubRepository clubRepository = new ClubRepository();
            return clubRepository.DeleteClubById(id);
        }

        public Club GetClubById(Guid id)
        {
            ClubRepository clubRepository = new ClubRepository();
            return clubRepository.GetClubById(id);
        }

        public List<Club> GetClubs()
        {
            ClubRepository clubRepository = new ClubRepository();
            return clubRepository.GetClubs();
        }

        public bool InsertClub(Club club)
        {
            ClubRepository clubRepository = new ClubRepository();
            return clubRepository.InsertClub(club);
        }


        public bool UpdateClubById(Guid id, Club club)
        {
            ClubRepository clubRepository = new ClubRepository();
            if (Object.ReferenceEquals(clubRepository.GetClubById(id), null))
            {
                return false;
            }
            return clubRepository.UpdateClubById(id, club);
        }
    }
}
