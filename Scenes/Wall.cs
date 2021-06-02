using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment.Scenes
{
    class Wall
    {
        public Rectangle rectangle;
        Texture2D black;
        int density;
        private Tile firstTile, lastTile;
        public Tile FirstTile
        {
            set
            {
                firstTile = value;
                rectangle.Location = firstTile.colRect.Location;
            }
            get { return firstTile; }
        }
        public Tile LastTile
        {
            set
            {
                lastTile = value;
                rectangle.Width = lastTile.colRect.Width - firstTile.colRect.Width;
                rectangle.Size = lastTile.colRect.Location - firstTile.colRect.Location + new Point(density, density);

            }
            get { return lastTile; }
        }
        public bool DropWall
        {
            get
            {
                if (firstMapPos == lastMapPos) return true;
                return false;
            }
        }
        public Vector2 firstMapPos;
        public Vector2 lastMapPos;
        public Wall(Vector2 _firstMapPos, Texture2D _black, int _density)
        {
            density = _density;
            firstMapPos = _firstMapPos;
            lastMapPos = _firstMapPos;
            black = _black;
        }
        public void Draw(SpriteBatch _spriteBatch, int bw = 4, Color? _color = null)
        {

            //Texture2D t = new Texture2D(_graphicsDevice, 1, 1);
            //t.SetData(new[] { Color.White });
            Color color = _color ?? Color.Black;

            //_spriteBatch.Draw(black, rectangle, color);
            
            _spriteBatch.Draw(black,
                new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width + bw, bw), color);
            // Bottom
            _spriteBatch.Draw(black,
                new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width + bw, bw), color);
            // Left
            _spriteBatch.Draw(black,
                new Rectangle(rectangle.Left, rectangle.Top, bw, rectangle.Height + bw), color);
            // Right
            _spriteBatch.Draw(black,
                new Rectangle(rectangle.Right, rectangle.Top, bw, rectangle.Height + bw), color);
        }
    }
}
