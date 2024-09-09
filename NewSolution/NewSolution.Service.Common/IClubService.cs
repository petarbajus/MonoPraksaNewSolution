using NewSolution.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSolution.Service.Common
{
    public interface IClubService
    {
        bool InsertClub(Club club);
        bool DeleteClubById(Guid id);
        bool UpdateClubById(Guid id, Club club);
        Club GetClubById(Guid id);
        List<Club> GetClubs();
    }
}
