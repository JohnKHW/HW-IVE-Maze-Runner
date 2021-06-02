using Assignment.Characters;
using Assignment.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.UI
{
    enum ItemType
    {
        HP_Potion,
        MP_Potion,
        MPHP_Potion,
        None
    }
    abstract class Item : IDisposable
    {
        public UITexture uITexture;
        public ItemType itemType;
        string path;
        public Vector2 pos;

        Texture2D t;
        SceneManager sceneManager;
        Color color;

        public Item(SceneManager _sceneManager, Vector2? _pos, Color? _color, string _path, ItemType _itemType)
        {
            sceneManager = _sceneManager;
            pos = _pos ?? Vector2.Zero;
            color = _color ?? Color.White;
            path = _path;
            itemType = _itemType;
            t = new Texture2D(sceneManager.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
        }
        public void LoadContent()
        {
            uITexture = new UITexture
                (
                    sceneManager.contentManager.Load<Texture2D>(path),
                    Vector2.Zero,
                    color
                );
            //uITexture.pos = pos - uITexture.rect.Size.ToVector2() / 2;
        }
        public void SetPos(Vector2 _pos)
        {
            uITexture.pos = _pos - uITexture.rect.Size.ToVector2() / 2;
        }
        public virtual void Update(GameTime gameTime, Vector2 _pos)
        {
            uITexture.pos = pos + _pos;
        }
        public void DrawBoard(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(t, uITexture.pos, uITexture.rect, Color.Red * 0.5f);
        }
        public virtual void Draw(SpriteBatch spriteBatch, float depth = 0, float scale = 1, float rotation = 0)
        {
            uITexture.Draw(spriteBatch, depth, scale, rotation);
        }
        public abstract void ItemEffect(Character _player);
        public void Dispose()
        {
            uITexture.Dispose();
        }
    }
}
