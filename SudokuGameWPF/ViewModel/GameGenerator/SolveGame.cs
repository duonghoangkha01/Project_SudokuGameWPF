using SudokuGameWPF.Model.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.GameGenerator.Solver
{
    internal class SolveGame
    {
        #region . Methods .

        #region . Methods: Public .

        /// <summary>
        /// Solve the puzzle specified.
        /// </summary>
        /// <param name="cells">Puzzle to solve.</param>
        /// <returns>Returns a flag to indicate whether a solution was found or not.</returns>
        internal bool SolvePuzzle(CellClass[,] cells)
        {
            Int32[,] iTask = ConvertBoard(cells);                   // Convert board to a 2D array of integers
            SudokuArena cArena = new SudokuArena(iTask, 3, 3);      // Instantiate a new dancing arena 
            cArena.Solve();                                         // Now solve it
            return (cArena.Solutions == 1);                         // Return true if there is only one solution
        }

        #endregion

        #region . Methods: Private .

        private int[,] ConvertBoard(CellClass[,] cells)
        {
            Int32[,] iRet = new Int32[9, 9];                        // Initialize a new 9 x 9 array of integers
            for (Int32 col = 0; col < 9; col++)                     // Loop through the columns
                for (Int32 row = 0; row < 9; row++)                 // Loop through the rows
                {                                                   // Is the cell state answer?
                    if (cells[col, row].CellState == Model.Enums.CellStateEnum.Answer)
                        iRet[col, row] = cells[col, row].Answer;    // Yes, save the answer
                    else
                        iRet[col, row] = 0;                         // No, set it to zero
                }
            return iRet;                                            // Return the array
        }

        #endregion

        #endregion
    }
}
