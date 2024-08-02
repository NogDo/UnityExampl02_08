using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Skill
{
    public class CArcaneSphere : CSkillBehaviour
    {
        #region public ����
        public GameObject projectilePrefab;
        #endregion

        #region private ����
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

            // �� ������ ��ü 2�� ����
            Instantiate(projectilePrefab, spinner);
            Instantiate(projectilePrefab, spinner);

            spinner.GetChild(0).localPosition = Vector3.right;
            spinner.GetChild(1).localPosition = Vector3.left;
        }

        public override void Use()
        {
            print("���� ��ü ��ų�� ��� ȿ���� �����ϴ�.");
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
