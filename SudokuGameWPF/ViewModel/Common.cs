using SudokuGameWPF.Model.Enums;
using SudokuGameWPF.Model.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel
{
    internal class Common
    {
        #region . Constant declarations .

        internal const Int32 MaxLevels = 5;                     // This value should correspond to the number of game levels (DifficultyLevels enum)

        #endregion

        #region . Public properties .

        /// <summary>
        /// Return true if the specified index is between 0 and 8.
        /// </summary>
        /// <param name="index">Index value to check.</param>
        /// <returns></returns>
        internal static bool IsValidIndex(Int32 index)
        {
            return ((0 <= index) && (index <= 8));
        }

        /// <summary>
        /// Return true if the specified column and row is between 0 and 8.
        /// </summary>
        /// <param name="col">Column value to check.</param>
        /// <param name="row">Row value to check.</param>
        /// <returns></returns>
        internal static bool IsValidIndex(Int32 col, Int32 row)
        {
            return (IsValidIndex(col) && IsValidIndex(row));
        }

        /// <summary>
        /// Return true if the specified CellIndex class is valid.
        /// </summary>
        /// <param name="uIndex">CellIndex call to check.</param>
        /// <returns></returns>
        internal static bool IsValidIndex(CellIndex uIndex)
        {
            if (uIndex != null)
                if (IsValidIndex(uIndex.Column, uIndex.Row))
                    return IsValidIndex(uIndex.Region);
            return false;
        }

        /// <summary>
        /// Return true if the specified value is between 1 and 9.
        /// </summary>
        /// <param name="value">Integer to check.</param>
        /// <returns></returns>
        internal static bool IsValidAnswer(Int32 value)
        {
            return ((1 <= value) && (value <= 9));
        }

        /// <summary>
        /// Return true if the specified object is a valid cell state enum.
        /// </summary>
        /// <param name="value">Object to check.</param>
        /// <returns></returns>
        internal static bool IsValidStateEnum(object value)
        {
            return Enum.IsDefined(typeof(CellStateEnum), value);
        }

        /// <summary>
        /// Return true if the specified object is a valid difficulty level.
        /// </summary>
        /// <param name="value">Object to check.</param>
        /// <returns></returns>
        internal static bool IsValidGameLevel(object value)
        {
            return Enum.IsDefined(typeof(DifficultyLevels), value);
        }

        #endregion
    }
}
