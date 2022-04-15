using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.Model.Enums
{
    public enum CellStateEnum
    {
        Blank,
        Answer,
        Hint,
        UserInputCorrect,
        UserInputIncorrect,
        Notes
    }
}
