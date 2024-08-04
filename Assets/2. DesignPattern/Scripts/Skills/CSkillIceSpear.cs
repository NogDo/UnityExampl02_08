using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CSkillIceSpear : CSkillBase
    {
        public CSkillIceSpear()
        {
            fDamage = 15.0f;
        }

        public override float GetDamage()
        {
            return fDamage;
        }
    }
}
