using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CMonsterAttackState : CMonsterBaseState
    {
        public override void OnStateEnter()
        {
            monsterController.ResetStateStayTime();
            monsterController.ChangeAnimation(EMonsterState.ATTACK);
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {
            monsterController.StateStayTime += Time.deltaTime;

            stringBuilder.Clear();
            stringBuilder.Append("Attack State : ").Append(monsterController.StateStayTime.ToString("n0"));

            monsterController.ChangeStateText(stringBuilder.ToString());
            monsterController.LookPlayer();
        }
    }
}
