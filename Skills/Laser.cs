using Assignment.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Skills
{
    class Laser : Skill
    {
        public Laser(ContentManager _contentManager, Vector2 _pos, float _rotAngle, UnitType _unitType) : base(_contentManager, _pos, _rotAngle, _unitType)
        {
            skillTexturePath = "Images//laser";
            totalSkillFrame = 11;
            LoadContent();
        }
    }
}
