using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill
{
    public class CSkillBehaviour : MonoBehaviour
    {
        #region internal º¯¼ö
        internal CSkillContext context;
        #endregion

        public virtual void Apply()
        {
            Debug.Log($"{GetType().Name} skill applied");
        }

        public virtual void Use()
        {
            Debug.Log($"{GetType().Name} skill used");
        }

        public virtual void Remove()
        {
            Debug.Log($"{GetType().Name} skill removed");
        }
    }
}