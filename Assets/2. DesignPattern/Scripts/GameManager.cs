using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject;
using System;

public class GameManager : MonoBehaviour
{
    #region public 변수
    // 어떤걸 싱글톤 패턴으로 만들면 좋은가?
    // 단일 책임 원칙에 부합하는 녀석인가??
    public static GameManager Instance { get; private set; }
    public new Light light;

    // C#의 event는 옵저버 패턴 구현에 최적화된 구조로 만들어져 있으므로 event를 활용하는 것 만으로도 옵저버 패턴을 적용했다고 볼 수 있음
    public event Action<bool> onDayNightChange;

    public float fDayLength = 5.0f;
    public bool isDay = true;
    #endregion

    #region private 변수
    // 옵저버 패턴 : 특정 임무를 수행하는 객체에게 상태 변화 또는 특정 이벤트의 호출 조건이 발생할 시
    // ㄴ 해당 이벤트 호출이 필요한 객체들이 "나도 상태 변하면 알려주세요." 라고 등록 해놓는 형태의 디자인 패턴이다.
    List<CMonster> monsters = new();

    float fDayTemp;
    #endregion


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Time.time - fDayTemp > fDayLength)
        {
            fDayTemp = Time.time;
            isDay = !isDay;
            light.gameObject.SetActive(isDay);

            // 옵저버가 구독자들에게 메시지를 보냄
            // 반복문을 이용한 방식
            //foreach (CMonster monster in monsters)
            //{
            //    monster.OnDayNightChange(isDay);
            //}

            // Action을 이용한 방식
            onDayNightChange?.Invoke(isDay);
        }
    }


    public void OnMonsterSpawn(CMonster monster)
    {
        monsters.Add(monster);
        monster.OnDayNightChange(isDay);
    }


    public void OnMonsterDespawn(CMonster monster)
    {
        monsters.Remove(monster);
    }
}