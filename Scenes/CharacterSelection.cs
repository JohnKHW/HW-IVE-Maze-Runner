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
    class CharacterSelection : Scene
    {
        string btnFontPath = "Font//btnFont";
        string titFontPath = "Font//titleFont";
        string sceneTitle = "character\nselection";
        string[] menuItems =
        {
            "Berserker",
            "Knite",
            "Mage",
            "Soldier"
        };
        Texture2D charDemo;
        Rectangle charDemoRect;
        Vector2 charDemoPos;
        int buttonSpace = 60;
        int currSelectBtn;
        SoundEffect selectSound;
        public CharacterSelection(SceneManager _sceneManager) : base(ref _sceneManager)
        {
        }
        public override void LoadContent()
        {
            LoadUIButton();
            LoadUIText();
            LoadTexture();
            selectSound = sceneManager.contentManager.Load<SoundEffect>("Sound//selectSound");
        }
        public override void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks)
        {
            foreach (UIButton button in uIButtons) button.Update(_ms, _ks);
            UpdateInput(_ms, _ks);
            if (uIButtons[0].isClicked)
            {
                sceneManager.SwitchScene(new Game_Level1(ref sceneManager, Characters.CharacterType.Berserker));
                currSelectBtn = 0;
                uIButtons[1].currState = UIButton.ButtonStates.None;
                uIButtons[2].currState = UIButton.ButtonStates.None;
                uIButtons[3].currState = UIButton.ButtonStates.None;
            }
            else if (uIButtons[1].isClicked)
            {
                sceneManager.SwitchScene(new Game_Level1(ref sceneManager, Characters.CharacterType.Knight));
                currSelectBtn = 1;
                uIButtons[0].currState = UIButton.ButtonStates.None;
                uIButtons[2].currState = UIButton.ButtonStates.None;
                uIButtons[3].currState = UIButton.ButtonStates.None;
            }
            else if (uIButtons[2].isClicked)
            {
                sceneManager.SwitchScene(new Game_Level1(ref sceneManager, Characters.CharacterType.Mage));
                currSelectBtn = 2;
                uIButtons[0].currState = UIButton.ButtonStates.None;
                uIButtons[1].currState = UIButton.ButtonStates.None;
                uIButtons[3].currState = UIButton.ButtonStates.None;
            }
            else if (uIButtons[3].isClicked)
            {
                sceneManager.SwitchScene(new Game_Level1(ref sceneManager, Characters.CharacterType.Soldier));
                currSelectBtn = 2;
                uIButtons[0].currState = UIButton.ButtonStates.None;
                uIButtons[1].currState = UIButton.ButtonStates.None;
                uIButtons[2].currState = UIButton.ButtonStates.None;
            }
        }
        private void UpdateInput(MouseState _ms, KeyboardState _ks)
        {
            //Menu item selection
            if ((_ks.IsKeyDown(Keys.Down) || _ks.IsKeyDown(Keys.S)) && _ks != sceneManager.lastKs)
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
                uIButtons[currSelectBtn].currState = UIButton.ButtonStates.Selected;
                selectSound.Play();
            }
            if (_ks.IsKeyDown(Keys.Enter) && _ks != sceneManager.lastKs)
            {
                uIButtons[currSelectBtn].isClicked = true;
            }
            charDemoRect.X = charDemoRect.Width * currSelectBtn;
            for (int i = 0; i < uIButtons.Count; i++)
            {
                uIButtons[(currSelectBtn + i) % uIButtons.Count].pos.Y =
                    GraphicsDevice.Viewport.Height / (menuItems.Length-1 ) + buttonSpace * i;
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (UIButton button in uIButtons) button.Draw(spriteBatch);
            foreach (UIText text in uITexts) text.Draw(spriteBatch);
            spriteBatch.Draw(charDemo, charDemoPos, charDemoRect, Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
        }
        private void LoadUIButton()
        {
            Vector2 pos = new Vector2(100, GraphicsDevice.Viewport.Height / (menuItems.Length -1));
            foreach (string menuItem in menuItems)
            {
                LoadButtonContent(btnFontPath, menuItem, pos, Color.White, Color.Wheat, Color.Yellow, 0.3f);
                pos.Y += buttonSpace;
            }
            uIButtons.First<UIButton>().currState = UIButton.ButtonStates.Selected;
        }
        private void LoadUIText()
        {
            Vector2 pos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 12);
            pos.X -= sceneManager.contentManager.Load<SpriteFont>(titFontPath).MeasureString(sceneTitle).X / 2;
            LoadTextContent(titFontPath, sceneTitle, pos, Color.White);
        }
        private void LoadTexture()
        {
            charDemo = sceneManager.contentManager.Load<Texture2D>("Images//char_select");
            charDemoRect.Width = charDemo.Width / menuItems.Length;
            charDemoRect.Height = charDemo.Height;
            charDemoPos = new Vector2(GraphicsDevice.Viewport.Width / 2 - charDemoRect.Width / 2,
                GraphicsDevice.Viewport.Height / (menuItems.Length - 1));
        }
    }
}
