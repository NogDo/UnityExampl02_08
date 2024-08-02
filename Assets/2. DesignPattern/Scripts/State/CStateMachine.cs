using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    // 플레이어와 같은 오브젝트에 있으면서 플레이어의 상태를 제어해주는 컴포넌트
    [RequireComponent(typeof(CPlayer))]
    public class CStateMachine : MonoBehaviour
    {
        #region public 변수
        public MoveState moveState;
        public IdleState idleState;
        public CPlayer player;
        public IState currentState;
        #endregion

        //void Awake()
        //{
        //    player = GetComponent<CPlayer>();
        //}

        void Reset()
        {
            player = GetComponent<CPlayer>();
        }

        void Start()
        {
            moveState = new MoveState();
            idleState = new IdleState();

            moveState.Initialize(player);
            idleState.Initialize(player);

            currentState = idleState;
            idleState.Enter();
        }

        void Update()
        {
            currentState.Update();
        }

        /// <summary>
        /// 현재 상태를 변경한다.
        /// </summary>
        /// <param name="state">변경할 State</param>
        public void Transition(IState state)
        {
            if (currentState == state)
            {
                return;
            }

            currentState.Exit();
            currentState = state;
            currentState.Enter();
        }
    }
}
