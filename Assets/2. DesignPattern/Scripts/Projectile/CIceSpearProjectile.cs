using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CIceSpearProjectile : MonoBehaviour
    {
        #region private ����
        Rigidbody rb;
        #endregion

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// ����ü�� ������ ���� Rigidbody�� �ӵ��� ����
        /// </summary>
        /// <param name="speed">�ӵ�</param>
        public void SetProjectile(float speed)
        {
            rb.velocity = Vector3.forward * speed;
        }
    }
}
