using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyProject.Skill;

namespace MyProject.Homework0804
{
    

    public class CWeapon : MonoBehaviour
    {
        public enum EWeaponType
        {
            NONE,
            FIRE,
            ICE,
            ARCANE
        }

        #region protected 변수
        [SerializeField]
        protected CWeaponAdapter weaponAdapter;
        protected EWeaponType weaponType;

        protected float fAttack;
        protected float fDurability;
        protected float fAttackSpeed;
        protected float fWeight;
        #endregion

        /// <summary>
        /// 무기의 타입
        /// </summary>
        public EWeaponType WeaponType
        {
            get
            {
                return weaponType;
            }
        }

        /// <summary>
        /// 무기 공격력
        /// </summary>
        public float Attack
        {
            get
            {
                return fAttack;
            }
        }

        /// <summary>
        /// 무기 내구성
        /// </summary>
        public float Durability
        {
            get
            {
                return fDurability;
            }
        }

        /// <summary>
        /// 무기 공격속도
        /// </summary>
        public float AttackSpeed
        {
            get
            {
                return fAttackSpeed;
            }
        }

        /// <summary>
        /// 무기 무게
        /// </summary>
        public float Weight
        {
            get
            {
                return fWeight;
            }
        }

        void OnEnable()
        {
            Init();
            StartCoroutine("WeaponToMonsterTimer");
        }

        void OnDisable()
        {
            StopCoroutine("WeaponToMonsterTimer");
        }

        /// <summary>
        /// 장비가 생성되었을 때 초기화
        /// </summary>
        public virtual void Init()
        {
            Debug.Log($"{GetType().Name} Initializeed");
        }

        /// <summary>
        /// 장비를 장착했을 때 실행될 메서드
        /// </summary>
        public virtual void Equip(TextMeshPro text, CSkillContext context)
        {
            Debug.Log($"{GetType().Name} Equiped");
        }

        /// <summary>
        /// 장비를 해제했을 때 실행될 메서드
        /// </summary>
        public virtual void TakeOff()
        {
            Debug.Log($"{GetType().Name} Take Off");
        }


        IEnumerator WeaponToMonsterTimer()
        {
            yield return new WaitForSeconds(5.0f);

            CWeaponAdapter weapon = Instantiate(weaponAdapter, transform.position, transform.rotation);
            weapon.Init(this);

            Destroy(this.gameObject);
        }
    }
}
