using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject;
using System;

public class GameManager : MonoBehaviour
{
    #region public ����
    // ��� �̱��� �������� ����� ������?
    // ���� å�� ��Ģ�� �����ϴ� �༮�ΰ�??
    public static GameManager Instance { get; private set; }
    public new Light light;

    // C#�� event�� ������ ���� ������ ����ȭ�� ������ ������� �����Ƿ� event�� Ȱ���ϴ� �� �����ε� ������ ������ �����ߴٰ� �� �� ����
    public event Action<bool> onDayNightChange;

    public float fDayLength = 5.0f;
    public bool isDay = true;
    #endregion

    #region private ����
    // ������ ���� : Ư�� �ӹ��� �����ϴ� ��ü���� ���� ��ȭ �Ǵ� Ư�� �̺�Ʈ�� ȣ�� ������ �߻��� ��
    // �� �ش� �̺�Ʈ ȣ���� �ʿ��� ��ü���� "���� ���� ���ϸ� �˷��ּ���." ��� ��� �س��� ������ ������ �����̴�.
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

            // �������� �����ڵ鿡�� �޽����� ����
            // �ݺ����� �̿��� ���
            //foreach (CMonster monster in monsters)
            //{
            //    monster.OnDayNightChange(isDay);
            //}

            // Action�� �̿��� ���
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