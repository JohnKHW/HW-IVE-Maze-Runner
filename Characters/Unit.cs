using Assignment.Manager;
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
    enum CharacterState
    {
        Idle,
        Walk,
        Attack,
        Die
    }
    enum Direction
    {
        Up,
        UpLeft,
        Left,
        DownLeft,
        Down,
        DownRight,
        Right,
        UpRight
    }
    enum UnitType
    {
        Player,
        Enemy
    }
    abstract class Unit:IDisposable
    {
        public UnitType unitType;
        double elaspedTime, timeStep = 80;

        public float speed;
        public int mp = 100, hp = 2000, damage;
        private float currHp, currMp;
        protected float regenHpRate = 0.015f, regenMpRate = 0.05f;
        public float CurrHp
        {
            get { return currHp; }
            set
            {
                if (value > hp) currHp = hp;
                else currHp = value;
            }
        }
        public float CurrMp
        {
            get { return currMp; }
            set
            {
                if (value > mp) currMp = mp;
                else currMp = value;
            }
        }

        protected int totlaWalkFrame, totalAtkFrame, currFrame;
        protected Rectangle rectWalk, rectAtk, rectIdle;
        public Rectangle currRect;//Cannot be used in draw

        public Vector2 viewportPos { get { return CameraManager.ViewportPos(this.mapPos, ref level); } }
        public Vector2 centerPos, lastMapPos, mapPos, currSpeed;

        protected Texture2D walkUp, walkUpLeft, walkLeft, walkDownLeft, walkDown;
        protected Texture2D atkUp, atkUpLeft, atkLeft, atkDownLeft, atkDown;
        public Texture2D currTexture;
        protected Color color = Color.White;

        public Color[] currData;
        Color[] walkUpData, walkUpLeftData, walkLeftData, walkDownLeftData, walkDownData;
        Color[] atkUpData, atkUpLeftData, atkLeftData, atkDownLeftData, atkDownData;

        public Collider collider;
        public Direction currDirection;
        public CharacterState currCharState, lastCharState;
        protected ContentManager contentManager;
        protected Level level;
        protected SkillType skillType;

        public Unit(ContentManager _contentManager, Level _level)
        {
            contentManager = _contentManager;
            collider = new Collider(this);
            level = _level;
        }
        public virtual void LoadContent()
        {
            CurrHp = hp;
            CurrMp = mp;
            LoadContentData();
        }
        public virtual void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            elaspedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (currCharState != lastCharState) currFrame = 0;
            if (elaspedTime >= timeStep && currCharState != CharacterState.Idle)
            {
                elaspedTime = 0;
                if (currCharState == CharacterState.Walk)
                {
                    currFrame = (currFrame + 1) % totlaWalkFrame;
                    rectWalk.X = currFrame * rectWalk.Width;
                }
                else if (currCharState == CharacterState.Attack)
                {
                    currFrame = (currFrame + 1) % totalAtkFrame;
                    rectAtk.X = currFrame * rectAtk.Width;
                }
            }

            if (currSpeed.X == 0 && currSpeed.Y > 0)
                currDirection = Direction.Down;
            else if (currSpeed.X == 0 && currSpeed.Y < 0)
                currDirection = Direction.Up;
            else if (currSpeed.X > 0 && currSpeed.Y == 0)
                currDirection = Direction.Right;
            else if (currSpeed.X < 0 && currSpeed.Y == 0)
                currDirection = Direction.Left;
            else if(currSpeed.X > 0 && currSpeed.Y > 0)
                currDirection = Direction.DownRight;
            else if (currSpeed.X > 0 && currSpeed.Y < 0)
                currDirection = Direction.UpRight;
            else if (currSpeed.X < 0 && currSpeed.Y > 0)
                currDirection = Direction.DownLeft;
            else if (currSpeed.X < 0 && currSpeed.Y < 0)
                currDirection = Direction.UpLeft;

            centerPos = new Vector2(viewportPos.X + currRect.Size.X / 2, viewportPos.Y + currRect.Size.Y / 2);
            UpdateRegen(gameTime);
            collider.Update();
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (currDirection)
            {
                case Direction.Up:
                    if (currCharState == CharacterState.Attack)
                    {
                        currData = atkUpData;
                        currTexture = atkUp;
                    }
                    else if (currCharState == CharacterState.Walk)
                    {
                        currData = walkUpData;
                        currTexture = walkUp;
                    }
                    else
                    {
                        currData = walkUpData;
                        currTexture = walkUp;
                    }
                    break;
                case Direction.UpLeft:
                    if (currCharState == CharacterState.Attack)
                    {
                        currData = atkUpData;
                        currTexture = atkUpLeft;
                    }
                    else if (currCharState == CharacterState.Walk)
                    {
                        currData = walkUpLeftData;
                        currTexture = walkUpLeft;
                    }
                    else
                    {
                        currData = walkUpLeftData;
                        currTexture = walkUpLeft;
                    }
                    break;
                case Direction.Left:
                    if (currCharState == CharacterState.Attack)
                    {
                        currData = atkLeftData;
                        currTexture = atkLeft;
                    }
                    else if (currCharState == CharacterState.Walk)
                    {
                        currData = walkLeftData;
                        currTexture = walkLeft;
                    }
                    else
                    {
                        currData = walkLeftData;
                        currTexture = walkLeft;
                    }
                    break;
                case Direction.DownLeft:
                    if (currCharState == CharacterState.Attack)
                    {
                        currData = atkDownLeftData;
                        currTexture = atkDownLeft;
                    }
                    else if (currCharState == CharacterState.Walk)
                    {
                        currData = walkDownLeftData;
                        currTexture = walkDownLeft;
                    }
                    else
                    {
                        currData = walkDownLeftData;
                        currTexture = walkDownLeft;
                    }
                    break;
                case Direction.Down:
                    if (currCharState == CharacterState.Attack)
                    {
                        currData = atkDownData;
                        currTexture = atkDown;
                    }
                    else if (currCharState == CharacterState.Walk)
                    {
                        currData = walkDownData;
                        currTexture = walkDown;
                    }
                    else
                    {
                        currData = walkDownData;
                        currTexture = walkDown;
                    }
                    break;
                case Direction.DownRight:
                    if (currCharState == CharacterState.Attack)
                    {
                        currData = atkDownLeftData;
                        currTexture = atkDownLeft;
                    }
                    else if (currCharState == CharacterState.Walk)
                    {
                        currData = walkDownLeftData;
                        currTexture = walkDownLeft;
                    }
                    else
                    {
                        currData = walkDownLeftData;
                        currTexture = walkDownLeft;
                    }
                    break;
                case Direction.Right:
                    if (currCharState == CharacterState.Attack)
                    {
                        currData = atkLeftData;
                        currTexture = atkLeft;
                    }
                    else if (currCharState == CharacterState.Walk)
                    {
                        currData = walkLeftData;
                        currTexture = walkLeft;
                    }
                    else
                    {
                        currData = walkLeftData;
                        currTexture = walkLeft;
                    }
                    break;
                case Direction.UpRight:
                    if (currCharState == CharacterState.Attack)
                    {
                        currData = atkUpLeftData;
                        currTexture = atkUpLeft;
                    }
                    else if (currCharState == CharacterState.Walk)
                    {
                        currData = walkUpLeftData;
                        currTexture = walkUpLeft;
                    }
                    else
                    {
                        currData = walkUpLeftData;
                        currTexture = walkUpLeft;
                    }
                    break;
            }
            if(currDirection == Direction.DownRight ||
                currDirection == Direction.Right ||
                currDirection == Direction.UpRight)
            {
                spriteBatch.Draw(currTexture, viewportPos, currRect, color, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0.5f);
            }
            else
            {
                spriteBatch.Draw(currTexture, viewportPos, currRect, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
            }
        }

        private void UpdateRegen(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CurrHp += hp * regenHpRate * deltaTime;
            CurrMp += mp * regenMpRate * deltaTime;
        }
        protected void ReachEdge()
        {
            if (mapPos.X + currRect.Width / 2 < 0)
            {
                mapPos.X = -currRect.Width / 2;
            }
            else if (mapPos.X + currRect.Width / 2 > level.worldSize.X)
            {
                mapPos.X = level.worldSize.X - currRect.Width / 2;
            }
            if (mapPos.Y + currRect.Height / 2 < 0)
            {
                mapPos.Y = -currRect.Height / 2;
            }
            else if (mapPos.Y + currRect.Height > level.worldSize.Y)
            {
                mapPos.Y = level.worldSize.Y - currRect.Height;
            }
        }
        #region LoadContent
        private void LoadContentData()
        {
            #region Useless
            /*
            Texture2D[] textures = new Texture2D[10];
            Color[][] data = new Color[10][];
            textures[0] = walkUp;
            textures[1] = walkUpLeft;
            textures[2] = walkLeft;
            textures[3] = walkDownLeft;
            textures[4] = walkDown;
            textures[5] = atkUp;
            textures[6] = atkUpLeft;
            textures[7] = atkLeft;
            textures[8] = atkDownLeft;
            textures[9] = atkDown;
            for(int i =0; i< textures.Length; i++)
            {
                data[i] = new Color[textures[i].Width * textures[i].Height];
                textures[i].GetData<Color>(data[i]);
            }
            walkUpData = data[0];
            walkUpLeftData = data[1];
            walkLeftData = data[2];
            walkDownLeftData = data[3];
            walkDownData = data[4];
            atkUpData = data[5];
            atkUpLeftData = data[6];
            atkLeftData = data[7];
            atkDownLeftData = data[8];
            atkDownData = data[9];
            */
            #endregion

            walkUpData = new Color[walkUp.Width * walkUp.Height];
            walkUp.GetData<Color>(walkUpData);

            walkUpLeftData = new Color[walkUpLeft.Width * walkUpLeft.Height];
            walkUpLeft.GetData<Color>(walkUpLeftData);

            walkLeftData = new Color[walkLeft.Width * walkLeft.Height];
            walkLeft.GetData<Color>(walkLeftData);

            walkDownLeftData = new Color[walkDownLeft.Width * walkDownLeft.Height];
            walkDownLeft.GetData<Color>(walkDownLeftData);

            walkDownData = new Color[walkDown.Width * walkDown.Height];
            walkDown.GetData<Color>(walkDownData);

            atkUpData = new Color[atkUp.Width * atkUp.Height];
            atkUp.GetData<Color>(atkUpData);

            atkUpLeftData = new Color[atkUpLeft.Width * atkUpLeft.Height];
            atkUpLeft.GetData<Color>(atkUpLeftData);

            atkLeftData = new Color[atkLeft.Width * atkLeft.Height];
            atkLeft.GetData<Color>(atkLeftData);

            atkDownLeftData = new Color[atkDownLeft.Width * atkDownLeft.Height];
            atkDownLeft.GetData<Color>(atkDownLeftData);

            atkDownData = new Color[atkDown.Width * atkDown.Height];
            atkDown.GetData<Color>(atkDownData);
            currData = walkUpData;
        }
        public Texture2D LoadTextureContent(string _path)
        {
            return contentManager.Load<Texture2D>(_path);
        }
        protected void NewSkill(Vector2 _center, float _rotAngle)
        {

            switch (skillType)
            {
                case SkillType.Fireball:
                    if (CurrMp >= Fireball.UseMP)
                    {
                        CurrMp -= Fireball.UseMP;
                        level.skills.Add(new Fireball(contentManager, mapPos + _center, _rotAngle, unitType));
                    }
                    break;
                case SkillType.Axe:
                    if (CurrMp >= Fireball.UseMP)
                    {
                        CurrMp -= Fireball.UseMP;
                        level.skills.Add(new Axe(contentManager, mapPos + _center, _rotAngle, unitType));
                    }
                    break;
                case SkillType.Laser:
                    if (CurrMp >= Laser.UseMP)
                    {
                        CurrMp -= Laser.UseMP;
                        level.skills.Add(new Laser(contentManager, mapPos + _center, _rotAngle, unitType));
                    }
                    break;
                case SkillType.Sword:
                    if (CurrMp >= Sword.UseMP)
                    {
                        CurrMp -= Sword.UseMP;
                        level.skills.Add(new Sword(contentManager, mapPos + _center, _rotAngle, unitType));
                    }
                    break;
            }
        }
        #endregion
        public void Dispose()
        {

        }
    }
}
