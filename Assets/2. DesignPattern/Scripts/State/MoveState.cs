using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    public class MoveState : CBaseState
    {
        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            //Debug.Log("이동 상태 종료");
        }

        public override void Update()
        {
            player.textState.text = $"{GetType().Name} : {player.fMoveDistance:n1}";
            player.fMoveDistance += Time.deltaTime;
        }
    }
}
