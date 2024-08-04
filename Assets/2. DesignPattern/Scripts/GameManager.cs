using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject;
using System;

public class GameManager : MonoBehaviour
{
    #region public ����
    public new Light light;

    // C#�� event�� ������ ���� ������ ����ȭ�� ������ ������� �����Ƿ� event�� Ȱ���ϴ� �� �����ε� ������ ������ �����ߴٰ� �� �� ����
    public event Action<bool> onDayNightChange;
    public event Action onPlayerDead;

    public float fDayLength = 5.0f;
    public bool isDay = true;
    #endregion

    #region private ����
    // ��� �̱��� �������� ����� ������?
    // ���� å�� ��Ģ�� �����ϴ� �༮�ΰ�??
    static GameManager instance;

    // ������ ���� : Ư�� �ӹ��� �����ϴ� ��ü���� ���� ��ȭ �Ǵ� Ư�� �̺�Ʈ�� ȣ�� ������ �߻��� ��
    // �� �ش� �̺�Ʈ ȣ���� �ʿ��� ��ü���� "���� ���� ���ϸ� �˷��ּ���." ��� ��� �س��� ������ ������ �����̴�.
    List<CMonster> monsters = new();

    float fDayTemp;
    #endregion

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

    /// <summary>
    /// ���Ͱ� Spawn���� �� ������ ���� List�� ���͸� ����Ѵ�.
    /// </summary>
    /// <param name="monster">����� ����</param>
    public void OnMonsterSpawn(CMonster monster)
    {
        monsters.Add(monster);
        monster.OnDayNightChange(isDay);
    }

    /// <summary>
    /// ���Ͱ� Despawn���� �� ��ϵ� ���͸� �����Ѵ�.
    /// </summary>
    /// <param name="monster">������ ����</param>
    public void OnMonsterDespawn(CMonster monster)
    {
        monsters.Remove(monster);
    }

    /// <summary>
    /// �÷��̾� ���� ��ư Ŭ�� (�÷��̾ �׾��� �� ������ Action�� �����Ѵ�)
    /// </summary>
    public void OnPlayerDeadButtonClick()
    {
        onPlayerDead?.Invoke();
    }
}