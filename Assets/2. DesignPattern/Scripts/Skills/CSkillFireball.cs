using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CSkillFireball : CSkillBase
    {
        public CSkillFireball()
        {
            fDamage = 10.0f;
        }

        public override float GetDamage()
        {
            return fDamage;
        }
    }
}
