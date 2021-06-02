using Assignment.Scenes;
using Assignment.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Characters
{
    class Mage : Character
    {
        public Mage(ContentManager _contentManager, Level _level, Vector2 _startPos) : base(_contentManager, _level)
        {
            totlaWalkFrame = 1;
            totalAtkFrame = 5;
            speed = 5;
            characterType = CharacterType.Mage;
            skillType = SkillType.Fireball;
            mapPos = _startPos;
            hp = 1000;
            mp = 500;
        }
        public override void LoadContent()
        {
            LoadWalkContent();
            LoadAtkContent();
            base.LoadContent();
        }

        private void LoadWalkContent()
        {
            walkUp = LoadTextureContent("Characters//Mage//anim_walk//up");
            walkUpLeft = LoadTextureContent("Characters//Mage//anim_walk//upLeft");
            walkLeft = LoadTextureContent("Characters//Mage//anim_walk//left");
            walkDownLeft = LoadTextureContent("Characters//Mage//anim_walk//downLeft");
            walkDown = LoadTextureContent("Characters//Mage//anim_walk//down");
            rectWalk.Height = walkDown.Height;
            rectWalk.Width = walkDown.Width / totlaWalkFrame;
            rectIdle = rectWalk;
            currTexture = walkDown;
        }
        private void LoadAtkContent()
        {
            atkUp = LoadTextureContent("Characters//Mage//anim_attack//up");
            atkUpLeft = LoadTextureContent("Characters//Mage//anim_attack//upLeft");
            atkLeft = LoadTextureContent("Characters//Mage//anim_attack//left");
            atkDownLeft = LoadTextureContent("Characters//Mage//anim_attack//downLeft");
            atkDown = LoadTextureContent("Characters//Mage//anim_attack//down");
            rectAtk.Height = atkDown.Height;
            rectAtk.Width = atkDown.Width / totalAtkFrame;
        }

    }
}
