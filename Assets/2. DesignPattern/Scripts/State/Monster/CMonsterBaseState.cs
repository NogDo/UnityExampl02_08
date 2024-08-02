using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.HomeworkMonsterState
{
    public class CMonsterBaseState
    {
        #region protected º¯¼ö
        protected CMonsterController monsterController;
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
