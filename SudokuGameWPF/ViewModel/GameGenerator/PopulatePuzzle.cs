using SudokuGameWPF.Model.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.GameGenerator
{
    internal class PopulatePuzzle
    {
        #region . Variables .

        private CellClass[] cells = new CellClass[81];

        #endregion

        #region . Methods .

        #region . Methods: Public .

        /// <summary>
        /// Create a new Sudoku puzzle.
        /// </summary>
        /// <returns>Return the puzzle generated.</returns>
        internal CellClass[,] GeneratePuzzle()
        {
            ClearCells();                                           // Clear the cells
            GenerateGrid();                                         // Generate a new puzzle
            return TransferGameToGrid();                            // Transfer the puzzle to the game grid
        }

        #endregion

        #region . Methods: Private .

        private void ClearCells()
        {
            for (Int32 i = 0; i < 81; i++)                          // Loop through the array
                cells[i] = null;                                   // Set each element to null
        }

        private void GenerateGrid()
        {
            List<Int32>[] available = InitializeArray();                        // Initialize the list
            Int32 index = 0;                                                    // Set the index pointer to zero
            do
            {
                if (available[index].Count > 0)                                 // If more number available
                {
                    Int32 i = RandomClass.GetRandomInt(available[index].Count); // Get a random number
                    CellClass item = new CellClass(index, available[index][i]); // Create a new 
                    if (Conflicts(item))                                        // Any conflicts with existing cells?
                    {
                        available[index].RemoveAt(i);                           // Yes, remove it from the available list
                        item = null;                                            // Clear the CellClass pointer
                    }
                    else
                    {                                                           // No conflicts
                        cells[index] = item;                                   // Save the CellClass to the array
                        item = null;                                            // Clear the CellClass pointer
                        available[index].RemoveAt(i);                           // Remove it from the available list
                        index++;                                                // Increment the index pointer
                    }
                }
                else
                {                                                               // No more number available
                    available[index] = InitArray();                             // Re-initialize the available list
                    index--;                                                    // To back one spot
                    cells[index] = null;                                       // Clear the array pointer
                }
            } while (index < 81);                                               // Keep looping until there are no more cells to process
        }

        private static List<Int32>[] InitializeArray()
        {
            List<Int32>[] available = new List<int>[81];                        // Instantiate a new array of lists
            for (Int32 i = 0; i < 81; i++)                                      // Loop through the array
                available[i] = InitArray();                                     // Initialize the element
            return available;                                                   // Return the list of arrays
        }

        private static List<Int32> InitArray()
        {
            List<Int32> retVal = new List<int>();                               // Instantiate a new list
            for (Int32 j = 1; j <= 9; j++)                                      // Loop through the answers
                retVal.Add(j);                                                  // Add it to the list
            return retVal;                                                      // Return the list
        }

        private bool Conflicts(CellClass check)
        {   // If the answer already exists in the column, row, or region, return true.
            foreach (CellClass item in cells)                                  // Loop through the list of cells
                if (item != null)                                               // If it's not null, check it
                    if ((item.IsSameRow(check) || item.IsSameCol(check) || item.IsSameRegion(check)) && (item.Answer == check.Answer))
                        return true;                                            // Return true if the answer already exists along the column, row, or region
            return false;                                                       // Return false if the answer does not exist
        }

        private CellClass[,] TransferGameToGrid()
        {
            CellClass[,] _cells = new CellClass[9, 9];                           // Initialize a new cell array
            foreach (CellClass item in cells)                                  // Loop the cell list
                _cells[item.Col, item.Row] = item;                               // Save the pointer
            return _cells;                                                       // Return the cell array
        }

        #endregion

        #endregion
    }
}
