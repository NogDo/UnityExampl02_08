using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CFireLightController : MonoBehaviour
    {
        #region private 변수
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
        /// 밤과 낮에 따라 불 이펙트를 활성화 / 비활성화 한다.
        /// </summary>
        /// <param name="isDay">낮인지 밤인지</param>
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
