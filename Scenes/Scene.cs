using Assignment.Manager;
using Assignment.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Scenes
{
    abstract class Scene : IDisposable
    {
        public SceneManager sceneManager;
        public GraphicsDevice GraphicsDevice;

        protected List<UIButton> uIButtons = new List<UIButton>();
        protected List<UIText> uITexts = new List<UIText>();
        protected List<UITexture> uITextures = new List<UITexture>();

        public Scene(ref SceneManager _sceneManager)
        {
            sceneManager = _sceneManager;
            GraphicsDevice = _sceneManager.GraphicsDevice;
        }
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime, MouseState _ms, KeyboardState _ks);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public void Dispose()
        {
            foreach (UIButton uIButton in uIButtons) uIButton.Dispose();
            foreach (UIText uIText in uITexts) uIText.Dispose();
            foreach (UITexture uITexture in uITextures) uITexture.Dispose();
        }

        #region LoadContent
        protected void LoadButtonContent(
            string _path,
            string _text,
            Vector2 _pos,
            Color? _defaultColor = null,
            Color? _hoverColor = null,
            Color? _selectedColor = null,
            float _hoverTime = 1)
        {
            SpriteFont _font = sceneManager.contentManager.Load<SpriteFont>(_path);
            _pos.X -= (int)_font.MeasureString(_text).X / 2; 
            _pos.Y -= (int)_font.MeasureString(_text).Y / 2;
            UIText text = new UIText
                (
                    _font,
                    _text,
                    _pos,
                    _defaultColor
                );
            UIButton button = new UIButton
                (
                    text,
                    _defaultColor,
                    _hoverColor,
                    _selectedColor,
                    _hoverTime
                );
            uIButtons.Add(button);
        }
        protected void LoadTextContent(string _path, string _text, Vector2 _pos, Color? _color = null)
        {
            UIText text = new UIText
                (
                    sceneManager.contentManager.Load<SpriteFont>(_path),
                    _text,
                    _pos,
                    _color
                );
            uITexts.Add(text);
        }
        protected void LoadTextureContent(string _path, Vector2 _pos, Color? _color = null)
        {
            UITexture texture = new UITexture
                (
                    sceneManager.contentManager.Load<Texture2D>(_path),
                    _pos,
                    _color
                );
            uITextures.Add(texture);
        }
        #endregion

    }
}
