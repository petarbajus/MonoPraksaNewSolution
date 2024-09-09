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
        bool InsertFootballer(Footballer footballer);
        bool DeleteFootballerById(Guid id);
        bool UpdateFootballerById(Guid id, Footballer footballer);
        Footballer GetFootballerById(Guid id);
        List<Footballer> GetFootballers();
    }
}
