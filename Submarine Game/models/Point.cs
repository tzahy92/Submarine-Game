using System;
using System.Collections.Generic;
using System.Text;


namespace Submarine_Game.models
{
    /*
     * class Point representing coordinates of x and y
     */
    class Point
    {

        public int X;
        public int Y;

        public Point()
        {
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            return point != null &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override string ToString()
        {
            return "row = "+this.X+" col = "+this.Y ;
        }


    }
}
