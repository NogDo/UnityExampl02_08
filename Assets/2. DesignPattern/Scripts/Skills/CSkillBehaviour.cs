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
        #region internal 변수
        internal CSkillContext context;
        #endregion

        #region protected 변수
        protected CSkillBase skill;
        #endregion

        #region public 변수
        public Sprite skillIcon;
        public Image skillImage;
        public TextMeshPro skillDamageText;
        #endregion

        /// <summary>
        /// 스킬 등록
        /// </summary>
        public virtual void Apply()
        {
            Debug.Log($"{GetType().Name} skill applied");
            skillImage.sprite = skillIcon;
        }

        /// <summary>
        /// 스킬 사용
        /// </summary>
        public virtual void Use()
        {
            Debug.Log($"{GetType().Name} skill used");
        }

        /// <summary>
        /// 스킬 등록 해제
        /// </summary>
        public virtual void Remove()
        {
            Debug.Log($"{GetType().Name} skill removed");
        }

        /// <summary>
        /// 스킬 데미지 업그레이드
        /// </summary>
        public virtual void DamageUp()
        {
            Debug.Log($"{GetType().Name} skill Damage Up");

            skill = new CSkillDecoratorDamageUpBuff(skill);
            skillDamageText.text = skill.GetDamage().ToString();
        }
    }
}