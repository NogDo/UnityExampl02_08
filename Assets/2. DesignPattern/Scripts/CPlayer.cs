using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MyProject.Skill;
using MyProject.State;
using MyProject.Homework0804;

public class CPlayer : MonoBehaviour
{
    public enum EState
    {
        IDLE = 0,
        MOVE = 1,
        JUMP = 2,
        ATTACK = 3
    }

    #region public 변수
    public TextMeshPro textState;
    public Transform shotPoint;
    public EState currentState;

    public float fMoveSpeed = 1.0f;
    public float fStateStay;   // 현재 상태에 머문 시간
    public float fMoveDistance; // 누적 이동 거리
    #endregion

    #region private 변수
    CSkillContext skillContext;
    CStateMachine stateMachine;
    CWeapon currentEquipWeapon;

    CharacterController cc;
    [SerializeField]
    TextMeshPro textWeapon;
    #endregion

    /// <summary>
    /// 현재 장착중인 무기
    /// </summary>
    public CWeapon EquipWeapon
    {
        get
        {
            if (currentEquipWeapon == null)
            {
                return null;
            }

            else
            {
                return currentEquipWeapon;
            }
        }
    }

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


        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    skillContext.SetCurrentSkill(0);
        //}

        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    skillContext.SetCurrentSkill(1);
        //}

        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    skillContext.SetCurrentSkill(2);
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CWeapon>(out CWeapon weapon))
        {
            currentEquipWeapon?.TakeOff();
            currentEquipWeapon = weapon;
            currentEquipWeapon.Equip(textWeapon, skillContext);
        }
    }

    /// <summary>
    /// 플레이어 이동
    /// </summary>
    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, 0, y);

        cc.Move(moveDir * fMoveSpeed * Time.deltaTime);

        // 상태전이를 결정하는 조건 (이전 상태패턴)
        //if (moveDir.magnitude >= 0.1f)
        //{
        //    StateChange(EState.MOVE);
        //}

        //else
        //{
        //    StateChange(EState.IDLE);
        //}


        // 전략패턴과 상태패턴을 동시에 적용한 것
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
    /// 플레이어 상태 전이
    /// </summary>
    public void StateChange(EState nextState)
    {
        if (currentState != nextState)
        {
            // 현재 상태 exit
            switch (currentState)
            {
                case EState.IDLE:
                    print("대기 상태 종료");
                    break;

                case EState.MOVE:
                    print("이동 상태 종료");
                    break;

                case EState.JUMP:
                    break;

                case EState.ATTACK:
                    break;
            }

            // 다음 상태 enter
            switch (nextState)
            {
                case EState.IDLE:
                    print("대기 상태 시작");
                    break;

                case EState.MOVE:
                    print("이동 상태 시작");
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
    /// 플레이어 상태 갱신
    /// </summary>
    public void StateUpdate()
    {
        // 현재 상태에 따른 행동 정의
        switch (currentState)
        {
            case EState.IDLE:
                textState.text = $"{EState.IDLE} state : {fStateStay.ToString("n0")}";
                break;

            case EState.MOVE:
                textState.text = $"{EState.MOVE} state : {fStateStay.ToString("n0")}";
                break;

            case EState.JUMP:

                break;

            case EState.ATTACK:

                break;
        }

        fStateStay += Time.deltaTime;
    }
}