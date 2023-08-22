using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model
{
    public class Ship
    {
        public Ship()
        {
            Coordinates = new List<Tuple<char, int>>();
        }
        public List<Tuple<char, int>> Coordinates { get; set; }
        public bool IsSunk { get; set; }           

    }
}
