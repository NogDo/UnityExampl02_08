using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CMonsterStateMachine : MonoBehaviour
    {
        #region private 변수
        CMonsterIdleState idle;
        CMonsterChaseState chase;
        CMonsterAttackState attack;
        CMonsterController monsterController;

        CMonsterBaseState currentState;
        #endregion

        /// <summary>
        /// Monster의 Idle State
        /// </summary>
        public CMonsterIdleState Idle
        {
            get
            {
                return idle;
            }
        }

        /// <summary>
        /// Monster의 Chase State
        /// </summary>
        public CMonsterChaseState Chase
        {
            get
            {
                return chase;
            }
        }

        /// <summary>
        /// Monster의 Attack State
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
        /// 현재 State를 변경한다.
        /// </summary>
        /// <param name="state">변경될 State</param>
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
