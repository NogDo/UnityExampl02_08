using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class UIDatabaseUIManager : MonoBehaviour
    {
        #region static 변수
        static UIDatabaseUIManager instance;
        #endregion

        #region public 변수
        [Header("보여질 Panel")]
        public GameObject loginPanel;
        public GameObject infoPanel;

        [Header("로그인 및 회원가입 관련 InputField")]
        public InputField emailInput;
        public InputField pwInput;

        [Header("유저 정보 변경 관련 UI")]
        public InputField changeNameInput;
        public InputField changeProfileInput;
        public Dropdown changeClassDropdown;

        [Header("회원가입, 로그인 버튼")]
        public Button signUpButton;
        public Button loginButton;

        [Header("회원 정보 관련 텍스트")]
        public Text infoText;
        public Text levelText;

        [Header("회원가입 결과 텍스트")]
        public Text signinResultText;

        [Header("회원 탈퇴 버튼")]
        public Button deleteInfoButton;
        #endregion

        #region private 변수
        CUserData userData;
        #endregion

        /// <summary>
        /// 데이터베이스 UI 매니저 객체
        /// </summary>
        public UIDatabaseUIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<UIDatabaseUIManager>();
                }

                return instance;
            }
        }

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }

            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            // 리스너 등록
            loginButton.onClick.AddListener(OnLoginButtonClick);
        }

        /// <summary>
        /// 로그인 버튼이 클릭 됐을 때 실행될 메서드
        /// </summary>
        public void OnLoginButtonClick()
        {
            CDatabaseManager.Instance.Login(emailInput.text, pwInput.text, OnLoginSuccess, OnLoginFailure);
        }

        /// <summary>
        /// 레벨업 버튼이 클릭 됐을 때 실행될 메서드
        /// </summary>
        public void OnLevelUpButtonClick()
        {
            CDatabaseManager.Instance.LevelUp(userData, OnLevelSuccess);
        }

        /// <summary>
        /// 회원가입 버튼이 클릭 됐을 때 실행될 메서드
        /// </summary>
        public void OnSignInButtonClick()
        {
            CDatabaseManager.Instance.SignIn(emailInput.text, pwInput.text, OnSignInSuccess, OnSignInFailure);
        }

        /// <summary>
        /// 이름 변경 Input Field가 Submit 됐을 때 실행될 메서드
        /// </summary>
        public void OnSubmitNameInputField()
        {
            CDatabaseManager.Instance.ChangeName(userData, changeNameInput.text, OnChangeNameSuccess);
        }

        /// <summary>
        /// 클래스 Dropdown의 값이 변경됐을 때 실행될 메서드
        /// </summary>
        public void OnClassDropdownValueChange(int index)
        {
            CDatabaseManager.Instance.ChangeClass(userData, index, OnChangeClassSuccess);
        }

        /// <summary>
        /// 프로필 변경 Input Field가 Submit 됐을 때 실행될 메서드
        /// </summary>
        public void OnSubmitProfileInputField()
        {
            CDatabaseManager.Instance.ChangeProfile(userData, changeProfileInput.text, OnChangeProfileSuccess);
        }

        /// <summary>
        /// 로그인 성공시 호출할 메서드
        /// </summary>
        /// <param name="data">유저 데이터</param>
        void OnLoginSuccess(CUserData data)
        {
            print("로그인 성공!");

            userData = data;

            loginPanel.SetActive(false);
            infoPanel.SetActive(true);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"안녕하세요, {data.name}");
            sb.AppendLine($"이메일 : {data.email}");
            sb.AppendLine($"직업 : {data.charClass}");
            sb.AppendLine($"소개글 : {data.profileText}");

            infoText.text = sb.ToString();
            levelText.text = $"레벨 : {data.level}";
        }

        /// <summary>
        /// 로그인 실패시 호출할 메서드
        /// </summary>
        void OnLoginFailure()
        {
            print("로그인 실패 ㅠㅠ");
        }

        /// <summary>
        /// 회원가입 성공시 호출할 메서드
        /// </summary>
        /// <param name="data">유저 데이터</param>
        void OnSignInSuccess()
        {
            signinResultText.text = "회원가입 성공!";
        }

        /// <summary>
        /// 회원가입 실패시 호출할 메서드
        /// </summary>
        void OnSignInFailure()
        {
            signinResultText.text = "회원가입 실패...";
        }

        /// <summary>
        /// 유저의 레벨 올리는 것에 성공했을 때 호출할 메서드
        /// </summary>
        void OnLevelSuccess()
        {
            levelText.text = $"레벨 : {userData.level}";
        }

        /// <summary>
        /// 유저의 이름을 바꾸는 것에 성공했을 때 호출할 메서드
        /// </summary>
        void OnChangeNameSuccess(CUserData data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"안녕하세요, {data.name}");
            sb.AppendLine($"이메일 : {data.email}");
            sb.AppendLine($"직업 : {data.charClass}");
            sb.AppendLine($"소개글 : {data.profileText}");

            infoText.text = sb.ToString();
            levelText.text = $"레벨 : {data.level}";
        }

        /// <summary>
        /// 유저의 직업을 바꾸는 것에 성공했을 때 호출할 메서드
        /// </summary>
        /// <param name="data"></param>
        void OnChangeClassSuccess(CUserData data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"안녕하세요, {data.name}");
            sb.AppendLine($"이메일 : {data.email}");
            sb.AppendLine($"직업 : {data.charClass}");
            sb.AppendLine($"소개글 : {data.profileText}");

            infoText.text = sb.ToString();
            levelText.text = $"레벨 : {data.level}";
        }

        /// <summary>
        /// 유저의 프로필을 바꾸는 것에 성공했을 때 호출할 메서드
        /// </summary>
        void OnChangeProfileSuccess(CUserData data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"안녕하세요, {data.name}");
            sb.AppendLine($"이메일 : {data.email}");
            sb.AppendLine($"직업 : {data.charClass}");
            sb.AppendLine($"소개글 : {data.profileText}");

            infoText.text = sb.ToString();
            levelText.text = $"레벨 : {data.level}";
        }
    }
}
