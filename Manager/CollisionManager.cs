using Assignment.Characters;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Manager
{
    class CollisionManager
    {
        public static void CollisionBoundary(Unit unit, Rectangle _target)
        {

            if (IsTouchingRight(unit, _target))
            {
                unit.mapPos.X = unit.lastMapPos.X;
            }
            if (IsTouchingLeft(unit, _target))
            {
                unit.mapPos.X = unit.lastMapPos.X;
            }
            if (IsTouchingBottom(unit, _target))
            {
                unit.mapPos.Y = unit.lastMapPos.Y;
            }
            if (IsTouchingTop(unit, _target))
            {
                unit.mapPos.Y = unit.lastMapPos.Y;
            }

        }
        protected static bool IsTouchingLeft(Unit unit, Rectangle _target)
        {
            return unit.collider.colMove.Right + unit.currSpeed.X > _target.Left &&
              unit.collider.colMove.Left < _target.Left &&
              unit.collider.colMove.Bottom > _target.Top &&
              unit.collider.colMove.Top < _target.Bottom;
        }

        protected static bool IsTouchingRight(Unit unit, Rectangle _target)
        {
            return unit.collider.colMove.Left + unit.currSpeed.X < _target.Right &&
              unit.collider.colMove.Right > _target.Right &&
              unit.collider.colMove.Bottom > _target.Top &&
              unit.collider.colMove.Top < _target.Bottom;
        }

        protected static bool IsTouchingTop(Unit unit, Rectangle _target)
        {
            return unit.collider.colMove.Bottom + unit.currSpeed.Y > _target.Top &&
              unit.collider.colMove.Top < _target.Top &&
              unit.collider.colMove.Right > _target.Left &&
              unit.collider.colMove.Left < _target.Right;
        }

        protected static bool IsTouchingBottom(Unit unit, Rectangle _target)
        {
            return unit.collider.colMove.Top + unit.currSpeed.Y < _target.Bottom &&
              unit.collider.colMove.Bottom > _target.Bottom &&
              unit.collider.colMove.Right > _target.Left &&
              unit.collider.colMove.Left < _target.Right;
        }
    }
}
