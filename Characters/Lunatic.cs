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
    class Lunatic : Enemy
    {
        public Lunatic(ContentManager _contentManager, Level _level, Vector2 _startPos) : base(_contentManager, _level)
        {
            totlaWalkFrame = 8;
            totalAtkFrame = 4;
            detectDistant = 700;
            stopDistant = 100;
            atkRange = 250;
            atkSpeed = 500;
            speed = 4;
            skillType = SkillType.Axe;
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
            walkUp = LoadTextureContent("Characters//Berserker//anim_walk//up");
            walkUpLeft = LoadTextureContent("Characters//Berserker//anim_walk//upLeft");
            walkLeft = LoadTextureContent("Characters//Berserker//anim_walk//left");
            walkDownLeft = LoadTextureContent("Characters//Berserker//anim_walk//downLeft");
            walkDown = LoadTextureContent("Characters//Berserker//anim_walk//down");
            rectWalk.Height = walkUp.Height;
            rectWalk.Width = walkUp.Width / totlaWalkFrame;
            rectIdle = rectWalk;
        }
        private void LoadAtkContent()
        {
            atkUp = LoadTextureContent("Characters//Berserker//anim_attack//up");
            atkUpLeft = LoadTextureContent("Characters//Berserker//anim_attack//upLeft");
            atkLeft = LoadTextureContent("Characters//Berserker//anim_attack//left");
            atkDownLeft = LoadTextureContent("Characters//Berserker//anim_attack//downLeft");
            atkDown = LoadTextureContent("Characters//Berserker//anim_attack//down");
            rectAtk.Height = atkUp.Height;
            rectAtk.Width = atkUp.Width / totalAtkFrame;
        }
    }
}
