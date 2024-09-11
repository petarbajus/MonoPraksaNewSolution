using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSolution.Common
{
    public class ClubFilter
    {
        public string SearchQuery {  get; set; }
        public string CharacteristicColor { get; set; }
        public DateOnly? DateOfFoundationFrom { get; set; }
        public DateOnly? DateOfFoundationTo { get; set; }

        public ClubFilter() { }

    }
}
