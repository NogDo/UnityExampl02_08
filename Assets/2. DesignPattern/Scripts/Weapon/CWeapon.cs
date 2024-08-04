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

        #region protected ����
        [SerializeField]
        protected CWeaponAdapter weaponAdapter;
        protected EWeaponType weaponType;

        protected float fAttack;
        protected float fDurability;
        protected float fAttackSpeed;
        protected float fWeight;
        #endregion

        /// <summary>
        /// ������ Ÿ��
        /// </summary>
        public EWeaponType WeaponType
        {
            get
            {
                return weaponType;
            }
        }

        /// <summary>
        /// ���� ���ݷ�
        /// </summary>
        public float Attack
        {
            get
            {
                return fAttack;
            }
        }

        /// <summary>
        /// ���� ������
        /// </summary>
        public float Durability
        {
            get
            {
                return fDurability;
            }
        }

        /// <summary>
        /// ���� ���ݼӵ�
        /// </summary>
        public float AttackSpeed
        {
            get
            {
                return fAttackSpeed;
            }
        }

        /// <summary>
        /// ���� ����
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
        /// ��� �����Ǿ��� �� �ʱ�ȭ
        /// </summary>
        public virtual void Init()
        {
            Debug.Log($"{GetType().Name} Initializeed");
        }

        /// <summary>
        /// ��� �������� �� ����� �޼���
        /// </summary>
        public virtual void Equip(TextMeshPro text, CSkillContext context)
        {
            Debug.Log($"{GetType().Name} Equiped");
        }

        /// <summary>
        /// ��� �������� �� ����� �޼���
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
