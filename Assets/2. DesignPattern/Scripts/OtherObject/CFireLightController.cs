using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CFireLightController : MonoBehaviour
    {
        #region private ����
        [SerializeField]
        GameObject oFireEffect;
        #endregion

        void OnEnable()
        {
            GameManager.Instance.onDayNightChange += OnDayNightChange;
            OnDayNightChange(GameManager.Instance.isDay);
        }

        void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onDayNightChange -= OnDayNightChange;
            }
        }

        /// <summary>
        /// ��� ���� ���� �� ����Ʈ�� Ȱ��ȭ / ��Ȱ��ȭ �Ѵ�.
        /// </summary>
        /// <param name="isDay">������ ������</param>
        public void OnDayNightChange(bool isDay)
        {
            if (isDay)
            {
                oFireEffect.SetActive(false);
            }

            else
            {
                oFireEffect.SetActive(true);
            }
        }
    }
}
