using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public abstract class CSkillBase
    {
        #region protected ����
        protected float fDamage;
        #endregion

        public abstract float GetDamage();
    }
}
