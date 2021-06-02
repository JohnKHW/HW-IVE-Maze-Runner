using Assignment.Characters;
using Assignment.Manager;
using Assignment.Scenes;
using Assignment.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Skills
{
    enum SkillType
    {
        None,
        Fireball,
        Axe,
        Laser,
        Sword
    }
    abstract class Skill
    {
        ContentManager contentManager;
        public bool isDestory, isCollided;
        public UnitType unitType;

        public Vector2 mapPos, viewportPos, center;
        private Vector2 exploBasePos;
        protected Vector2 speed, startPos;
        public float rotAngle, travelDistance = 500;

        protected string skillTexturePath;
        protected int currSkillFrame, totalSkillFrame;
        private string exploTexturePath;
        private int currExploFrame, totalExploFrame;
        private double elapsedTime;

        public Texture2D skillTexture, exploTexture;
        public Color[] data;
        public Rectangle skillRect, exploRect;

        public int Damage=100;
        public static int UseMP = 10;
        public Skill() { }
        public Skill(ContentManager _contentManager, Vector2 _pos, float _rotAngle, UnitType _unitType)
        {
            contentManager = _contentManager;
            _contentManager.Load<SoundEffect>("Sound//useSkill").Play(0.1f, 1, 0);
            startPos = _pos;
            mapPos = _pos;
            rotAngle = _rotAngle;
            unitType = _unitType;
            speed.Y = (float)Math.Sin(rotAngle) * 7;
            speed.X = (float)Math.Cos(rotAngle) * 7;
            exploTexturePath = "Images//explosion";
            totalExploFrame = 5;
        }
        public void LoadContent()
        {
            LoadSkillTexture();
            LoadExplosionTexture();
        }
        public void Update(GameTime gameTime, Vector2 _worldOrigin)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            viewportPos = _worldOrigin + mapPos;
            if (!isCollided)
            {
                if (Vector2.Distance(mapPos, startPos) >= travelDistance) isDestory = true;
                mapPos += speed;
                if (elapsedTime >= 20)
                {
                    currSkillFrame = (currSkillFrame + 1) % totalSkillFrame;
                    skillRect.X = currSkillFrame * skillRect.Width;
                    elapsedTime = 0;
                }
            }
            else
            {
                if (elapsedTime >= 60)
                {
                    if (currExploFrame == totalExploFrame) isDestory = true;
                    else
                    {
                        currExploFrame = (currExploFrame + 1);
                        exploRect.X = currExploFrame * exploRect.Width;
                        elapsedTime = 0;
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isCollided)
                spriteBatch.Draw(skillTexture, viewportPos, skillRect, Color.White, rotAngle, center, 1, SpriteEffects.None, 0);
            else if (isCollided)
                spriteBatch.Draw(exploTexture, viewportPos - exploBasePos, exploRect, Color.White);
        }
        private void LoadSkillTexture()
        {
            skillTexture = contentManager.Load<Texture2D>(skillTexturePath);
            skillRect.Width = skillTexture.Width / totalSkillFrame;
            skillRect.Height = skillTexture.Height;
            center.X = skillRect.Size.X / 2;
            center.Y = skillRect.Size.Y / 2;
            data = new Color[skillTexture.Width * skillTexture.Height];
            skillTexture.GetData(data);
        }
        private void LoadExplosionTexture()
        {
            exploTexture = contentManager.Load<Texture2D>(exploTexturePath);
            exploRect.Width = exploTexture.Width / totalExploFrame;
            exploRect.Height = exploTexture.Height;
            exploBasePos.X = exploRect.Size.X / 2;
            exploBasePos.Y = exploRect.Bottom;
        }
    }
}
