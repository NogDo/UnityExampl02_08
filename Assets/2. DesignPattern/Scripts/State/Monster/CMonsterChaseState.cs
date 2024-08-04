using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CMonsterChaseState : CMonsterBaseState
    {
        public override void OnStateEnter()
        {
            monsterController.ResetStateStayTime();
            monsterController.ChangeAnimation(EMonsterState.CHASE);
        }

        public override void OnStateExit()
        {

        }

        public override void OnStateUpdate()
        {
            monsterController.StateStayTime += Time.deltaTime;

            stringBuilder.Clear();
            stringBuilder.Append("Chase State : ").Append(monsterController.StateStayTime.ToString("n0"));

            monsterController.ChangeStateText(stringBuilder.ToString());
            monsterController.LookPlayer();
        }
    }
}
