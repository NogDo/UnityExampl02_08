using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

namespace MyProject.Homework0804
{
    public enum EMonsterState
    {
        IDLE,
        CHASE,
        ATTACK
    }

    public class CMonsterController : MonoBehaviour
    {
        #region private ����
        [SerializeField]
        TextMeshPro text;
        Animator animator;
        Transform tfLookTarget;

        float fStateStayTime = 0.0f;
        #endregion

        #region protected ����
        protected float fAttack;
        protected float fHp;
        protected float fAttackSpeed;
        protected float fMoveSpeed;
        #endregion

        /// <summary>
        /// ���Ͱ� �ٶ� Ÿ�� Transform
        /// </summary>
        public Transform LookTarget
        {
            get
            {
                return tfLookTarget;
            }

            set
            {
                tfLookTarget = value;
            }
        }

        /// <summary>
        /// ���� State�� �ӹ��� �ð�
        /// </summary>
        public float StateStayTime
        {
            get
            {
                return fStateStayTime;
            }

            set
            {
                fStateStayTime = value;
            }
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void OnEnable()
        {
            GameManager.Instance.onPlayerDead += ChangeAnimationOnPlayerDead;
        }

        void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onPlayerDead -= ChangeAnimationOnPlayerDead;
            }
        }

        /// <summary>
        /// ���Ϳ� ������ State���¸� ��Ÿ���� Text�� �����Ѵ�
        /// </summary>
        /// <param name="state">���� ���¸� ��Ÿ���� ���ڿ�</param>
        public void ChangeStateText(string state)
        {
            text.text = state;
        }

        /// <summary>
        /// ���� ���¿� ���� �ִϸ��̼��� ����Ѵ�.
        /// </summary>
        public void ChangeAnimation(EMonsterState state)
        {
            if (animator == null)
            {
                return;
            }

            switch (state)
            {
                case EMonsterState.IDLE:
                    animator.SetInteger("Action", 0);
                    break;

                case EMonsterState.CHASE:
                    animator.SetInteger("Action", 1);
                    break;

                case EMonsterState.ATTACK:
                    animator.SetInteger("Action", 2);
                    break;
            }
        }

        /// <summary>
        /// State�� �ӹ� �ð��� 0�ʷ� �ʱ�ȭ�Ѵ�.
        /// </summary>
        public void ResetStateStayTime()
        {
            fStateStayTime = 0.0f;
        }

        /// <summary>
        /// �÷��̾ �ٶ󺻴�.
        /// </summary>
        public void LookPlayer()
        {
            Vector3 dir = tfLookTarget.position - transform.position;

            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        /// <summary>
        /// �÷��̾ �׾��� �� �ִϸ��̼��� �����Ѵ�.
        /// </summary>
        public void ChangeAnimationOnPlayerDead()
        {
            animator?.SetInteger("Action", 3);
        }
    }
}
