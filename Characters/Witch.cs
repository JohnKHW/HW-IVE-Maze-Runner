using Assignment.Scenes;
using Assignment.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Characters
{
    class Witch : Enemy
    {
        public Witch(ContentManager _contentManager, Level _level, Vector2 _startPos) : base(_contentManager, _level)
        {
            totlaWalkFrame = 1;
            totalAtkFrame = 5;
            detectDistant = 700;
            stopDistant = 100;
            atkRange = 250;
            atkSpeed = 500;
            speed = 4;
            skillType = SkillType.Fireball;
            mapPos = _startPos;
            lastMapPos = mapPos;
            color = Color.LightPink;
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
            rectWalk.Height = walkUp.Height;
            rectWalk.Width = walkUp.Width / totlaWalkFrame;
            rectIdle = rectWalk;
        }
        private void LoadAtkContent()
        {
            atkUp = LoadTextureContent("Characters//Mage//anim_attack//up");
            atkUpLeft = LoadTextureContent("Characters//Mage//anim_attack//upLeft");
            atkLeft = LoadTextureContent("Characters//Mage//anim_attack//left");
            atkDownLeft = LoadTextureContent("Characters//Mage//anim_attack//downLeft");
            atkDown = LoadTextureContent("Characters//Mage//anim_attack//down");
            rectAtk.Height = atkUp.Height;
            rectAtk.Width = atkUp.Width / totalAtkFrame;
        }
    }
}
