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
    class UITexture : GUI
    {
        public Texture2D texture;
        public Color[] data;

        public UITexture(Texture2D _texture, Vector2 _pos, Color? _color) : base(_pos, _color?? Color.White)
        {
            texture = _texture;
            rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
            data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);
        }
        public override void Draw(SpriteBatch spriteBatch, float depth = 0, float scale = 1, float rotation = 0)
        {
            spriteBatch.Draw(texture, pos, rect, color, rotation, Vector2.Zero, scale, SpriteEffects.None, depth);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 _center, float rotation = 0, float depth = 0, float scale = 1)
        {
            spriteBatch.Draw(texture, pos, rect, color, rotation, _center, scale, SpriteEffects.None, depth);
        }
        public override void Dispose()
        {

        }
    }
}
