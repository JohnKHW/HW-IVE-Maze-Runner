using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment.UI
{
    class UIText : GUI
    {
        SpriteFont font;
        public string text;

        public UIText(SpriteFont _font, string _text, Vector2? _pos, Color? _color) : base(_pos, _color)
        {
            font = _font;
            text = _text;
            rect.Width = (int)_font.MeasureString(_text).X;
            rect.Height = (int)_font.MeasureString(_text).Y;
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }
        public void Update()
        {
            rect.Location = pos.ToPoint();
        }
        public void SetHorCenter(float _vpPos)
        {
            pos.X = (_vpPos - font.MeasureString(text).X / 2);
        }
        public void SetVerCenter(float _vpPos)
        {
            pos.Y = (_vpPos - font.MeasureString(text).Y / 2);
        }
        public override void Draw(SpriteBatch spriteBatch, float depth = 0, float scale = 1, float rotation = 0)
        {
            spriteBatch.DrawString(font, text, pos, color);
        }
        public override void Dispose()
        {

        }
    }
}
