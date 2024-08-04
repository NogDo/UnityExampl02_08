using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CSkillDecoratorDamageUpBuff : CSkillDecorator
    {
        public CSkillDecoratorDamageUpBuff(CSkillBase skill)
        {
            this.skill = skill;
        }

        public override float GetDamage()
        {
            return skill.GetDamage() + 5.0f;
        }
    }
}
