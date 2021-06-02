using Assignment.Scenes;
using Assignment.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Characters
{
    enum CharacterType
    {
        Berserker,
        Knight,
        Mage,
        Soldier
    }
    abstract class Character : Unit
    {
        protected CharacterType characterType;
        protected ButtonState lastBs;
        #region Constructor
        public Character(ContentManager _contentManager, Level _level) : base(_contentManager, _level)
        {
            unitType = UnitType.Player;
        }
        #endregion
        public override void LoadContent()
        {
            base.LoadContent();
        }
        #region Update
        public override void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            UpdateIntput(_ks, _ms);
            base.Update(gameTime, _ms, _ks);
            lastCharState = currCharState;
        }
        private void UpdateIntput(KeyboardState _ks, MouseState _ms)
        {
            lastMapPos = mapPos;
            #region Move the Character
            if (_ks.IsKeyDown(Keys.A) || _ks.IsKeyDown(Keys.Left))
                currSpeed.X = -speed;
            else if (_ks.IsKeyDown(Keys.D) || _ks.IsKeyDown(Keys.Right))
                currSpeed.X = speed;
            else currSpeed.X = 0;
            if (_ks.IsKeyDown(Keys.W) || _ks.IsKeyDown(Keys.Up))
                currSpeed.Y = -speed;
            else if (_ks.IsKeyDown(Keys.S) || _ks.IsKeyDown(Keys.Down))
                currSpeed.Y = speed;
            else currSpeed.Y = 0;
            #endregion

            ReachEdge();

            #region Update Character State
            if (_ms.RightButton==ButtonState.Pressed)
            {
                currCharState = CharacterState.Attack;
                currRect = rectAtk;
                currSpeed = currSpeed/ 3 * 2;
            }
            else if (currSpeed != Vector2.Zero)
            {
                currCharState = CharacterState.Walk;
                currRect = rectWalk;
                if (_ks.IsKeyDown(Keys.Space))
                {
                    currSpeed *= 1.5f;
                    CurrMp -= 0.5f;
                }
            }
            else
            {
                currCharState = CharacterState.Idle;
                currRect = rectWalk;
                currSpeed = Vector2.Zero;
            }
            #endregion
            mapPos += currSpeed;
            if (_ms.RightButton == ButtonState.Released && lastBs == ButtonState.Pressed)
                {
                UseSkill(_ms.Position.ToVector2());
            }
            lastBs = _ms.RightButton;
        }
        #endregion
        protected void UseSkill(Vector2 _mousePos)
        {
            Vector2 center = currRect.Size.ToVector2();
            center.X /= 2;
            center.Y /= 2;
            float rotAngle = (float)Math.Atan2(_mousePos.Y - viewportPos.Y - center.Y, _mousePos.X - viewportPos.X - center.X);
            NewSkill(center, rotAngle);
        }
    }
}
