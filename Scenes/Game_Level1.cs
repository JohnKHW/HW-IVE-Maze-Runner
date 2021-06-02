using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Characters;
using Assignment.Manager;
using Assignment.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment.Scenes
{
    class Game_Level1 : Level
    {
        Random random = new Random();
        protected List<Item> items = new List<Item>();
        private SoundEffect collection;
        string itemText = "Token Items : ", alertText = "(Level Up)";
        string fontPath = "Font//gameFont",
            alertPath = "Font//alertFont";
        int totalItem = 50;
        public Game_Level1(ref SceneManager _sceneManager, CharacterType _type) : base(ref _sceneManager, _type)
        {
            mapPath = "Levels//maze.txt";
            tileDensity = (int)(31);
            startPos = new Vector2(1270, 2440);
            //endPos = new Rectangle(36 * tileDensity, 0, 3 * tileDensity, 2 * tileDensity);
        }
        public override void LoadContent()
        {
            base.LoadContent();
            RandomItem();
            LoadTextContent(fontPath, itemText, new Vector2(10, 10), Color.White);
            LoadTextContent(alertPath, alertText,
                new Vector2(sceneManager.GraphicsDevice.Viewport.Width / 2 - 200,
                sceneManager.GraphicsDevice.Viewport.Height / 2 - 30), Color.White);
            collection = sceneManager.contentManager.Load<SoundEffect>("Sound//collection");
        }
        private void RandomItem()
        {
            for (int i = 0; i < totalItem; i++)
            {
                ItemType itemType = (ItemType)random.Next(Enum.GetValues(typeof(ItemType)).Length - 1);
                float x = (float)random.NextDouble() * worldSize.X;
                float y = (float)random.NextDouble() * worldSize.Y;
                bool isSame = false;
                do
                {
                    foreach (Wall wall in map.walls)
                    {
                        if (x + 46 > wall.rectangle.Left && x < wall.rectangle.Left ||
                            x < wall.rectangle.Right && x + 46 > wall.rectangle.Right ||
                            y + 55 > wall.rectangle.Top && y < wall.rectangle.Top ||
                            y < wall.rectangle.Bottom && y + 55 > wall.rectangle.Bottom)
                        {
                            isSame = true;
                            x = (float)random.NextDouble() * worldSize.X;
                            y = (float)random.NextDouble() * worldSize.Y;
                            break;
                        }
                        else
                            isSame = false;
                    }
                }
                while (isSame);
                switch (itemType)
                {
                    case ItemType.HP_Potion:
                        items.Add(new HP_Potion(sceneManager, new Vector2(x, y), Color.White));
                        break;
                    case ItemType.MP_Potion:
                        items.Add(new MP_Potion(sceneManager, new Vector2(x, y), Color.White));
                        break;
                    case ItemType.MPHP_Potion:
                        items.Add(new MPHP_Potion(sceneManager, new Vector2(x, y), Color.White));
                        break;
                }
                items.Last<Item>().LoadContent();
            }
        }
        public override void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            if (!isGameOver)
            {
                base.Update(gameTime, _ms, _ks);
                foreach (Item item in items)
                {
                    item.Update(gameTime, worldOrigin);
                    if (player.collider.CheckCollision(item.uITexture.pos, item.uITexture.rect,
                        item.uITexture.rect.Size, ref item.uITexture.data, null))
                    {
                        collection.Play();
                        gameUI.AddItem(item.itemType);
                        item.Dispose();
                        items.Remove(item);
                        break;
                    }
                }
                uITexts[1].text = itemText + (totalItem - items.Count) + "/" + totalItem;
                if (player.mapPos.Y + player.currTexture.Height / 2 <= 0)
                {
                    Level level2 = new Game_Level2(ref sceneManager, type);
                    level2.gameUI = gameUI;
                    sceneManager.SwitchScene(level2);
                }
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.YellowGreen);
            base.Draw(gameTime, spriteBatch);
            foreach (Item item in items)
            {
                if (item.uITexture.pos.X >= -item.uITexture.rect.Width &&
                    item.uITexture.pos.Y >= -item.uITexture.rect.Height &&
                    item.uITexture.pos.X - item.uITexture.rect.Width <= GraphicsDevice.Viewport.Width &&
                    item.uITexture.pos.Y - item.uITexture.rect.Height <= GraphicsDevice.Viewport.Height)
                    item.Draw(spriteBatch);
                //item.DrawBoard(spriteBatch);
            }
            gameUI.Draw(spriteBatch);
            if (!isGameOver)
            {
                uITexts[0].Draw(spriteBatch);
                uITexts[1].Draw(spriteBatch);
            }
            else GameOver(spriteBatch);
        }
        protected override void GameOver(SpriteBatch spriteBatch)
        {
            uITexts[2].text =
                "   You Are Lost" +
               "\nPress Ese To Back";
            uITexts[2].SetHorCenter(sceneManager.GraphicsDevice.Viewport.Width / 2);
            uITexts[2].SetVerCenter(sceneManager.GraphicsDevice.Viewport.Height / 2);
            uITexts[2].Draw(spriteBatch);
        }
        
    }
}
