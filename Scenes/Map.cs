using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Scenes
{
    class Map
    {
        //string filePath;
        public Tile[,] tiles;
        public int width, height;
        public List<Wall> walls = new List<Wall>();
        public List<Wall> wallsX = new List<Wall>();
        public List<Wall> wallsY = new List<Wall>();
        string grassPath = "Tiles//tile_grass";
        string rockPath = "Tiles//tile_rock";
        string waterPath = "Tiles//tile_water";
        string lavaPath = "Tiles//tile_magma";
        string snowPath = "Tiles//tile_snow";
        string woodPath = "Tiles//tile_Log";
        string dirtPath = "Tiles//tile_dirt";
        string whitePath = "Images//White";
        int density;
        ContentManager contentManager;

        public Map(string _filePath, ContentManager _contentManager, int _density)
        {
            contentManager = _contentManager;
            density = _density;
            LoadFile(_filePath);
        }
        public void Update(GameTime gametime, Vector2 _worldOrigin, int _density)
        {
            foreach (Tile tile in tiles) tile.Update(gametime);
            SetComponentSize(_worldOrigin, _density);
        }

        public void SetComponentSize(Vector2 _worldOrigin, int _density)
        {
            Vector2 tilePos = _worldOrigin;
            for (int h = 0; h < tiles.GetLength(0); h++)
            {
                tilePos.X = _worldOrigin.X;
                for (int w = 0; w < tiles.GetLength(1); w++)
                {
                    tiles[h, w].colRect.Location = tilePos.ToPoint();
                    tilePos.X += _density;
                }
                tilePos.Y += _density;
            }
            foreach (Wall wall in walls)
            {
                wall.FirstTile = tiles[(int)wall.firstMapPos.Y, (int)wall.firstMapPos.X];
                wall.LastTile = tiles[(int)wall.lastMapPos.Y, (int)wall.lastMapPos.X];
            }
        }

        #region LoadContent
        private void LoadFile(string _filePath)
        {
            List<string> lines = new List<string>();
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("Content//" + _filePath);
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
                height = lines.Count;
            }
            catch (Exception) { }

            if (width != 0 && height != 0) LoadTile(lines);

        }
        private void LoadTile(List<string> _lines)
        {
            tiles = new Tile[height, width];
            int lasTile = 0;

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    tiles[h, w] = LoadTileType(_lines[h][w]);
                    if (_lines[lasTile][w].Equals(_lines[h][w]) && _lines[h][w].Equals('r'))
                    {
                        if (wallsX.Count != 0 && wallsX.Last<Wall>().lastMapPos.X == w - 1)
                        {
                            wallsX.Last<Wall>().lastMapPos = new Vector2(w, h);
                            wallsX.Last<Wall>().LastTile = tiles[h, w];
                        }
                        else
                        {
                            if (wallsX.Count != 0 && wallsX.Last<Wall>().DropWall)
                                wallsX.Remove(wallsX.Last<Wall>());
                            wallsX.Add(new Wall(new Vector2(w, h), contentManager.Load<Texture2D>(whitePath), density));
                            wallsX.Last().FirstTile = tiles[h, w];
                        }
                    }
                    lasTile = h;
                }
            }
            lasTile = 0;
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    if (_lines[h][lasTile].Equals(_lines[h][w]) && _lines[h][w].Equals('r'))
                    {
                        if (wallsY.Count != 0 && wallsY.Last<Wall>().lastMapPos.Y == h - 1)
                        {
                            wallsY.Last<Wall>().lastMapPos = new Vector2(w, h);

                            wallsX.Last<Wall>().LastTile = tiles[h, w];
                        }
                        else
                        {
                            if (wallsY.Count != 0 && wallsY.Last<Wall>().DropWall)
                                wallsY.Remove(wallsY.Last<Wall>());
                            wallsY.Add(new Wall(new Vector2(w, h), contentManager.Load<Texture2D>(whitePath), density));
                            wallsX.Last<Wall>().FirstTile = tiles[h, w];
                        }
                    }
                    lasTile = w;
                }
            }

            for (int i = 0; i< wallsX.Count; i++)
            {
                walls.Add(wallsX[i]);
            }
            for (int i = 0; i < wallsY.Count; i++)
            {
                walls.Add(wallsY[i]);
            }
        }
        private Tile LoadTileType(char currType)
        {
            switch (currType)
            {
                case 'g':
                    return new Tile(contentManager.Load<Texture2D>(grassPath), TileType.Grass, false);
                case 'r':
                    return new Tile(contentManager.Load<Texture2D>(rockPath), TileType.Rock, false);
                case 'w':
                    return new Tile(contentManager.Load<Texture2D>(waterPath), TileType.Water);
                case 'l':
                    return new Tile(contentManager.Load<Texture2D>(woodPath), TileType.Wood, false);
                case 'm':
                    return new Tile(contentManager.Load<Texture2D>(lavaPath), TileType.Lava);
                case 's':
                    return new Tile(contentManager.Load<Texture2D>(snowPath), TileType.Snow);
                case 'd':
                    return new Tile(contentManager.Load<Texture2D>(dirtPath), TileType.Dirt);
                default:
                    return new Tile(contentManager.Load<Texture2D>(grassPath), TileType.Other);
            }
        }
        #endregion

        private void AddCollider()
        {

        }
    }
}
