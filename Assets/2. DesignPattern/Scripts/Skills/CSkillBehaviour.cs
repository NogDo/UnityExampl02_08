using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyProject.Homework0804;

namespace MyProject.Skill
{
    public class CSkillBehaviour : MonoBehaviour
    {
        #region internal ����
        internal CSkillContext context;
        #endregion

        #region protected ����
        protected CSkillBase skill;
        #endregion

        #region public ����
        public Sprite skillIcon;
        public Image skillImage;
        public TextMeshPro skillDamageText;
        #endregion

        /// <summary>
        /// ��ų ���
        /// </summary>
        public virtual void Apply()
        {
            Debug.Log($"{GetType().Name} skill applied");
            skillImage.sprite = skillIcon;
        }

        /// <summary>
        /// ��ų ���
        /// </summary>
        public virtual void Use()
        {
            Debug.Log($"{GetType().Name} skill used");
        }

        /// <summary>
        /// ��ų ��� ����
        /// </summary>
        public virtual void Remove()
        {
            Debug.Log($"{GetType().Name} skill removed");
        }

        /// <summary>
        /// ��ų ������ ���׷��̵�
        /// </summary>
        public virtual void DamageUp()
        {
            Debug.Log($"{GetType().Name} skill Damage Up");

            skill = new CSkillDecoratorDamageUpBuff(skill);
            skillDamageText.text = skill.GetDamage().ToString();
        }
    }
}