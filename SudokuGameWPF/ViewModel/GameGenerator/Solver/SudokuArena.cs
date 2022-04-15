using SudokuGameWPF.ViewModel.GameGeneretor.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.GameGenerator.Solver
{
    internal class SudokuArena : DancingArena
    {
        #region . Constructors .

        internal SudokuArena(Int32[,] puzzle, Int32 boxRows, Int32 boxCols)
            : base(puzzle.Length * 4)
        {
            Solutions = 0;
            Size = puzzle.GetLength(0);
            Int32[] positions = new Int32[4];
            List<DancingNode> known = new List<DancingNode>();
            for (Int32 row = 0; row < Size; row++)
                for (Int32 col = 0; col < Size; col++)
                {
                    Int32 boxRow = row / boxRows;
                    Int32 boxCol = col / boxCols;
                    for (Int32 digit = 0; digit < Size; digit++)
                    {
                        bool isGiven = (puzzle[row, col] == (digit + 1));
                        positions[0] = 1 + (row * Size + col);
                        positions[1] = 1 + puzzle.Length + (row * Size + digit);
                        positions[2] = 1 + 2 * puzzle.Length + (col * Size + digit);
                        positions[3] = 1 + 3 * puzzle.Length + ((boxRow * boxRows + boxCol) * Size + digit);
                        DancingNode newRow = AddRow(positions);
                        if (isGiven)
                            known.Add(newRow);
                    }
                }
            RemoveKnown(known);
        }

        #endregion

        #region . Properties .

        #region . Properties: Public Read-only.

        internal Int32 Solutions { get; private set; }

        #endregion

        #region . Properties: Private .

        private Int32 Size { get; set; }

        #endregion

        #endregion

        #region . Methods: Public .

        internal override void HandleSolution(DancingNode[] rows)
        {
            Solutions++;
        }

        #endregion
    }
}
