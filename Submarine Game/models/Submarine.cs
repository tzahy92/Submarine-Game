using System;
using System.Collections.Generic;
using System.Text;


namespace Submarine_Game.models
{

    class Submarine
    {
        private List<Point> location;
        private List<Point> surrounded;
        private int size;
        private int countActive;

       

        public Submarine( int type)
        {
            this.location = new List<Point>();
            this.surrounded = new List<Point>();
            this.size = type;
            this.countActive = type;
        }

        public void AddLocation(Point p) => this.location.Add(p);

        public void AddSurrounded(Point p) => this.surrounded.Add(p);

        /*
         * update the amoutn acitve cell of the submarine
         */
        public void SecsuccessHit() => this.countActive--;

        public int GetCountActive() => this.countActive;

        /*
         * check if all submarine get hit
         */
        public bool GetActive() => this.countActive != 0;

        public List<Point> GetSurronded() => this.surrounded;

        public new int GetType() => this.size;

        public List<Point> GetLocation() => this.location;



    }
}
