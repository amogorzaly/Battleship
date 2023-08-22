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
