using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        private void Update()
        {
            spinner.Rotate(Vector3.up * 180.0f * Time.deltaTime);
        }

        public override void Apply()
        {
            base.Apply();

            // 양 옆으로 구체 2개 생성
            Instantiate(projectilePrefab, spinner);
            Instantiate(projectilePrefab, spinner);

            spinner.GetChild(0).localPosition = Vector3.right;
            spinner.GetChild(1).localPosition = Vector3.left;
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
                Destroy(projectile.gameObject);
            }
        }
    }
}
