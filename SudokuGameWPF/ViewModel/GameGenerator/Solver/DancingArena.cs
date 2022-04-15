using SudokuGameWPF.ViewModel.GameGenerator.Solver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGameWPF.ViewModel.GameGeneretor.Solver
{
    internal abstract class DancingArena
    {
        #region . Variables .

        private DancingNode[] solutionsRows;
        private DancingColumn[] headerColumns;

        #endregion

        #region . Constructors .

        internal DancingArena(Int32 primary, Int32 secondary)
        {
            InitClass(primary, secondary);

            for (Int32 i = 0; i < primary; i++)
            {
                solutionsRows[i] = null;
                headerColumns[i] = new DancingColumn(i + 1);
                headerColumns[i].Right = null;
                if (i > 0)
                {   // Connect the current column to the previous column.
                    headerColumns[i].Left = headerColumns[i - 1];
                    headerColumns[i - 1].Right = headerColumns[i];
                }
            }

            // Connect first and last primary column to the root node.
            headerColumns[0].Left = Root;
            Root.Right = headerColumns[0];
            headerColumns[primary - 1].Right = Root;
            Root.Left = headerColumns[primary - 1];

            // Adding self referential secondary columns.
            if (secondary > 0)
                for (Int32 i = 0; i < secondary; i++)
                    headerColumns[primary + i] = new DancingColumn(primary + i + 1);
        }

        internal DancingArena(Int32 columns)
            : this(columns, 0)
        { }

        #endregion

        #region . Properties: Private .

        private Int32 Initial { get; set; }
        private DancingColumn Root { get; set; }
        private Int32 Rows { get; set; }
        private DancingColumn FirstColumn
        {
            get
            {
                return (DancingColumn)Root.Right;
            }
        }
        private DancingColumn LastColumn
        {
            get
            {
                return (DancingColumn)Root.Left;
            }
        }

        #endregion

        #region . Methods .

        #region . Methods: Public .

        internal DancingNode AddRow(Int32[] positions)
        {
            DancingNode result = null;
            if (positions.Length > 0)
            {
                bool found;
                DancingNode thisNode = null;
                DancingNode prevNode = null;
                Rows++;
                for (Int32 i = 0; i < positions.Length; i++)
                {
                    if (positions[i] > 0)
                    {
                        thisNode = new DancingNode(Rows, positions[i]);
                        thisNode.Left = prevNode;
                        thisNode.Right = null;
                        if (prevNode != null)
                            prevNode.Right = thisNode;
                        else
                            result = thisNode;
                        found = false;
                        for (Int32 j = 0; j < headerColumns.Length; j++)
                        {
                            DancingColumn col = headerColumns[j];
                            if (col.Col == positions[i])
                            {
                                thisNode.Header = col;
                                col.AddNode(thisNode);
                                found = true;
                            }
                        }
                        if (!found)
                            Debug.WriteLine("Can't find header for {0}.", positions[i]);
                        prevNode = thisNode;
                        result.Left = prevNode;
                        prevNode.Right = result;
                    }
                }
            }
            return result;
        }

        internal void Solve()
        {
            SolveRecurse(Initial);
        }

        internal void RemoveKnown(List<DancingNode> solutions)
        {
            for (Int32 i = 0; i < solutions.Count; i++)
            {
                DancingNode row = solutions[i];
                solutionsRows[Initial] = row;
                Initial++;
                CoverColumn(row.Header);
                DancingNode col = row.Right;
                while (Equals(col, row) == false)
                {
                    CoverColumn(col.Header);
                    col = col.Right;
                }

            }
        }

        internal void ShowState()
        {
            DancingColumn col = FirstColumn;
            while (Equals(col, Root) == false)
            {
                Debug.WriteLine("C : {0}", col.ToString());
                DancingNode row = col.Lower;
                while (Equals(row, col) == false)
                {
                    Debug.WriteLine("  R : {0}", row.ToString());
                    row = row.Lower;
                }
                col = (DancingColumn)col.Right;
            }
        }

        internal abstract void HandleSolution(DancingNode[] rows);

        #endregion

        #region . Methods: Private .

        private void InitClass(Int32 primary, Int32 secondary)
        {
            Rows = 0;
            Initial = 0;
            Root = new DancingColumn(0);

            // Only primary columns form the solution.
            solutionsRows = new DancingNode[primary];
            headerColumns = new DancingColumn[primary + secondary];
        }

        private void CoverColumn(DancingColumn column)
        {
            column.Left.Right = column.Right;
            column.Right.Left = column.Left;
            DancingNode row = column.Lower;

            while (Equals(row, column) == false)
            {
                DancingNode col = row.Right;
                while (Equals(col, row) == false)
                {
                    col.Upper.Lower = col.Lower;
                    col.Lower.Upper = col.Upper;
                    col.Header.DecRows();
                    col = col.Right;
                }
                row = row.Lower;
            }
        }

        private void UncoverColumn(DancingColumn column)
        {
            DancingNode row = column.Upper;
            while (Equals(row, column) == false)
            {
                DancingNode col = row.Left;
                while (Equals(col, row) == false)
                {
                    col.Upper.Lower = col;
                    col.Lower.Upper = col;
                    col.Header.IncRows();
                    col = col.Left;
                }
                row = row.Upper;
            }
            column.Left.Right = column;
            column.Right.Left = column;
        }

        private DancingColumn NextColumn()
        {
            DancingColumn result = (DancingColumn)Root.Right;
            Int32 minRows = result.Rows;
            DancingColumn scanner = (DancingColumn)Root.Left;
            while (Equals(scanner, Root.Right) == false)
            {
                if (scanner.Rows < minRows)
                {
                    result = scanner;
                    minRows = scanner.Rows;
                }
                scanner = (DancingColumn)scanner.Left;
            }
            return result;
        }

        private void SolveRecurse(Int32 index)
        {
            if (Equals(Root, Root.Right))
                HandleSolution(solutionsRows);             // No more columns, we found one solution.
            else
            {
                DancingColumn nextCol = NextColumn();       // Select next column using some selection algorithm.
                CoverColumn(nextCol);                       // Exclude selected column from the matrix.
                DancingNode row = nextCol.Lower;            // Try for each row in selected column.
                while (Equals(row, nextCol) == false)
                {
                    solutionsRows[index] = row;            // Add row to solutions array.
                    DancingNode col = row.Right;            // Exclude all columns covered by this row
                    while (Equals(col, row) == false)
                    {
                        CoverColumn(col.Header);
                        col = col.Right;
                    }
                    SolveRecurse(index + 1);                // And try to solve the reduced matrix.

                    col = row.Left;                         // Now, restore all columns covered by this row.
                    while (Equals(col, row) == false)
                    {
                        UncoverColumn(col.Header);
                        col = col.Left;
                    }
                    solutionsRows[index] = null;           // And remove row from solution array.
                    row = row.Lower;
                }
                UncoverColumn(nextCol);                     // Return excluded column back to list.
            }
        }

        #endregion

        #endregion
    }
}
