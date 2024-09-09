using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSolution.Model
{
    public class Club
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? CharacteristicColor { get; set; }
        public DateOnly? FoundationDate { get; set; }


        public Club()
        {

        }
        public Club(string? name, string? characteristicColor, DateOnly? foundationDate)
        {
            Name = name;
            CharacteristicColor = characteristicColor;
            FoundationDate = foundationDate;
        }


    }
}
