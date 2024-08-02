using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MyProject.Skill;
using MyProject.State;

public class CPlayer : MonoBehaviour
{
    public enum EState
    {
        IDLE = 0,
        MOVE = 1,
        JUMP = 2,
        ATTACK = 3
    }

    #region public ����
    public TextMeshPro text;
    public Transform shotPoint;
    public EState currentState;

    public float fMoveSpeed = 1.0f;
    public float fStateStay;   // ���� ���¿� �ӹ� �ð�
    public float fMoveDistance; // ���� �̵� �Ÿ�
    #endregion

    #region private ����
    CharacterController cc;
    CSkillContext skillContext;
    CStateMachine stateMachine;
    #endregion

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        stateMachine = GetComponent<CStateMachine>();
        skillContext = GetComponentInChildren<CSkillContext>();
        CSkillBehaviour[] skills = skillContext.GetComponentsInChildren<CSkillBehaviour>();

        foreach (CSkillBehaviour skill in skills)
        {
            skillContext.AddSkill(skill);
        }
        skillContext.SetCurrentSkill(0);
    }

    void Start()
    {
        currentState = EState.IDLE;
    }

    void Update()
    {
        Move();
        //StateUpdate();

        if (Input.GetButtonDown("Fire1"))
        {
            skillContext.UseSkill();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skillContext.SetCurrentSkill(0);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skillContext.SetCurrentSkill(1);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skillContext.SetCurrentSkill(2);
        }
    }

    /// <summary>
    /// �÷��̾� �̵�
    /// </summary>
    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, 0, y);

        cc.Move(moveDir * fMoveSpeed * Time.deltaTime);

        // �������̸� �����ϴ� ���� (���� ��������)
        //if (moveDir.magnitude >= 0.1f)
        //{
        //    StateChange(EState.MOVE);
        //}

        //else
        //{
        //    StateChange(EState.IDLE);
        //}


        // �������ϰ� ���������� ���ÿ� ������ ��
        if (moveDir.magnitude >= 0.1f)
        {
            stateMachine.Transition(stateMachine.moveState);
        }

        else
        {
            stateMachine.Transition(stateMachine.idleState);
        }
    }

    /// <summary>
    /// �÷��̾� ���� ����
    /// </summary>
    public void StateChange(EState nextState)
    {
        if (currentState != nextState)
        {
            // ���� ���� exit
            switch (currentState)
            {
                case EState.IDLE:
                    print("��� ���� ����");
                    break;

                case EState.MOVE:
                    print("�̵� ���� ����");
                    break;

                case EState.JUMP:
                    break;

                case EState.ATTACK:
                    break;
            }

            // ���� ���� enter
            switch (nextState)
            {
                case EState.IDLE:
                    print("��� ���� ����");
                    break;

                case EState.MOVE:
                    print("�̵� ���� ����");
                    break;

                case EState.JUMP:
                    break;

                case EState.ATTACK:
                    break;
            }

            currentState = nextState;
            fStateStay = 0.0f;
        }
    }

    /// <summary>
    /// �÷��̾� ���� ����
    /// </summary>
    public void StateUpdate()
    {
        // ���� ���¿� ���� �ൿ ����
        switch (currentState)
        {
            case EState.IDLE:
                text.text = $"{EState.IDLE} state : {fStateStay.ToString("n0")}";
                break;

            case EState.MOVE:
                text.text = $"{EState.MOVE} state : {fStateStay.ToString("n0")}";
                break;

            case EState.JUMP:

                break;

            case EState.ATTACK:

                break;
        }

        fStateStay += Time.deltaTime;
    }
}