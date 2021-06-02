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
    class MPHP_Potion : Item
    {
        private float magnitudeHp = 0.12f, magnitudeMp = 0.07f;
        public MPHP_Potion(SceneManager _sceneManager, Vector2 _pos, Color? _color) :
            base(_sceneManager, _pos, _color, "Images//mphp_potion", ItemType.MPHP_Potion)
        {

        }
        public override void ItemEffect(Character _player)
        {
            _player.CurrMp += _player.mp * magnitudeMp;
            _player.CurrHp += _player.hp * magnitudeHp;
        }
    }
}
