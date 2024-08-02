using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    // �÷��̾�� ���� ������Ʈ�� �����鼭 �÷��̾��� ���¸� �������ִ� ������Ʈ
    [RequireComponent(typeof(CPlayer))]
    public class CStateMachine : MonoBehaviour
    {
        #region public ����
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
        /// ���� ���¸� �����Ѵ�.
        /// </summary>
        /// <param name="state">������ State</param>
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
