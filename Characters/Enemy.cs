using Assignment.Scenes;
using Assignment.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Characters
{
    enum EnemyType
    {
        DarkKnight,
        Killer,
        Witch,
        Lunatic
    }
    abstract class Enemy : Unit
    {
        Character player;
        private double atkElaspedTime;
        protected float detectDistant = 1000, stopDistant = 100, atkRange = 300, atkSpeed = 2000;
        public Enemy(ContentManager _contentManager, Level _level) : base(_contentManager, _level)
        {
            hp = 100;
            player = _level.player;
            unitType = UnitType.Enemy;
        }
        #region Update
        public override void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            atkElaspedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            UpdateState();
            UpdateMovement(_ks);
            base.Update(gameTime, _ms, _ks);
        }
        private void UpdateState()
        {
            if (Vector2.Distance(mapPos, player.mapPos) <= atkRange &&
                player.currCharState != CharacterState.Die &&
                CurrMp >= 10)
            {
                currCharState = CharacterState.Attack;
                currRect = rectAtk;
                if (atkElaspedTime >= atkSpeed)
                {
                    UseSkill();
                    atkElaspedTime = 0;
                }
            }
            else if (currSpeed != Vector2.Zero)
            {
                currCharState = CharacterState.Walk;
                currRect = rectWalk;
            }
            else
            {
                currCharState = CharacterState.Idle;
                currRect = rectWalk;
            }

            lastCharState = currCharState;
        }
        private void UpdateMovement( KeyboardState _ks)
        {
            if (Vector2.Distance(mapPos, player.mapPos) <= detectDistant &&
                Vector2.Distance(mapPos, player.mapPos) >= stopDistant &&
                 player.currCharState != CharacterState.Die)
            {
                lastMapPos = mapPos;
                #region Move the Character & Change the Direction
                if (mapPos.X - 5 > player.mapPos.X)
                    currSpeed.X = -speed;
                else if (mapPos.X + 5 < player.mapPos.X)
                    currSpeed.X = speed;
                else currSpeed.X = 0;

                if (mapPos.Y - 5 > player.mapPos.Y)
                    currSpeed.Y = -speed;
                else if (mapPos.Y + 5 < player.mapPos.Y)
                    currSpeed.Y = speed;
                else currSpeed.Y = 0;

                #endregion
                ReachEdge();
                mapPos += currSpeed;
            }
            else currSpeed = Vector2.Zero;
        }
        #endregion
        protected void UseSkill()
        {
            Vector2 center = currRect.Size.ToVector2();
            center.X /= 2;
            center.Y /= 2;
            float rotAngle = (float)Math.Atan2(player.mapPos.Y - mapPos.Y, player.mapPos.X - mapPos.X);
            NewSkill(center, rotAngle);
        }
    }
}
