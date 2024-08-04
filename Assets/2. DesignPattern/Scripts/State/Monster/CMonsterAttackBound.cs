using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CMonsterAttackBound : MonoBehaviour
    {
        #region private º¯¼ö
        CMonsterStateMachine stateMachine;
        CMonsterController monsterController;
        #endregion

        void Awake()
        {
            stateMachine = GetComponentInParent<CMonsterStateMachine>();
            monsterController = GetComponentInParent<CMonsterController>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (monsterController.LookTarget == null)
                {
                    monsterController.LookTarget = other.transform;
                }

                stateMachine.ChangeState(stateMachine.Attack);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                stateMachine.ChangeState(stateMachine.Chase);
            }
        }
    }
}
