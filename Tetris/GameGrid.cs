using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public  class GameGrid
    {
        private readonly int[,] grid;
        public int Rows { get; set; }
        public int Columns { get; set; }

        public int this[int row, int column]
        {
            get { return grid[row, column]; }
            set { grid[row, column] = value; }
        }

        public GameGrid(int rows = 22, int columns = 10)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[Rows, Columns];
        }

        public bool CheckInside(int row, int column)
        {
            if (row >= 0 && column >= 0 && row < Rows && column < Columns)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckEmpty(int row, int column)
        {
            if (CheckInside(row, column) && grid[row, column] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckRowFull(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckRowEmpty(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearRow(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row, column] = 0;

            }
        }

        private void MoveDownRow(int row, int clearedRows)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row + clearedRows, column] = grid[row,column];
                grid[row, column] = 0; 
            }
        }

        public int ClearFullRow()
        {
            int clearedRows = 0;
            for (int row = Rows-1 ; row >=0; row--)
            {
                if (CheckRowFull(row))
                {
                    ClearRow(row);
                    clearedRows++;
                }else if(clearedRows > 0)
                {
                    MoveDownRow(row, clearedRows);
                }
            }
            return clearedRows;
        }
    }
}
