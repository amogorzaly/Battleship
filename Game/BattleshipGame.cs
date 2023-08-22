using Battleship.Model;

namespace Battleship.Game
{
    internal class BattleshipGame
    {
        private readonly Random _random;
        private readonly int _numberOfFiveSquaresShips;
        private readonly int _numberOfFourSquaresShips;
        private readonly int _numberOfColumns;
        private readonly int _numberOfRows;
        private readonly List<Tuple<char, int>> _firedPositions;
        private readonly List<Ship> _createdShips;
        public BattleshipGame(int numberOfFiveSquaresShips, int numberOfFourSquaresShips, int numberOfRows, int numberOfColumns,
            ref List<Tuple<char, int>> firedPositions, ref List<Ship> createdShips)
        {
            _random = new Random();
            this._numberOfFiveSquaresShips = numberOfFiveSquaresShips;
            this._numberOfFourSquaresShips = numberOfFourSquaresShips;
            this._numberOfRows = numberOfRows;
            this._numberOfColumns = numberOfColumns;
            this._firedPositions = firedPositions;
            this._createdShips = createdShips;
        }
        public void Init(ref List<Ship> createdShips)
        {
            for (int i = 0; i < _numberOfFiveSquaresShips; i++)
            {
                bool hasShipBeenPlaced;
                do
                {
                    hasShipBeenPlaced = PlaceShip(5, ref createdShips);
                }
                while (hasShipBeenPlaced == false);
            }
            for (int i = 0; i < _numberOfFourSquaresShips; i++)
            {
                bool hasShipBeenPlaced;
                do
                {
                    hasShipBeenPlaced = PlaceShip(4, ref createdShips);
                }
                while (hasShipBeenPlaced == false);
            }
        }

        private bool PlaceShip(int numberOfSquares, ref List<Ship> createdShips)
        {
            bool wasShipPlaced = false;
            int initialPosition;
            Ship newShip = new Ship();
            bool isHorizontal = Convert.ToBoolean(_random.Next(2));
            if (isHorizontal)//horizontal
            {
                int rowNumber = _random.Next(_numberOfRows);
                initialPosition = _random.Next(_numberOfColumns - numberOfSquares + 1);
                for (int j = initialPosition; j < numberOfSquares + initialPosition; j++)
                {
                    if (createdShips.Any(ship => ship.Coordinates.Any(coordinate => coordinate.Item1 == Convert.ToChar(65 + j) && coordinate.Item2 == rowNumber + 1)))
                    {
                        return false;
                    }
                    newShip.Coordinates.Add(new Tuple<char, int>(Convert.ToChar(65 + j), rowNumber + 1));
                    wasShipPlaced = true;
                }
            }
            else //vertical
            {
                int columnNumber = _random.Next(_numberOfColumns);
                char column = Convert.ToChar(65 + columnNumber);
                initialPosition = _random.Next(_numberOfRows - numberOfSquares + 1);
                for (int i = initialPosition; i < numberOfSquares + initialPosition; i++)
                {
                    if (createdShips.Any(ship => ship.Coordinates.Any(coordinate => coordinate.Item1 == column && coordinate.Item2 == i + 1)))
                    {
                        return false;
                    }
                    newShip.Coordinates.Add(new Tuple<char, int>(column, i + 1));
                    wasShipPlaced = true;
                }
            }
            createdShips.Add(newShip);
            return wasShipPlaced;
        }

        private void DrawBattlefield(ref List<Ship> createdShips)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\t");
            for (int j = 0; j < _numberOfColumns; j++)
            {
                Console.Write(Convert.ToChar(65 + j) + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < _numberOfRows; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Convert.ToString(i + 1) + "\t");
                Console.ForegroundColor = ConsoleColor.White;
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    if (!_firedPositions.Any(fired => fired.Item1 == Convert.ToChar(65 + j) && fired.Item2 == i + 1))
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    else if (createdShips.Any(ship => !ship.IsSunk && ship.Coordinates.Any(coordinate => coordinate.Item1 == Convert.ToChar(65 + j) && coordinate.Item2 == i + 1)))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("X ");
                    }
                    else if (createdShips.Any(ship => ship.IsSunk && ship.Coordinates.Any(coordinate => coordinate.Item1 == Convert.ToChar(65 + j) && coordinate.Item2 == i + 1)))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write("X ");
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();

            }
            Console.WriteLine();
        }
        public void Play(ref List<Ship> createdShips)
        {
            do
            {
                DrawBattlefield(ref createdShips);
                Console.Write("Enter target coordinates (e.g. A1): ");
                string? typedCoordinates = Console.ReadLine();
                if (!IsTypedCoordinateValid(typedCoordinates))
                {
                    Console.WriteLine("Invalid coordinates!");
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(typedCoordinates))
                {
                    Int32.TryParse(typedCoordinates.Substring(1), out int convertedCoordinate);
                    _firedPositions.Add(new Tuple<char, int>(typedCoordinates.ToUpper()[0], convertedCoordinate));
                    HasTargetBeenHit();
                }
            }
            while (createdShips.Any(x => !x.IsSunk));
            DrawBattlefield(ref createdShips);
            Console.WriteLine("You won! Press any key to continue.");
            Console.ReadKey();
        }
        private bool IsTypedCoordinateValid(string? typedCoordinates)
        {
            if (string.IsNullOrWhiteSpace(typedCoordinates) || typedCoordinates.Length < 2
                || !Char.IsLetter(typedCoordinates[0]) || !Int32.TryParse(typedCoordinates.Substring(1), out _)) return false;
            return true;
        }
        private void HasTargetBeenHit()
        {
            var coordinates = _firedPositions.Last();
            if (_createdShips.Any(ship => ship.Coordinates.Any(coordinate => coordinate.Item1 == coordinates.Item1 && coordinate.Item2 == coordinates.Item2)))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ship just has been hit!");                
                if (HasShipSunk())
                {
                    Console.WriteLine("You have destroyed the ship!");
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Missed! Try again!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        private bool HasShipSunk()
        {
            foreach (var ship in _createdShips.Where(x => !x.IsSunk))
            {
                bool isSunk = true;
                foreach (var coordinate in ship.Coordinates)
                {
                    if(!_firedPositions.Any(x=>x.Item1 == coordinate.Item1 && x.Item2 == coordinate.Item2))
                    {
                        isSunk = false;
                    }
                }
                if (isSunk)
                {
                    ship.IsSunk = true;
                    return true;
                }
            }
            return false;
        }
    }
}
