using SudokuGameWPF.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.CustomEventArgs
{
    internal class GameManagerEventArgs : EventArgs
    {
        #region . Constructors .

        /// <summary>
        /// Initializes a new instance of the GameManagerEventArgs class.
        /// </summary>
        /// <param name="level">Difficulty level of the count.</param>
        /// <param name="count">Number of games generated and saved.</param>
        internal GameManagerEventArgs(DifficultyLevels level, Int32 count)
        {
            Level = level;                                  // Save the input paramters.
            Count = count;
        }

        #endregion

        #region . Properties: Public Read-only .

        /// <summary>
        /// Gets the difficulty level this count belongs to.
        /// </summary>
        internal DifficultyLevels Level { get; private set; }
        /// <summary>
        /// Gets the number of games that were generated and saved.
        /// </summary>
        internal Int32 Count { get; private set; }

        #endregion
    }
}
