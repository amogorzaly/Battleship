using Battleship.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Logic
{
    internal class BattleshipLogic
    {
        private Random random;
        private int numberOfFiveSquaresShips;
        private int numberOfFourSquaresShips;
        private int numberOfColumns;
        private int numberOfRows;
        private List<Tuple<char, int>> firedPositions;
        private List<Ship> createdShips;
        public BattleshipLogic(int numberOfFiveSquaresShips, int numberOfFourSquaresShips, int numberOfRows, int numberOfColumns,
            ref List<Tuple<char, int>> firedPositions, ref List<Ship> createdShips)
        {
            random = new Random();
            this.numberOfFiveSquaresShips = numberOfFiveSquaresShips;
            this.numberOfFourSquaresShips = numberOfFourSquaresShips;
            this.numberOfRows = numberOfRows;
            this.numberOfColumns = numberOfColumns;
            this.firedPositions = firedPositions;
            this.createdShips = createdShips;
        }
        public void Init(ref bool[,] battlefield, ref List<Ship> createdShips)
        {
            for (int i = 0; i < numberOfFiveSquaresShips; i++)
            {
                bool hasShipBeenPlaced = false;
                do
                {
                    hasShipBeenPlaced = placeShip(5, ref battlefield, ref createdShips);
                }
                while (hasShipBeenPlaced == false);
            }
            for (int i = 0; i < numberOfFourSquaresShips; i++)
            {
                bool hasShipBeenPlaced = false;
                do
                {
                    hasShipBeenPlaced = placeShip(4, ref battlefield, ref createdShips);
                }
                while (hasShipBeenPlaced == false);
            }
        }
        //bool placeShip(int numberOfSquares, ref bool[,] battlefield, ref List<Ship> createdShips)
        //{
        //    bool wasShipPlaced = false;
        //    int initialPosition = 0;
        //    Ship newShip = new Ship();
        //    bool isHorizontal = Convert.ToBoolean(random.Next(2));
        //    if (isHorizontal)
        //    {
        //        int rowNumber = random.Next(numberOfRows);
        //        initialPosition = random.Next(numberOfColumns - numberOfSquares + 1);
        //        for (int j = initialPosition; j < numberOfSquares + initialPosition; j++)
        //        {
        //            if (battlefield[rowNumber, j] == true) return false;
        //        }
        //        for (int j = initialPosition; j < numberOfSquares + initialPosition; j++)
        //        {
        //            battlefield[rowNumber, j] = true;
        //            newShip.Coordinates.Add(new Tuple<int, int>(rowNumber, j));
        //        }
        //        wasShipPlaced = true;
        //    }
        //    else
        //    {
        //        int columnNumber = random.Next(numberOfColumns);
        //        initialPosition = random.Next(numberOfRows - numberOfSquares + 1);
        //        for (int i = initialPosition; i < numberOfSquares + initialPosition; i++)
        //        {
        //            if (battlefield[i, columnNumber] == true) return false;
        //        }
        //        for (int i = initialPosition; i < numberOfSquares + initialPosition; i++)
        //        {
        //            battlefield[i, columnNumber] = true;
        //            newShip.Coordinates.Add(new Tuple<int, int>(i, columnNumber));
        //        }
        //        wasShipPlaced = true;
        //    }
        //    createdShips.Add(newShip);
        //    return wasShipPlaced;
        //}
        bool placeShip(int numberOfSquares, ref bool[,] battlefield, ref List<Ship> createdShips)
        {
            bool wasShipPlaced = false;
            int initialPosition = 0;
            Ship newShip = new Ship();
            bool isHorizontal = Convert.ToBoolean(random.Next(2));
            if (isHorizontal)//horizontal
            {
                int rowNumber = random.Next(numberOfRows);
                initialPosition = random.Next(numberOfColumns - numberOfSquares + 1);
                for (int j = initialPosition; j < numberOfSquares + initialPosition; j++)
                {
                    if (createdShips.Any(x => x.Coordinates.Any(x => x.Item1 == Convert.ToChar(65 + j) && x.Item2 == rowNumber + 1)))
                    {
                        return false;
                    }
                    string dd = Convert.ToChar(65 + j) + Convert.ToString(rowNumber + 1);
                    newShip.Coordinates.Add(new Tuple<char, int>(Convert.ToChar(65 + j), rowNumber + 1));
                    wasShipPlaced = true;
                }
            }
            else //vertical
            {
                int columnNumber = random.Next(numberOfColumns);
                char column = Convert.ToChar(65 + columnNumber);
                initialPosition = random.Next(numberOfRows - numberOfSquares + 1);
                for (int i = initialPosition; i < numberOfSquares + initialPosition; i++)
                {
                    if (createdShips.Any(x => x.Coordinates.Any(x => x.Item1 == column && x.Item2 == i + 1)))
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
        public void drawBattlefield(ref List<Ship> createdShips)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\t");
            for (int j = 0; j < numberOfColumns; j++)
            {
                Console.Write(Convert.ToChar(65 + j) + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < numberOfRows; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Convert.ToString(i + 1) + "\t");
                Console.ForegroundColor = ConsoleColor.White;
                for (int j = 0; j < numberOfColumns; j++)
                {
                    if (!firedPositions.Any(x => x.Item1 == Convert.ToChar(65 + j) && x.Item2 == i + 1))
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    else if (createdShips.Any(x => !x.IsSunk && x.Coordinates.Any(x => x.Item1 == Convert.ToChar(65 + j) && x.Item2 == i + 1)))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("X ");
                    }
                    else if (createdShips.Any(x => x.IsSunk && x.Coordinates.Any(x => x.Item1 == Convert.ToChar(65 + j) && x.Item2 == i + 1)))
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
                drawBattlefield(ref createdShips);
                Console.Write("Enter target coordinates (e.g. A1): ");
                string typedCoordinates = Console.ReadLine();
                if (!isTypedCoordinateValid(typedCoordinates))
                {
                    Console.WriteLine("Invalid coordinates!");
                    continue;
                }
                Int32.TryParse(typedCoordinates.Substring(1), out int convertedCoordinate);
                firedPositions.Add(new Tuple<char, int>(typedCoordinates.ToUpper()[0], convertedCoordinate));
                hasTargetBeenHit();
            }
            while (createdShips.Any(x => !x.IsSunk));
            drawBattlefield(ref createdShips);
            Console.WriteLine("You won! Press any key to countinue.");
            Console.ReadKey();
        }
        private bool isTypedCoordinateValid(string? typedCoordinates)
        {
            if (string.IsNullOrWhiteSpace(typedCoordinates) || typedCoordinates.Length < 2
                || !Char.IsLetter(typedCoordinates[0]) || !Int32.TryParse(typedCoordinates.Substring(1), out int result)) return false;
            return true;
        }
        private bool hasTargetBeenHit()
        {
            var coordinates = firedPositions.Last();
            if (createdShips.Any(x => x.Coordinates.Any(x => x.Item1 == coordinates.Item1 && x.Item2 == coordinates.Item2)))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ship just has been hit!");                
                if (hasShipSunk())
                {
                    Console.WriteLine("You have destroyed the ship!");
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                return true;

            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Missed! Try again!");
            Console.ForegroundColor = ConsoleColor.White;
            return false;
        }
        private bool hasShipSunk()
        {
            foreach (var ship in createdShips.Where(x => !x.IsSunk))
            {
                bool isSunk = true;
                foreach (var coordinate in ship.Coordinates)
                {
                    if(!firedPositions.Any(x=>x.Item1 == coordinate.Item1 && x.Item2 == coordinate.Item2))
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
