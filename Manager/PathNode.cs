using Assignment.Characters;
using Assignment.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Manager
{
    class PathNode
    {
        Direction direction;
        float distant;
        Vector2 position;
        Rectangle detector;
        Level level;
        bool isEnd;
        List<PathNode> nextNode = new List<PathNode>();
        public PathNode(Direction _direction, Vector2 _position, Level _level, Unit _ai)
        {
            direction = _direction;
            position = _position;
            level = _level;
            detector = _ai.currRect;
            detector.Location = _position.ToPoint();
        }
        public void AddPath()
        {
            
        }
        private void CheckToUp()
        {
            bool isBreak = false;
            while (detector.Location.ToVector2() != level.player.mapPos && !isBreak)
            {
                foreach (Wall wall in level.map.walls)
                {
                    if (IsTouchingTop(wall))
                    {
                        //End Process
                        isBreak = true;
                        break;
                    }
                    else if (!IsTouchingBottom(wall))
                    {

                    }
                }
                detector.Y -= level.tileDensity;
            }
        }
        protected bool IsTouchingLeft(Wall wall)
        {
            return detector.Right > wall.rectangle.Left &&
              detector.Left < wall.rectangle.Left &&
              detector.Bottom > wall.rectangle.Top &&
              detector.Top < wall.rectangle.Bottom;
        }

        protected bool IsTouchingRight(Wall wall)
        {
            return detector.Left< wall.rectangle.Right &&
              detector.Right > wall.rectangle.Right &&
              detector.Bottom > wall.rectangle.Top &&
              detector.Top < wall.rectangle.Bottom;
        }

        protected bool IsTouchingTop(Wall wall)
        {
            return detector.Bottom > wall.rectangle.Top &&
              detector.Top < wall.rectangle.Top &&
              detector.Right > wall.rectangle.Left &&
              detector.Left < wall.rectangle.Right;
        }

        protected bool IsTouchingBottom(Wall wall)
        {
            return detector.Top < wall.rectangle.Bottom &&
              detector.Bottom > wall.rectangle.Bottom &&
              detector.Right > wall.rectangle.Left &&
              detector.Left < wall.rectangle.Right;
        }

    }
}
