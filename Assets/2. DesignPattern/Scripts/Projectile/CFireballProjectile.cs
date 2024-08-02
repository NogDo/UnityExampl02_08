using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CFireballProjectile : MonoBehaviour
    {
        #region private 변수
        Rigidbody rb;
        #endregion

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// 투사체를 날리기 위해 속도와 방향을 설정한다.
        /// </summary>
        /// <param name="speed">속도</param>
        public void SetProjectile(float speed)
        {
            rb.velocity = Vector3.forward * speed;
        }
    }
}
