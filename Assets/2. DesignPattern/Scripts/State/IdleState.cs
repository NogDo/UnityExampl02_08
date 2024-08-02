using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    public class IdleState : CBaseState
    {
        public override void Enter()
        {

        }

        public override void Exit()
        {
            Debug.Log("대기 상태 종료");
        }

        public override void Update()
        {
            player.text.text = $"{GetType().Name} : {player.fStateStay:n0}";
            player.fStateStay += Time.deltaTime;
        }
    }
}
