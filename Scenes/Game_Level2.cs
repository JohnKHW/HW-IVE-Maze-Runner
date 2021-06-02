using Assignment.Characters;
using Assignment.Manager;
using Assignment.Skills;
using Assignment.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Scenes
{
    class Game_Level2 : Level
    {
        double peroidElapsedTime, elapsedTime, totalTime;
        Texture2D t;
        GamePeriod[] gamePeriods = new GamePeriod[5];

        SoundEffect alert, win;
        string enemyText = "Total Enemies : ", alertText = "(Level Up)";
        string fontPath = "Font//gameFont",
            alertPath = "Font//alertFont", 
            alertSoundPath = "Sound//levelUp", 
            winSoundPath = "Sound//win";
        bool isAlertText;
        public Game_Level2(ref SceneManager _sceneManager, CharacterType _type) : base(ref _sceneManager, _type)
        {
            mapPath = "Levels//Level1.txt";
            tileDensity = (int)(31);
            //endPos = new Rectangle(36 * tileDensity, 0, 3 * tileDensity, 2 * tileDensity);
            t = new Texture2D(sceneManager.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
        }
        #region LoadContent
        public override void LoadContent()
        {
            base.LoadContent();
            SetGamePeriod();
            LoadAlert();
        }
        private void LoadAlert()
        {
            LoadTextContent(fontPath, enemyText, new Vector2(10, 10), Color.White);
            LoadTextContent(alertPath, alertText,
                new Vector2(sceneManager.GraphicsDevice.Viewport.Width / 2 - 200,
                sceneManager.GraphicsDevice.Viewport.Height / 2 - 30), Color.White);
            alert = sceneManager.contentManager.Load<SoundEffect>(alertSoundPath);
            win = sceneManager.contentManager.Load<SoundEffect>(winSoundPath);
        }
        private void SetGamePeriod()
        {
            for (int i = 0; i < gamePeriods.Length; i++)
            {
                gamePeriods[i] = new GamePeriod(gamePeriods.Length * (i + 1) / 2, gamePeriods.Length * (i + 1));
                totalEnemies += gamePeriods[i].numOfEnemy;
            }
            gamePeriods[0].isStart = true;
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            if (!isGameOver || isWin)
            {
                base.Update(gameTime, _ms, _ks);
                if (killedEnemies == totalEnemies)
                {
                    isWin = true;
                    win.Play();
                }
                totalTime += gameTime.ElapsedGameTime.TotalSeconds;
                UpdateGamePeriod(gameTime);
                uITexts[1].text = enemyText + killedEnemies + "/" + totalEnemies;
            }
        }
        private void UpdateGamePeriod(GameTime gameTime)
        {
            peroidElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < gamePeriods.Length; i++)
            {
                if (gamePeriods[i].isStart)
                {
                    if (i + 1 < gamePeriods.Length &&
                        !gamePeriods[i + 1].isStart &&
                        peroidElapsedTime >= gamePeriods[i].periodTime)
                    {
                        peroidElapsedTime = 0;
                        gamePeriods[i + 1].isStart = true;
                    }
                    if (!gamePeriods[i].isSpawn)
                    {
                        if (i != 0)
                        {
                            isAlertText = true;
                            alert.Play();
                        }
                        Random random = new Random();
                        for (int j = 0; j < gamePeriods[i].numOfEnemy; j++)
                        {
                            EnemyType itemType = (EnemyType)random.Next(Enum.GetValues(typeof(EnemyType)).Length);
                            float x = (float)random.NextDouble() * worldSize.X;
                            float y = (float)random.NextDouble() * worldSize.Y;
                            switch (itemType)
                            {
                                case EnemyType.DarkKnight:
                                    units.Add(new DarkKnight(sceneManager.contentManager, this, new Vector2(x, y)));
                                    break;
                                case EnemyType.Lunatic:
                                    units.Add(new Lunatic(sceneManager.contentManager, this, new Vector2(x, y)));
                                    break;
                                case EnemyType.Witch:
                                    units.Add(new Witch(sceneManager.contentManager, this, new Vector2(x, y)));
                                    break;
                                case EnemyType.Killer:
                                    units.Add(new Killer(sceneManager.contentManager, this, new Vector2(x, y)));
                                    break;
                            }
                            units.Last<Unit>().LoadContent();
                        }
                        gamePeriods[i].isSpawn = true;
                    }
                }
            }
        }
        #endregion
        #region Draw
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.YellowGreen);
            base.Draw(gameTime, spriteBatch);
            gameUI.Draw(spriteBatch);
            DrawText(gameTime, spriteBatch);
        }
        private void DrawText(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!isGameOver && !isWin)
            {
                uITexts[0].Draw(spriteBatch);
                uITexts[1].Draw(spriteBatch);
                if (isAlertText)
                {
                    elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
                    uITexts[2].Draw(spriteBatch);
                    if (elapsedTime >= 2)
                    {
                        elapsedTime = 0;
                        isAlertText = false;
                    }
                }
            }
            else if (isGameOver)
            {
                GameOver(spriteBatch);
            }
            else
            {
                Win(spriteBatch);
            }
            
        }
        #endregion
        private void Win(SpriteBatch spriteBatch)
        {
            uITexts[2].text =
                "       You Win"+
                "\nPress Ese To Back";
            uITexts[2].SetHorCenter(sceneManager.GraphicsDevice.Viewport.Width / 2);
            uITexts[2].SetVerCenter(sceneManager.GraphicsDevice.Viewport.Height / 2);
            uITexts[2].Draw(spriteBatch);
        }
        protected override void GameOver(SpriteBatch spriteBatch)
        {
            uITexts[2].text = "Survial Time: " + (int)Math.Round(totalTime) + "s" +
                "\n Killed Enemies: " + killedEnemies+
                "\nPress Ese To Back";
            uITexts[2].SetHorCenter(sceneManager.GraphicsDevice.Viewport.Width / 2);
            uITexts[2].SetVerCenter(sceneManager.GraphicsDevice.Viewport.Height / 2);
            uITexts[2].Draw(spriteBatch);
        }
    }
}
