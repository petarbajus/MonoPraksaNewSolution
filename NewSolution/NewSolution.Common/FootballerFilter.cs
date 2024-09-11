using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSolution.Common
{
    public class FootballerFilter
    {
        
        public string SearchQuery { get; set; }
        public DateOnly? DOBFrom { get; set; }
        public DateOnly? DOBTo { get; set; }
        
        public Guid? ClubId { get; set; }

        public FootballerFilter()
        {
            
        }
    }
}
