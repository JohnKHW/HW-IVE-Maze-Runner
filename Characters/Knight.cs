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
    class Knight: Character
    {
        public Knight(ContentManager _contentManager, Level _level, Vector2 _startPos) : base(_contentManager, _level)
        {
            totlaWalkFrame = 8;
            totalAtkFrame = 4;
            speed = 5;
            characterType = CharacterType.Knight;
            skillType = SkillType.Laser;
            mapPos = _startPos;
            hp = 3000;
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
            walkUp = LoadTextureContent("Characters//Knight//anim_walk//up");
            walkUpLeft = LoadTextureContent("Characters//Knight//anim_walk//upLeft");
            walkLeft= LoadTextureContent("Characters//Knight//anim_walk//left");
            walkDownLeft = LoadTextureContent("Characters//Knight//anim_walk//downLeft");
            walkDown = LoadTextureContent("Characters//Knight//anim_walk//down");
            rectWalk.Height = walkDown.Height;
            rectWalk.Width = walkDown.Width / totlaWalkFrame;
            rectIdle = rectWalk;
            currTexture = walkDown;
        }
        private void LoadAtkContent()
        {
            atkUp = LoadTextureContent("Characters//Knight//anim_attack//up");
            atkUpLeft = LoadTextureContent("Characters//Knight//anim_attack//upLeft");
            atkLeft = LoadTextureContent("Characters//Knight//anim_attack//left");
            atkDownLeft = LoadTextureContent("Characters//Knight//anim_attack//downLeft");
            atkDown = LoadTextureContent("Characters//Knight//anim_attack//down");
            rectAtk.Height = atkDown.Height;
            rectAtk.Width = atkDown.Width / totalAtkFrame;
        }
        
    }
}
