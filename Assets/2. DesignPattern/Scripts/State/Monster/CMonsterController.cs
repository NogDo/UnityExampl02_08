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
        #region private 변수
        [SerializeField]
        TextMeshPro text;
        Animator animator;
        Transform tfLookTarget;

        float fStateStayTime = 0.0f;
        #endregion

        #region protected 변수
        protected float fAttack;
        protected float fHp;
        protected float fAttackSpeed;
        protected float fMoveSpeed;
        #endregion

        /// <summary>
        /// 몬스터가 바라볼 타겟 Transform
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
        /// 현재 State에 머무른 시간
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
        /// 몬스터에 부착된 State상태를 나타내는 Text를 변경한다
        /// </summary>
        /// <param name="state">현재 상태를 나타내는 문자열</param>
        public void ChangeStateText(string state)
        {
            text.text = state;
        }

        /// <summary>
        /// 현재 상태에 따른 애니메이션을 재생한다.
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
        /// State에 머문 시간을 0초로 초기화한다.
        /// </summary>
        public void ResetStateStayTime()
        {
            fStateStayTime = 0.0f;
        }

        /// <summary>
        /// 플레이어를 바라본다.
        /// </summary>
        public void LookPlayer()
        {
            Vector3 dir = tfLookTarget.position - transform.position;

            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        /// <summary>
        /// 플레이어가 죽었을 때 애니메이션을 변경한다.
        /// </summary>
        public void ChangeAnimationOnPlayerDead()
        {
            animator?.SetInteger("Action", 3);
        }
    }
}
