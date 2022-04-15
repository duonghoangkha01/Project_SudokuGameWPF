using SudokuGameWPF.Model.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.CustomEventArgs
{
    internal class GameGeneratorEventArgs : EventArgs
    {
        #region . Constructors .

        /// <summary>
        /// Intializes a new instance of the GameGeneratorEventArgs class.
        /// </summary>
        /// <param name="cells">Two dimensional array of CellClass objects.</param>
        internal GameGeneratorEventArgs(CellClass[,] cells)
        {
            Cells = cells;                                      // Save the input parameter.
        }

        #endregion

        #region . Properties: Public Read-only .

        /// <summary>
        /// Gets the two dimensional array of CellClass objects that was saved in this instance.
        /// </summary>
        internal CellClass[,] Cells { get; private set; }

        #endregion
    }
}
