using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject.Homework0804;

namespace MyProject.Skill
{
    public class CArcaneSphere : CSkillBehaviour
    {
        #region public 변수
        public GameObject projectilePrefab;
        #endregion

        #region private 변수
        Transform spinner;
        #endregion

        private void Awake()
        {
            if (transform.childCount > 0)
            {
                spinner = transform.GetChild(0);
            }

            else
            {
                spinner = new GameObject("ArcaneSpinner").transform;
                spinner.SetParent(transform);
                spinner.localPosition = Vector3.up;
            }

            Instantiate(projectilePrefab, spinner);
            Instantiate(projectilePrefab, spinner);
            Instantiate(projectilePrefab, spinner);

            for (int i = 0; i < 3; i++)
            {
                spinner.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            spinner.Rotate(Vector3.up * 180.0f * Time.deltaTime);
        }

        public override void Apply()
        {
            base.Apply();

            if (context.owner.EquipWeapon != null && context.owner.EquipWeapon.WeaponType == CWeapon.EWeaponType.ARCANE)
            {
                for (int i = 0; i < 3; i++)
                {
                    spinner.GetChild(i).gameObject.SetActive(true);
                }

                spinner.GetChild(0).localPosition = Vector3.forward;
                spinner.GetChild(1).localPosition = Vector3.right + Vector3.back;
                spinner.GetChild(2).localPosition = Vector3.left + Vector3.back;
            }

            else
            {
                for (int i = 0; i < 2; i++)
                {
                    spinner.GetChild(i).gameObject.SetActive(true);
                }

                spinner.GetChild(0).localPosition = Vector3.right;
                spinner.GetChild(1).localPosition = Vector3.left;
            }

            if (skill == null)
            {
                skill = new CSkillArcaneSphere();
            }

            skillDamageText.text = $"Damage : {skill.GetDamage()}";
        }

        public override void Use()
        {
            print("비전 구체 스킬은 사용 효과가 없습니다.");
        }

        public override void Remove()
        {
            base.Remove();

            foreach (Transform projectile in spinner)
            {
                projectile.gameObject.SetActive(false);
            }
        }
    }
}
