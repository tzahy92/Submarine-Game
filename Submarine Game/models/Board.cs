using System;
using System.Collections.Generic;
using System.Text;

namespace Submarine_Game.models
{
    class Board
    {
        private int[,] boardeMatrix;
        private int rows;
        private int cols;
        private int countDropp;
        private int[] submarinesSizeAndAmount;
        private List<Submarine> submarines;

        public Board()
        {

            this.rows = 10;
            this.cols = 10;
            SetSubmarineSizeAndAmount();
            this.boardeMatrix = new int[this.rows, this.cols];
            this.submarines = new List<Submarine>();
            this.countDropp = 0;
        }

        public Board(int row, int col)
        {
            this.rows = row;
            this.cols = col;
            this.boardeMatrix = new int[this.rows, this.cols];
            SetSubmarineSizeAndAmount();
            this.submarines = new List<Submarine>();
            this.countDropp = 0;
        }

        public List<Submarine> GetSubmarine() => this.submarines;

        /*
         * checking for active submarines
         */
        public bool GameFinsh() => this.countDropp == this.submarines.Count;

        public int[] GetSubmarinesSizeAndAmount() => this.submarinesSizeAndAmount;

        public int[,] GetBoarderMatrix() => this.boardeMatrix;

        public int GetRows() => this.rows;

        public int GetCols() => this.cols;

        /*
         * get valide row and col and check if the user hit submarine, sea, surround 
         * and if the submarine destroyed and update the matrix board
         */
        public bool? AttemptedToHit(int row, int col)
        {
            if (this.boardeMatrix[row, col] == (int)Cell.Submarine)
            {
                this.boardeMatrix[row, col] = (int)Cell.Bullseye;
                foreach (Submarine sub in this.submarines)
                    if (sub.GetLocation().Contains(new Point(row, col)))
                    {
                        sub.SecsuccessHit();
                        if (!sub.GetActive())
                        {
                            this.DroppingSubmarine(sub);
                            this.countDropp++;
                        }
                        break;
                    }
                return true;
            }
            else if (this.boardeMatrix[row, col] == (int)Cell.Sea || this.boardeMatrix[row, col] == (int)Cell.Damage)
            {
                this.boardeMatrix[row, col] = (int)Cell.Miss;
                return false;
            }
            return null;
        }

        /*
         * update all the submarine and serround submarine cell in the board
         */
        private void DroppingSubmarine(Submarine sub)
        {
            foreach (Point p in sub.GetLocation())
                sub.GetSurronded().Remove(p);
            foreach(Point p in sub.GetSurronded())
            {
                this.boardeMatrix[p.X, p.Y] = (int)Cell.Dropp;
            }
        }

        /*
         * add submarine to the board and update the submarine location and serround cell,
         * and add the submarin to the list
         */
        public void AddSubmarine(Point end,int selectedRow,int selectedCol,int submarineSize)
        {
            int startR = end.X-1 > selectedRow ? selectedRow : end.X-1;
            int startC = end.Y-1 > selectedCol ? selectedCol : end.Y-1;

            Submarine subm = new Submarine(submarineSize);
            for(int i = 0; i < submarineSize; i++)
            {
                if(end.X-1 == selectedRow)
                {
                    this.boardeMatrix[startR, startC + i] = (int)Cell.Submarine;
                    subm.AddLocation(new Point(startR, startC + i));
                    this.AddSubmarineSurrounded(startR,startC+i,subm);

                }
                else
                {
                    this.boardeMatrix[startR + i, startC] = (int)Cell.Submarine;
                    subm.AddLocation(new Point(startR + i, startC));
                    this.AddSubmarineSurrounded(startR+i, startC , subm);
                }
                this.submarines.Add(subm);


            }

        }

        /*
         * add srrounded cell to the submarine and update the cell value in the board
         */
        private void AddSubmarineSurrounded(int row,int col, Submarine subm)
        {
            for (int i = -1; i < 2; i++)
            {
                if (row + i >= 0)
                {
                    if (row + i == this.rows)
                        break;
                    if (col - 1 >= 0 && this.boardeMatrix[row + i, col - 1] != (int)Cell.Submarine)
                    {
                        subm.AddSurrounded(new Point(row + i, col - 1));
                        this.boardeMatrix[row + i, col -1] = (int)Cell.Damage;
                    }
                    if (this.boardeMatrix[row + i, col] != (int)Cell.Submarine)
                    {
                        subm.AddSurrounded(new Point(row + i, col));
                        this.boardeMatrix[row + i, col] = (int)Cell.Damage;

                    }
                    if (col + 1 < this.cols && this.boardeMatrix[row + i, col + 1] != (int)Cell.Submarine)
                    {
                        subm.AddSurrounded(new Point(row + i, col + 1));
                        this.boardeMatrix[row + i, col + 1] = (int)Cell.Damage;

                    }
                }
            }
        }

        /*
         * get first cell of the submarine, size and return valide option of end cells of the submarine
         */
        public List<Point> GetOpstionsPointForSubmarine(int row,int col,int submarineSize)
        {
            List<Point> endSubmarine = new List<Point>();
            if (this.boardeMatrix[row , col ] == (int)Cell.Sea)
            {
                if (submarineSize == 1 && CheckSurroundedValidation(row,col))
                {
                    this.boardeMatrix[row, col] = (int)Cell.Submarine;
                    endSubmarine.Add(new Point(row + 1, col + 1));
                    Submarine sub = new Submarine(submarineSize);
                    sub.AddLocation(new Point(row , col ));
                    this.submarines.Add(sub);
                    this.AddSubmarineSurrounded(row, col, sub);
                    return endSubmarine;
                }
                endSubmarine = CheckValidation(row, col, submarineSize);
                return endSubmarine;
            }
            return null;

        }

        /*
         * checks the validation of 4 cells of the submarine end
         * and return list of valid cells
         */
        private List<Point> CheckValidation(int row,int col,int submarineSize)
        {
            List<Point> result = new List<Point>();
            Point[] endSubmarine = new Point[4];

            endSubmarine[0] = CheckOpenCellToUp(row, col, row - submarineSize);
            endSubmarine[1] = CheckOpenCellToRight(row, col, col + submarineSize);
            endSubmarine[2] = CheckOpenCellToDown(row, col, row + submarineSize);
            endSubmarine[3] = CheckOpenCellToLeft(row, col, col - submarineSize);
            int count = 0;
            for(int i =0;i < 4; i++)
            {
                if (endSubmarine[i] != null)
                    result.Add(endSubmarine[i]);
            }
            return result;
        }

        /*
         * Checks if the cells and the serrounded cells are free in the Right direction and returns the cell 
         * 
         */
        private Point CheckOpenCellToRight(int row,int startCol,int lastCol)
        {
            for(int i = startCol; i < lastCol; i++)
            {
                if (i >= this.cols ||  !CheckSurroundedValidation(row,i))
                    return null;
            }
            return new Point(row+1, lastCol);
        }

        /*
         * Checks if the cells and the serrounded cells are free in the Left direction and returns the cell 
         * 
         */
        private Point CheckOpenCellToLeft(int row, int startCol, int lastCol)
        {
            for (int i = startCol; i > lastCol; i--)
            {
                if (i < 0 || !CheckSurroundedValidation(row,i))
                    return null;
            }
            return new Point(row+1, lastCol+2);
        }

        /*
         * Checks if the cells and the serrounded cells are free in the Down direction and returns the cell 
         * 
         */
        private Point CheckOpenCellToDown(int startRow, int col, int latRow)
        {
            for (int i = startRow; i < latRow; i++)
            {
                if (i >= this.rows || !CheckSurroundedValidation(i, col))
                    return null;
            }
            return new Point(latRow, col+1);
        }

        /*
         * Checks if the cells and the serrounded cells are free in the Up direction and returns the cell 
         * 
         */
        private Point CheckOpenCellToUp(int startRow, int col, int latRow)
        {
            for (int i = startRow; i > latRow; i--)
            {
                if (i < 0 || !CheckSurroundedValidation(i,col))
                    return null;
            }
            return new Point(latRow+2, col+1);
        }

        /*
         * Checks if the serrounded cells are free  
         * 
         */
        private bool CheckSurroundedValidation(int row,int col)
        {
           for(int i=-1; i < 2; i++)
            {
                if(row+i >=0)
                {
                    if (row + i == this.rows)
                        break;
                    if (col - 1 >= 0 && this.boardeMatrix[row + i, col-1] == (int)Cell.Submarine)
                        return false;
                    if (this.boardeMatrix[row + i, col] == (int)Cell.Submarine)
                        return false;
                    if (col + 1 < this.cols && this.boardeMatrix[row + i, col + 1] == (int)Cell.Submarine)
                        return false;
                }
            }
            
            return true;
        }

        /*
         * get the number and size of the submarine in the game.
         * 
         */
        public void SetSubmarineSizeAndAmount()
        {
            int area = this.rows * this.cols;
            if(area < (20*20))
                this.submarinesSizeAndAmount = new int[] { 0, 4, 3, 2, 1 };
            else if (area < (30 * 30))
                this.submarinesSizeAndAmount = new int[] { 0, 5, 4, 3, 2 };
            else if (area < (40 * 40))
                this.submarinesSizeAndAmount = new int[] { 0, 5, 4, 3, 2, 1 };
            else if (area < (50 * 50))
                this.submarinesSizeAndAmount = new int[] { 0, 6, 5, 4, 3, 2 };
            else
                this.submarinesSizeAndAmount = new int[] { 0, 6, 5, 4, 3, 2, 1};

        }

        /*
         * print the board by the value of the cells
         */
        public void PrintBoard(bool firstTime = false)
        {
            Console.Write("    ");
            for(int i = 0; i < this.cols;i++)
            {
                if (i < 9)
                {
                    Console.Write(" " + (i + 1) + " |");
                }
                else
                {
                    Console.Write((i + 1) + " |");
                }
            }
            Console.WriteLine();
            for (int i = 0; i < this.rows; i++)
            {
                Console.Write("   ");
                for (int j = 0; j < this.cols; j++)
                {
                    Console.Write("----");
                }
                if (i < 9)
                {
                    Console.Write("\n" + (i + 1) + " ");
                }
                else
                {
                    Console.Write("\n" + (i + 1));
                }
                for (int j = 0; j < this.cols; j++)
                {
                    char cell = ' ';
                    Console.Write(" | " );
                    if(firstTime && this.boardeMatrix[i, j] == (int)Cell.Submarine)
                        cell = 'O' ;
                    else if (this.boardeMatrix[i, j] == (int)Cell.Bullseye)
                    {
                        cell = 'X';
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (this.boardeMatrix[i, j] == (int)Cell.Miss )
                    {
                        cell = 'X';
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if((firstTime &&  this.boardeMatrix[i, j] == (int)Cell.Damage) || this.boardeMatrix[i, j] == (int)Cell.Dropp )
                    {
                        cell = 'X';
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(cell);
                    Console.ForegroundColor = ConsoleColor.Gray;


                }

                Console.WriteLine(" |");
            }
            Console.WriteLine();
        }
        
    }
}
