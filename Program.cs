
using Battleship.Logic;
using Battleship.Model;

const int numberOfRows = 10;
const int numberOfColumns = 10;
const int numberOfFiveSquaresShips = 1;
const int numberOfFourSquaresShips = 2;
List<Ship> createdShips = new List<Ship>();
List<Tuple<char, int>> firedPositions = new List<Tuple<char, int>>();
BattleshipLogic logic = new BattleshipLogic(numberOfFiveSquaresShips, numberOfFourSquaresShips, numberOfRows, numberOfColumns, 
    ref firedPositions, ref createdShips);
logic.Init(ref createdShips);
logic.Play(ref createdShips);
