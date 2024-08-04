using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CSkillArcaneSphere : CSkillBase
    {
        public CSkillArcaneSphere()
        {
            fDamage = 5.0f;
        }

        public override float GetDamage()
        {
            return fDamage;
        }
    }
}
