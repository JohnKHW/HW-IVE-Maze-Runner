using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Assignment.Manager
{
    class SceneManager:DrawableGameComponent
    {
        List<Scene> scenes = new List<Scene>();
        Scene currScene;
        public ContentManager contentManager;
        public KeyboardState lastKs;
        public MouseState lastMs;
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        MouseState ms;
        KeyboardState ks;
        Song bgSong;
        string bgSongPath = "Sound//theme";

        public SceneManager(Game g) : base(g)
        {
            contentManager = Game.Content;
        }

        public override void Initialize()
        {
            graphicsDevice = Game.GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currScene = new MainMenu(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            currScene.LoadContent();
            LoadBgSong();
            base.LoadContent();
        }
        private void LoadBgSong()
        {
            bgSong = contentManager.Load<Song>(bgSongPath);
            MediaPlayer.Volume = 1f;
            MediaPlayer.Play(bgSong);
            MediaPlayer.IsRepeating = true;
        }
        public void SwitchSong(string _path)
        {
            if (!_path.Equals(bgSongPath))
            {
                bgSongPath = _path;
                MediaPlayer.Pause();
                bgSong = contentManager.Load<Song>(_path);
                MediaPlayer.Play(bgSong);
            }
        }
        public override void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();
            ks = Keyboard.GetState();
            currScene.Update(gameTime, ms, ks);
            if (ks.IsKeyDown(Keys.Escape) && ks != lastKs && currScene.GetType() != typeof(MainMenu))
                SwitchScene(new MainMenu(this));
            lastKs = ks;
            lastMs = ms;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            currScene.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void SwitchScene(Scene _scene)
        {
            currScene.Dispose();
            //lastScene = currScene;
            currScene = _scene;
            currScene.LoadContent();
        }

    }
}
