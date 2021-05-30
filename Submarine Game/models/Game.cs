using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Submarine_Game.models
{
    /*
     * the class game managed the submarine game,
     * and have a board of the computer and player
     */
    class Game
    {
        private Player user;
        private Board board;


        static Random rand = new Random();

        public Game()
        {
            this.user = new Player();
        }

        public void SetBoard(Board board) => this.board = board;

        /*
         * the function get the level of the game and
         * shows the board to the user according to the level
         */
        private void InsertLevelGame()
        {
            Console.WriteLine("Select the level of the game by selecting a number from 1 to 5:");
            Console.WriteLine("1. Level 1 – the easiest – submarines revealed for 4 seconds.");
            Console.WriteLine("2. Level 2 - submarines revealed for 3 seconds.");
            Console.WriteLine("3. Level 3 – submarines revealed for 2 seconds.");
            Console.WriteLine("4. Level 4 – submarines revealed for 1 seconds.");
            Console.WriteLine("5. Level 5 – the game will not show its subs at all.");
            int levelGame = ChoosRowAndCol(1, 5);
            this.board.PrintBoard(true);
            Thread.Sleep(1000*(5-levelGame));
            Console.Clear();
        }

    
        /*
         * the function get range of integer and return the valide user input,
         * exit for the option that the user can input -1 to exit
         */
        private int ChoosRowAndCol(int min,int max ,bool exit = false)
        {
            int number;
            while (true)
            {
                try
                {
                    number = Convert.ToInt32(Console.ReadLine());
                    if (exit && number == -1)
                        return -1;
                    if (number < min || number > max)
                        throw new Exception();
                    return number;

                }
                catch (Exception)
                {
                    Console.WriteLine("Please choose number between "+min+" - " + max);
                }
            }
        }

        /*
         * managed the user attempted and update the board and the user statistics
         */
        public bool UserAttempted()
        {
            while (true)
            {
                Console.Write("Choose row: ");
                int row = ChoosRowAndCol(1,this.board.GetRows(),true);
                if (row == -1)
                    return false;
                Console.Write("Choose col: ");
                int col = ChoosRowAndCol(1,this.board.GetCols(),true);

                if (col == -1)
                    return false;
                bool? result = this.board.AttemptedToHit(row-1, col-1);

                if (result == true)
                {

                    Console.WriteLine($"Hite the enemy submarine in ({row},{col})");
                    this.user.increaseHites();
                    this.user.AddScore(10);
                    return true;
                }
                else if(result == false)
                {
                    Console.WriteLine($"Miss the enemy submarine in ({row},{col})");
                    this.user.increaseMiss();
                    this.user.AddScore(-5);
                    return true;
                }
                else
                    Console.WriteLine($"Already hite in ({row},{col}), choose deffrent location");
            }
        }
       


        /*
         * create random board by getting random number for row and column and input the first cell of the submarine,
         * the function choose random from cell from the option the end cells of the submarine 
         */
        public void CreateRandomBoard()
        {
           
            int[] numberofSubmarine = this.board.GetSubmarinesSizeAndAmount();
            for (int i = 1; i < numberofSubmarine.Length; i++)
            {
                for (int j = 1; j <= numberofSubmarine[i]; j++)
                {
                    int row = rand.Next(this.board.GetRows());
                    int col = rand.Next(this.board.GetCols());

                    List<Point> submarineEndLocation = this.board.GetOpstionsPointForSubmarine(row, col, i);
                    if (submarineEndLocation == null || submarineEndLocation.Count == 0)
                    {
                        j -= 1;
                        continue;
                    }
                    else if (submarineEndLocation.Count != 0)
                    {
                        if (i != 1)
                        {
                            
                            int chooseEndLocation = rand.Next(submarineEndLocation.Count);
                            this.board.AddSubmarine(submarineEndLocation[chooseEndLocation], row, col, i);
                        }
                    }
                }
            }
        }

        /*
         * print all the player statistics
         */
        private void GetStatistics()
        {
            Console.WriteLine("Player statistics:");
            Console.WriteLine("Given coordinates: "+ this.user.GetTotal());
            Console.WriteLine("Success hits: "+this.user.GetHitsCount());
            Console.WriteLine("Missed hits: "+this.user.GetMissCount());
            Console.WriteLine("Score: "+this.user.GetScore());

        }

        /*
         * manage the flow of the submarine game
         */
        public void RunGame()
        {
            Console.Write("Choose nuber of rows: ");
            int rows = ChoosRowAndCol(10, 50);
            Console.Write("Choose nuber of columns: ");
            int cols = ChoosRowAndCol(10, 50);
            if (rows != 10 && cols != 10)
                this.board = new Board(rows, cols);
            else
                this.board = new Board();
            // TODO: need to show the user amount of the submarine

            // need to build automatic the computer board
            Console.WriteLine("Insert all the submarine to the board");
            this.CreateRandomBoard();

            InsertLevelGame();

            while (!board.GameFinsh())
            {
                this.board.PrintBoard();
                Console.WriteLine("To quit the game insert to row or col -1");
                if (!this.UserAttempted())
                {
                    Console.Write("Are you sure you want to exit the game? (y/n)  ");
                    string answer = Console.ReadLine();
                    if (answer == "y" || answer == "Y")
                        break;
                }
            }
            Console.WriteLine("Last board:");
            this.board.PrintBoard(true);
            GetStatistics();

        }

    }

}
