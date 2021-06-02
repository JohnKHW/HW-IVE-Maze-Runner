using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Characters;
using Assignment.Manager;
using Assignment.Skills;
using Assignment.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment.Scenes
{
    
    abstract class Level : Scene
    {
        public Map map;
        protected string mapPath;
        public int tileDensity;
        public Vector2 worldSize;
        public Vector2 worldOrigin, camOrigin, lastWorldOrign;
        protected Vector2 startPos;

        protected CharacterType type;
        public Character player;
        protected List<Unit> units = new List<Unit>();
        public List<Skill> skills = new List<Skill>();
        public GameUI gameUI;

        public Vector2 lastMousePos;
        SoundEffect explosion, lost;
        protected double remainTime = 120;
        protected string timerString = "00 : 00";
        string timerFontPath = "Font//timerFont";
        protected int totalEnemies, killedEnemies;
        protected bool isGameOver, isWin;

        public Level(ref SceneManager _sm, CharacterType _type) : base( ref _sm)
        {
            gameUI = new GameUI(ref sceneManager);
            type = _type;
        }
        #region LoadContent
        public override void LoadContent()
        {
            LoadMapContent();
            LoadPlayerContent();
            gameUI.LoadContent();
            LoadTextContent(timerFontPath, timerString, new Vector2(sceneManager.GraphicsDevice.Viewport.Width / 2 - 80, 20));
            LoadSoundContent();
            sceneManager.SwitchSong("Sound//bgm");
        }
        private void LoadSoundContent()
        {
            explosion = sceneManager.contentManager.Load<SoundEffect>("Sound//explosion");
            lost = sceneManager.contentManager.Load<SoundEffect>("Sound//lost");
        }
        private void LoadMapContent()
        {
            map = new Map(mapPath, sceneManager.contentManager, tileDensity);
            worldSize.X = map.width * tileDensity;
            worldSize.Y = map.height * tileDensity;
            map.SetComponentSize(worldOrigin, tileDensity);
        }
        private void LoadPlayerContent()
        {
            switch (type)
            {
                case CharacterType.Berserker:
                    player = new Berserker(sceneManager.contentManager, this, startPos);
                    break;
                case CharacterType.Knight:
                    player = new Knight(sceneManager.contentManager, this, startPos);
                    break;
                case CharacterType.Mage:
                    player = new Mage(sceneManager.contentManager, this, startPos);
                    break;
                case CharacterType.Soldier:
                    player = new Soldier(sceneManager.contentManager, this, startPos);
                    break;
            }
            player.LoadContent();
            units.Add(player);
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            if (player.CurrHp > 0 && remainTime > 0) UpdateUseItem(_ms, _ks);
            else
            {
                isGameOver = true;
                lost.Play();
            }
            map.Update(gameTime, worldOrigin, tileDensity);
            UpdateUnit(gameTime, _ms, _ks);
            UpdateSkill(gameTime);
            gameUI.Update(ref player);
            UpdateTimer(gameTime);
        }
        private void UpdateTimer(GameTime gametime)
        {
            remainTime -= gametime.ElapsedGameTime.TotalSeconds;
            int min = (int)Math.Floor(remainTime / 60);
            int sec = (int)Math.Round(remainTime % 60);
            string _timer;
            if (sec >= 60) min++;
            if (min == 0)
            {
                _timer = "00 : ";
            }
            else if (min > 0 && min < 10)
            {
                _timer = "0" + min + " : ";
            }
            else
            {
                _timer = min + " : ";
            }
            if (sec == 0 || sec >= 60)
            {
                _timer += "00";
            }
            else if (sec > 0 && sec < 10)
            {
                _timer += "0" + sec;
            }
            else
            {
                _timer += sec;
            }
            uITexts[0].text = _timer;
        }
        private void UpdateUnit(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            foreach (Unit unit in units)
            {
                unit.Update(gameTime, _ms, _ks);
                //Check Unit and Wall Collision
                foreach (Wall wall in map.walls)
                {
                    if (!_ks.IsKeyDown(Keys.L) && unit == player)
                    {
                        CollisionManager.CollisionBoundary(unit, wall.rectangle);
                    }
                }
                if (unit == player && (!isGameOver || !isWin))
                {
                    foreach (Tile tile in map.tiles)
                    {
                        switch (tile.tileType)
                        {
                            case TileType.Lava:
                                if (tile.colRect.Intersects(unit.collider.colMove))
                                {
                                    unit.CurrHp -= (float)(50 * gameTime.ElapsedGameTime.TotalSeconds);
                                }
                                break;
                            case TileType.Water:
                                if (tile.colRect.Intersects(unit.collider.colMove))
                                {
                                    unit.CurrMp += (float)(5 * gameTime.ElapsedGameTime.TotalSeconds);
                                }
                                break;
                            case TileType.Snow:
                                if (tile.colRect.Intersects(unit.collider.colMove))
                                {
                                    unit.currSpeed =Vector2.Lerp(unit.currSpeed, unit.currSpeed * 5, 0.5f);
                                }
                                break;
                        }
                    }
                }
                //Check Unit and Unit Collision
                foreach (Unit unit2 in units)
                {
                    if (unit != unit2)
                    {
                        CollisionManager.CollisionBoundary(unit, unit2.collider.colMove);
                    }
                }
                //Check Unit and Skill Collision
                foreach (Skill skill in skills)
                {
                    if (
                        skill.unitType != unit.unitType &&
                        !skill.isCollided &&
                        unit.collider.CheckCollision(skill.viewportPos, skill.skillRect,
                        skill.skillTexture.Bounds.Size, ref skill.data,
                        skill.skillRect.Center.ToVector2(), skill.rotAngle)
                        )
                    {
                        unit.CurrHp -= (skill.Damage + unit.damage);
                        skill.isCollided = true;
                        explosion.Play(0.3f, 1, 0);
                        break;
                    }
                }
                if (unit.CurrHp <= 0 && unit != player)
                {
                    units.Remove(unit);
                    killedEnemies++;
                    if (unit == player) player.currCharState = CharacterState.Die;
                    break;
                }
            }
        }
        private void UpdateUseItem(MouseState _ms, KeyboardState _ks)
        {
            CameraManager.Drag(_ms, sceneManager.lastMs, this, GraphicsDevice);
            if (_ms.LeftButton != ButtonState.Pressed)
                CameraManager.Movement(player, this, GraphicsDevice);
            if (_ks.IsKeyDown(Keys.D1) && _ks != sceneManager.lastKs)
            {
                gameUI.UseItem(ItemUse.Item1, ref player);
            }
            if (_ks.IsKeyDown(Keys.D2) && _ks != sceneManager.lastKs)
            {
                gameUI.UseItem(ItemUse.Item2, ref player);
            }
            if (_ks.IsKeyDown(Keys.D3) && _ks != sceneManager.lastKs)
            {
                gameUI.UseItem(ItemUse.Item3, ref player);
            }
            if (_ks.IsKeyDown(Keys.D4) && _ks != sceneManager.lastKs)
            {
                gameUI.UseItem(ItemUse.Item4, ref player);
            }
            if (_ks.IsKeyDown(Keys.D5) && _ks != sceneManager.lastKs)
            {
                gameUI.UseItem(ItemUse.Item5, ref player);
            }
        }
        private void UpdateSkill(GameTime gameTime)
        {
            //Update Skill State
            foreach (Skill skill in skills)
            {
                skill.Update(gameTime, worldOrigin);
                if (skill.isDestory)
                {
                    skills.Remove(skill);
                    break;
                }
            }
        }
        #endregion
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float tileY = worldOrigin.Y;
            
            for (int h = 0; h < map.tiles.GetLength(0); h++)
            {
                float tileX = worldOrigin.X;
                for (int w = 0; w < map.tiles.GetLength(1); w++)
                {
                    //Reduce Drawing Time, Speed up the Process
                    if (tileX > -tileDensity &&
                        tileY > -tileDensity &&
                        tileX < GraphicsDevice.Viewport.Width + tileDensity &&
                        tileY < GraphicsDevice.Viewport.Height + tileDensity)
                        map.tiles[h, w].Draw(spriteBatch);
                    tileX += tileDensity;
                }
                tileY += tileDensity;
            }
            //foreach (Wall wall in map.walls) wall.Draw(spriteBatch, GraphicsDevice);
            foreach (Skill skill in skills) skill.Draw(spriteBatch);
            foreach (Unit unit in units)
            {
                if (unit.viewportPos.X >= -unit.currRect.Width &&
                    unit.viewportPos.Y >= -unit.currRect.Height &&
                    unit.viewportPos.X - unit.currRect.Width <= GraphicsDevice.Viewport.Width &&
                    unit.viewportPos.Y - unit.currRect.Height <= GraphicsDevice.Viewport.Height)
                    unit.Draw(gameTime, spriteBatch);
                //unit.collider.DrawRectBord(spriteBatch, GraphicsDevice, 4, Color.Blue);
                //unit.collider.DrawRect(spriteBatch, GraphicsDevice, Color.Gray);
            }
        }
        protected abstract void GameOver(SpriteBatch spriteBatch);
    }
}
