using System;
using System.Collections.Generic;
using System.Text;

namespace Submarine_Game.models
{
    class Player
    {

        private int hitsCount;
        private int missCount;
        private int score;

        public Player()
        {
            this.hitsCount = 0;
            this.missCount = 0;
            this.score = 0;
        }

        public int GetScore() => this.score;
        public void AddScore(int sco) => this.score += sco;
        public int GetHitsCount() => this.hitsCount;
        public int GetMissCount() => this.missCount;
        public int GetTotal() => this.hitsCount+this.missCount;
        public void increaseHites() => this.hitsCount++;
        public void increaseMiss() => this.missCount++;
    }
}
