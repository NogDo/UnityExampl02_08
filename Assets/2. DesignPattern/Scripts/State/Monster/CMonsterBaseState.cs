using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace MyProject.Homework0804
{
    public class CMonsterBaseState
    {
        #region protected º¯¼ö
        protected CMonsterController monsterController;
        protected StringBuilder stringBuilder = new StringBuilder();
        #endregion

        public void Init(CMonsterController monsterController)
        {
            this.monsterController = monsterController;
        }

        public virtual void OnStateEnter()
        {
            Debug.Log($"{GetType().Name} is Entered");
        }

        public virtual void OnStateExit()
        {
            Debug.Log($"{GetType().Name} is Exited");
        }

        public virtual void OnStateUpdate()
        {
            Debug.Log($"{GetType().Name} is Updated");
        }
    }
}
