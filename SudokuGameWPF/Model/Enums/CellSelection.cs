using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.Model.Enums
{
    public enum CellSelection
    {
        Selected,
        SameRow,
        SameCol,
        SameRegion,
        DifferentRowColRegionButSameNumber,
        Nothing
    }
}
