using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill
{
    public class CSkillContext : MonoBehaviour
    {
        #region public ����
        public CPlayer owner;
        public CSkillBehaviour currentSkill;
        #endregion

        // ���� ����� �������� ���� �����ϹǷ�, ���� ���� Ŭ���������� ���� �����ϳ� ����Ƽ ������ ������ �� �����Ƿ� Inspector���� ������ �Ұ��ϴ�.
        #region internal ����
        internal List<CSkillBehaviour> skills = new List<CSkillBehaviour>();
        #endregion

        /// <summary>
        /// ��ų�� �߰��Ѵ�.
        /// </summary>
        /// <param name="skill">��ų</param>
        public void AddSkill(CSkillBehaviour skill)
        {
            skill.context = this;
            skills.Add(skill);
        }

        /// <summary>
        /// ���� ����� ��ų�� �����Ѵ�. (���� �ִ� ��ų�� ����� ���� ��ų�� ����)
        /// </summary>
        /// <param name="index">��ų ��ȣ</param>
        public void SetCurrentSkill(int index)
        {
            if (index >= skills.Count)
            {
                Debug.LogError("�߸��� Index");
                return;
            }

            currentSkill?.Remove();
            currentSkill = skills[index];
            currentSkill.Apply();
        }

        /// <summary>
        /// ���� ��ų�� ����Ѵ�.
        /// </summary>
        public void UseSkill()
        {
            currentSkill.Use();
        }
    }
}
