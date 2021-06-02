using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Scenes
{
    enum TileType
    {
        Grass,
        Rock,
        Water,
        Lava,
        Wood,
        Dirt,
        Snow,
        Other
    }

    class Tile
    {
        public TileType tileType;
        public Rectangle colRect;
        Rectangle bound;
        public Texture2D texture;
        bool isAnimation;
        int tileSize;
        int currFrame;
        double elapsdTime, timeStep = 120;

        public Tile(Texture2D _texture, TileType _tileType, bool _isAnimation = true, int _tileSize = 3)
        {
            texture = _texture;
            tileType = _tileType;
            isAnimation = _isAnimation;
            tileSize = _tileSize;
            bound.Width = texture.Width / tileSize;
            bound.Height = texture.Height;
        }
        public void Update(GameTime gametime)
        {
            elapsdTime += gametime.ElapsedGameTime.TotalMilliseconds;
            if (isAnimation && elapsdTime >= timeStep)
            {
                currFrame = (currFrame + 1) % tileSize;
                bound.X = bound.Width * currFrame;
                elapsdTime = 0;
            }
            colRect = bound;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, pos, bound, Color.White, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, colRect.Location.ToVector2(), bound, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
        }
    }
}
