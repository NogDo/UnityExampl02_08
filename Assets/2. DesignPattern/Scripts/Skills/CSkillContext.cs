using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill
{
    public class CSkillContext : MonoBehaviour
    {
        #region public 변수
        public CPlayer owner;
        public CSkillBehaviour currentSkill;
        #endregion

        // 같은 어셈블리 내에서만 접근 가능하므로, 내가 만든 클래스끼리는 접근 가능하나 유니티 엔진은 접근할 수 없으므로 Inspector에서 수정이 불가하다.
        #region internal 변수
        internal List<CSkillBehaviour> skills = new List<CSkillBehaviour>();
        #endregion

        /// <summary>
        /// 스킬을 추가한다.
        /// </summary>
        /// <param name="skill">스킬</param>
        public void AddSkill(CSkillBehaviour skill)
        {
            skill.context = this;
            skills.Add(skill);
        }

        /// <summary>
        /// 현재 사용할 스킬을 설정한다. (전에 있던 스킬은 지우고 현재 스킬로 설정)
        /// </summary>
        /// <param name="index">스킬 번호</param>
        public void SetCurrentSkill(int index)
        {
            if (index >= skills.Count)
            {
                Debug.LogError("잘못된 Index");
                return;
            }

            currentSkill?.Remove();
            currentSkill = skills[index];
            currentSkill.Apply();
        }

        /// <summary>
        /// 현재 스킬을 사용한다.
        /// </summary>
        public void UseSkill()
        {
            currentSkill.Use();
        }
    }
}
