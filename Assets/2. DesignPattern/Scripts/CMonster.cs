using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CMonster : MonoBehaviour
    {
        #region public 변수
        public Renderer[] bodyRenderers;
        public Renderer[] eyeRenderers;

        public Color bodyDayColor;
        public Color eyeDayColor;
        public Color bodyNightColor;
        public Color eyeNightColor;
        #endregion

        void Start()
        {
            //GameManager.Instance.OnMonsterSpawn(this);
            GameManager.Instance.onDayNightChange += OnDayNightChange;
            OnDayNightChange(GameManager.Instance.isDay);
        }

        void OnDestroy()
        {
            //GameManager.Instance.OnMonsterDespawn(this);
            GameManager.Instance.onDayNightChange -= OnDayNightChange;
        }

        public void OnDayNightChange(bool isDay)
        {
            if (isDay)
            {
                DayColor();
            }

            else
            {
                NightColor();
            }
        }

        // 퍼사드 패턴 : 복잡한 알고리즘을 메서드 하나로 만들어서 사용
        public void DayColor()
        {
            foreach (Renderer render in bodyRenderers)
            {
                render.material.color = bodyDayColor;
            }

            foreach (Renderer render in eyeRenderers)
            {
                render.material.color = eyeDayColor;
            }
        }


        public void NightColor()
        {
            foreach (Renderer render in bodyRenderers)
            {
                render.material.color = bodyNightColor;
            }

            foreach (Renderer render in eyeRenderers)
            {
                render.material.color = eyeNightColor;
            }
        }
    }
}
