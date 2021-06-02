using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Scenes
{
    class GamePeriod
    {
        public bool isStart;
        public bool isSpawn;
        public int numOfEnemy;
        public double periodTime;
        public GamePeriod(int _numOfEnemy = 1, double _periodTime = 2)
        {
            numOfEnemy = _numOfEnemy;
            periodTime = _periodTime;
        }
    }
}
