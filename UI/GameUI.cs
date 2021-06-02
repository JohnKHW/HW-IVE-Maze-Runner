using Assignment.Characters;
using Assignment.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Assignment.UI
{
    enum ItemUse
    {
        Item1,
        Item2,
        Item3,
        Item4,
        Item5
    }
    class GameUI
    {
        UITexture hpBar, mpBar, containterTexture;
        SceneManager sceneManager;

        HP_Potion hpPotion;
        MP_Potion mpPotion;
        MPHP_Potion mpHpPotion;

        //Store in GameUI Containter
        Dictionary<ItemType, int> itemTypes = new Dictionary<ItemType, int>();
        //Text of Items Number in Containters
        UIText[] itemsNum = new UIText[5];
        //GameUI Containters Default Position
        Vector2[] itemsPos = new Vector2[5];
        //Type of Items
        Item[] items = new Item[5];
        string containterPath = "Images//GamePanel";
        string hpBarPath = "Images//HP";
        string mpBarPath = "Images//MP";
        string stringPath = "Font//itemNumFont";
        public GameUI(ref SceneManager _sceneManager)
        {
            sceneManager = _sceneManager;
        }
        public void LoadContent()
        {
            LoadItemContainterContent();
            LoadItemContent();
            LoadBarContent();
        }
        public void Update(ref Character _player)
        {
            hpBar.rect.Height = (int)Math.Round(hpBar.texture.Height* (_player.CurrHp / _player.hp));
            hpBar.pos.Y = sceneManager.GraphicsDevice.Viewport.Height - hpBar.rect.Height;

            mpBar.rect.Height = (int)Math.Round(mpBar.texture.Height * (_player.CurrMp / _player.mp));
            mpBar.pos.Y = sceneManager.GraphicsDevice.Viewport.Height - mpBar.rect.Height;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawItem(spriteBatch);
            DrawGamePanel(spriteBatch);
            DrawItemNum(spriteBatch);
        }
        #region LoadContent
        private void LoadItemContainterContent()
        {
            containterTexture = new UITexture
                (
                    sceneManager.contentManager.Load<Texture2D>(containterPath),
                    Vector2.Zero,
                    Color.White
                );
            containterTexture.pos.X = sceneManager.GraphicsDevice.Viewport.Width / 2 - containterTexture.rect.Width / 2;
            containterTexture.pos.Y = sceneManager.GraphicsDevice.Viewport.Height - containterTexture.rect.Height;
            for (int i = 0; i < itemsPos.Length; i++)
            {
                itemsPos[i].Y = containterTexture.rect.Height / 12 * 7;
                itemsPos[i].X = containterTexture.rect.Width / 27 + containterTexture.rect.Width / 54 * 5 * (i * 2 + 1);
                itemsPos[i] += containterTexture.pos;
                itemsNum[i] = new UIText
                    (
                        sceneManager.contentManager.Load<SpriteFont>(stringPath),
                        "99",
                        new Vector2(containterTexture.rect.Width / 27 * 5 * (i + 1), -35 + containterTexture.rect.Height) +
                        containterTexture.pos,
                        Color.White
                    );
            }
        }
        private void LoadItemContent()
        {
            hpPotion = new HP_Potion(sceneManager, Vector2.Zero, Color.White);
            hpPotion.LoadContent();
            mpPotion = new MP_Potion(sceneManager, Vector2.Zero, Color.White);
            mpPotion.LoadContent();
            mpHpPotion = new MPHP_Potion(sceneManager, Vector2.Zero, Color.White);
            mpHpPotion.LoadContent();
        }
        private void LoadBarContent()
        {
            hpBar = new UITexture
                (
                    sceneManager.contentManager.Load<Texture2D>(hpBarPath),
                    Vector2.Zero,
                    Color.White
                );
            hpBar.pos.X = containterTexture.pos.X;
            hpBar.pos.Y = sceneManager.GraphicsDevice.Viewport.Height - hpBar.rect.Height;
            mpBar = new UITexture
                (
                    sceneManager.contentManager.Load<Texture2D>(mpBarPath),
                    Vector2.Zero,
                    Color.White
                );
            mpBar.pos.X = containterTexture.pos.X + containterTexture.rect.Width - mpBar.rect.Width;
            mpBar.pos.Y = sceneManager.GraphicsDevice.Viewport.Height - mpBar.rect.Height;
        }
        #endregion
        #region Draw
        private void DrawGamePanel(SpriteBatch spriteBatch)
        {
            hpBar.Draw(spriteBatch);
            mpBar.Draw(spriteBatch);
            containterTexture.Draw(spriteBatch);
        }
        private void DrawItem(SpriteBatch spriteBatch)
        {
            for(int i= 0;i< items.Length; i++)
            {
                if (items[i] != null)
                {
                    items[i].SetPos(itemsPos[i]);
                    items[i].Draw(spriteBatch);
                }
            }
        }
        private void DrawItemNum(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    itemsNum[i].text = itemTypes[items[i].itemType].ToString();
                    itemsNum[i].Draw(spriteBatch);
                }
            }
        }
        #endregion
        public void UseItem(ItemUse _itemUse, ref Character _player)
        {
            int itemPos = (int)_itemUse;
            if (items[itemPos] != null)
            {
                ItemType key = items[(int)_itemUse].itemType;
                items[itemPos].ItemEffect(_player);
                if (itemTypes.ContainsKey(key))
                {
                    itemTypes[key] -= 1;
                    if (itemTypes[key] == 0)
                    {
                        itemTypes.Remove(key);
                        items[itemPos].Dispose();
                        items[itemPos] = null;
                    }
                }
            }
        }
        public void AddItem(ItemType _item)
        {
            if (!itemTypes.ContainsKey(_item))
            {
                itemTypes.Add(_item, 1);
            }
            else
            {
                itemTypes[_item] += 1;
            }
            for (int i = 0; i < items.Length; i++)
            {
                bool isNull = true;
                bool isSameType = false;
                for (int j = items.Length - 1; j > i; j--)
                {
                    if (items[j] != null)
                    {
                        if (items[j].itemType == _item) isSameType = true;
                        isNull = false;
                    }
                }
                //Check Current Position and After Position is Null
                if (items[i] == null && isNull)
                {
                    AddItemType(_item, i);
                    break;
                }
                else if (items[i] == null && !isNull && !isSameType)
                {
                    AddItemType(_item, i);
                    break;
                }
                else if (items[i] == null && isNull)
                {
                    AddItemType(_item, i);
                    break;
                }
                //Check Current Position is not Null and Type = Current Position Type(Normal Case)
                else if (items[i] != null && _item == items[i].itemType) break;
            }
        }
        private void AddItemType(ItemType _item, int _pos)
        {
            switch (_item)
            {
                case ItemType.HP_Potion:
                    items[_pos] = hpPotion;
                    break;
                case ItemType.MP_Potion:
                    items[_pos] = mpPotion;
                    break;
                case ItemType.MPHP_Potion:
                    items[_pos] = mpHpPotion;
                    break;
            }
        }
    }
}
