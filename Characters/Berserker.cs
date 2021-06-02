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
    class Berserker : Character
    {
        public Berserker(ContentManager _contentManager, Level _level, Vector2 _startPos) : base(_contentManager, _level)
        {
            totlaWalkFrame = 8;
            totalAtkFrame = 4;
            speed = 5;
            characterType = CharacterType.Knight;
            skillType = SkillType.Axe;
            mapPos = _startPos;
            hp = 400;
            mp = 150;
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
            rectWalk.Height = walkDown.Height;
            rectWalk.Width = walkDown.Width / totlaWalkFrame;
            rectIdle = rectWalk;
            currTexture = walkDown;
        }
        private void LoadAtkContent()
        {
            atkUp = LoadTextureContent("Characters//Berserker//anim_attack//up");
            atkUpLeft = LoadTextureContent("Characters//Berserker//anim_attack//upLeft");
            atkLeft = LoadTextureContent("Characters//Berserker//anim_attack//left");
            atkDownLeft = LoadTextureContent("Characters//Berserker//anim_attack//downLeft");
            atkDown = LoadTextureContent("Characters//Berserker//anim_attack//down");
            rectAtk.Height = atkDown.Height;
            rectAtk.Width = atkDown.Width / totalAtkFrame;
        }

    }
}
