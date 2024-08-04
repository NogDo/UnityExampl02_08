using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CMonsterStateMachine : MonoBehaviour
    {
        #region private ����
        CMonsterIdleState idle;
        CMonsterChaseState chase;
        CMonsterAttackState attack;
        CMonsterController monsterController;

        CMonsterBaseState currentState;
        #endregion

        /// <summary>
        /// Monster�� Idle State
        /// </summary>
        public CMonsterIdleState Idle
        {
            get
            {
                return idle;
            }
        }

        /// <summary>
        /// Monster�� Chase State
        /// </summary>
        public CMonsterChaseState Chase
        {
            get
            {
                return chase;
            }
        }

        /// <summary>
        /// Monster�� Attack State
        /// </summary>
        public CMonsterAttackState Attack
        {
            get
            {
                return attack;
            }
        }


        void Awake()
        {
            idle = new CMonsterIdleState();
            chase = new CMonsterChaseState();
            attack = new CMonsterAttackState();
            monsterController = GetComponent<CMonsterController>();
        }

        void Start()
        {
            idle.Init(monsterController);
            chase.Init(monsterController);
            attack.Init(monsterController);

            currentState = idle;
            currentState.OnStateEnter();
        }

        void Update()
        {
            currentState.OnStateUpdate();
        }

        /// <summary>
        /// ���� State�� �����Ѵ�.
        /// </summary>
        /// <param name="state">����� State</param>
        public void ChangeState(CMonsterBaseState state)
        {
            if (currentState == state)
            {
                return;
            }

            currentState.OnStateExit();
            currentState = state;
            currentState.OnStateEnter();
        }
    }
}
