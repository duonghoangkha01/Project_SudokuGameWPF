using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.GameGenerator.Solver
{
    internal class DancingNode
    {
        #region . Constructors .

        internal DancingNode(Int32 row, Int32 col)
        {
            Left = this;
            Right = this;
            Upper = this;
            Lower = this;
            Header = null;
            Row = row;
            Col = col;
        }

        #endregion

        #region . Properties: Public .

        internal DancingNode Left { get; set; }
        internal DancingNode Right { get; set; }
        internal DancingNode Upper { get; set; }
        internal DancingNode Lower { get; set; }
        internal DancingColumn Header { get; set; }
        internal Int32 Row { get; set; }
        internal Int32 Col { get; set; }

        #endregion

        #region . Methods .

        #region . Methods: Public .

        public override string ToString()
        {
            return string.Format("Node({0}), left({1}), right({2}), upper({3}), lower({4}), header({5})", Name(), Name(Left), Name(Right), Name(Upper), Name(Lower), Name(Header));
        }

        #endregion

        #region . Methods: Private .

        private static string Name(DancingNode node)
        {
            if (node == null)
                return "NULL";
            return node.Name();
        }

        private string Name()
        {
            return string.Format("R{0}, C{1}", Row, Col);
        }

        #endregion

        #endregion
    }
}
