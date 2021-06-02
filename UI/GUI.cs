using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment.UI
{
    abstract class GUI:IDisposable
    {
        public Rectangle rect;
        public Vector2 pos;
        public Color color;

        public GUI() { }

        public GUI(Vector2? _pos, Color? _color)
        {
            pos = _pos ?? Vector2.Zero;
            color = _color ?? Color.White;
        }
            
        public GUI(Rectangle _rect, Vector2? _pos, Color? _color)
        {
            rect = _rect;
            pos = _pos ?? Vector2.Zero;
            color = _color ?? Color.White;
        }

        public abstract void Draw(SpriteBatch spriteBatch, float depth = 0, float scale = 1, float rotation = 0);
        public abstract void Dispose();
    }
}
