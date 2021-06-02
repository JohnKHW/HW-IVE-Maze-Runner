using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Characters;
using Assignment.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment.UI
{
    class MP_Potion : Item
    {
        private float magnitude = 0.1f;
        public MP_Potion(SceneManager _sceneManager, Vector2 _pos, Color? _color) :
            base(_sceneManager, _pos, _color, "Images//mp_potion", ItemType.MP_Potion)
        {

        }
        public override void ItemEffect(Character _player)
        {
            _player.CurrMp += _player.mp * magnitude;
        }
    }
}
