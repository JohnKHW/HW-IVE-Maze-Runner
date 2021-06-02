using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Manager;
using Assignment.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment.Scenes
{
    class MainMenu : Scene
    {
        string btnFontPath = "Font//btnFont";
        string titFontPath = "Font//titleFont";
        string bgTexturePath = "Images//BackGround";
        SoundEffect selectSound;

        string gameTitle = "maze runner";
        string[] menuItems =
        {
            "Start Game",
            "Option",
            "Exit"
        };
        int buttonSpace = 60;
        int currSelectBtn;

        public MainMenu(SceneManager _sceneManager):base(ref _sceneManager)
        {
        }

        public override void LoadContent()
        {
            LoadUIButton();
            LoadUIText();
            LoadUITexture();
            sceneManager.SwitchSong("Sound//theme");
            selectSound = sceneManager.contentManager.Load<SoundEffect>("Sound//selectSound");
        }
        public override void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            foreach (UIButton button in uIButtons) button.Update(_ms, _ks);
            UpdateInput(_ms, _ks);
            if (uIButtons[0].isClicked)
            {
                //sceneManager.SwitchScene(new Game_Level2(sceneManager));
                sceneManager.SwitchScene(new CharacterSelection(sceneManager));
                currSelectBtn = 0;
                uIButtons[1].currState = UIButton.ButtonStates.None;
            }
            else if (uIButtons[1].isClicked)
            {
                currSelectBtn = 1;
                uIButtons[0].currState = UIButton.ButtonStates.None;
            }
            else if (uIButtons[2].isClicked)
            {
                currSelectBtn = 2;
                sceneManager.Game.Exit();
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (UITexture texture in uITextures) texture.Draw(spriteBatch);
            foreach (UIButton button in uIButtons) button.Draw(spriteBatch);
            foreach (UIText text in uITexts) text.Draw(spriteBatch);
        }
        private void UpdateInput(MouseState _ms, KeyboardState _ks)
        {
            //Menu item selection
            if ((_ks.IsKeyDown(Keys.Down)|| _ks.IsKeyDown(Keys.S)) && _ks != sceneManager.lastKs)
            {
                uIButtons[currSelectBtn].currState = UIButton.ButtonStates.None;
                currSelectBtn = (currSelectBtn + 1) % uIButtons.Count;
                uIButtons[currSelectBtn].currState = UIButton.ButtonStates.Selected;
                selectSound.Play();
            }
            else if ((_ks.IsKeyDown(Keys.Up) || _ks.IsKeyDown(Keys.W)) && _ks != sceneManager.lastKs)
            {
                uIButtons[currSelectBtn].currState = UIButton.ButtonStates.None;
                currSelectBtn = (currSelectBtn + uIButtons.Count - 1) % uIButtons.Count;
                selectSound.Play();
                uIButtons[currSelectBtn].currState = UIButton.ButtonStates.Selected;
            }
            if(_ks.IsKeyDown(Keys.Enter) && _ks != sceneManager.lastKs)
            {
                uIButtons[currSelectBtn].isClicked = true;
            }
        }
        private void LoadUIButton()
        {
            Vector2 pos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height/9*4);
            foreach(string menuItem in menuItems)
            {
                LoadButtonContent(btnFontPath, menuItem, pos, Color.White, Color.Wheat, Color.Yellow, 0.3f);
                pos.Y += buttonSpace;
            }
            uIButtons.First<UIButton>().currState = UIButton.ButtonStates.Selected;
        }
        private void LoadUIText()
        {
            Vector2 pos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 6);
            pos.X -= sceneManager.contentManager.Load<SpriteFont>(titFontPath).MeasureString(gameTitle).X / 2;
            LoadTextContent(titFontPath, gameTitle, pos, Color.White);
        }
        private void LoadUITexture()
        {
            LoadTextureContent(bgTexturePath, Vector2.Zero, null);
        }

    }
}
