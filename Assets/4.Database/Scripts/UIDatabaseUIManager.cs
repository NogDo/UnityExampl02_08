using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class UIDatabaseUIManager : MonoBehaviour
    {
        #region static ����
        static UIDatabaseUIManager instance;
        #endregion

        #region public ����
        [Header("������ Panel")]
        public GameObject loginPanel;
        public GameObject infoPanel;

        [Header("�α��� �� ȸ������ ���� InputField")]
        public InputField emailInput;
        public InputField pwInput;

        [Header("���� ���� ���� ���� UI")]
        public InputField changeNameInput;
        public InputField changeProfileInput;
        public Dropdown changeClassDropdown;

        [Header("ȸ������, �α��� ��ư")]
        public Button signUpButton;
        public Button loginButton;

        [Header("ȸ�� ���� ���� �ؽ�Ʈ")]
        public Text infoText;
        public Text levelText;

        [Header("ȸ������ ��� �ؽ�Ʈ")]
        public Text signinResultText;

        [Header("ȸ�� Ż�� ��ư")]
        public Button deleteInfoButton;
        #endregion

        #region private ����
        CUserData userData;
        #endregion

        /// <summary>
        /// �����ͺ��̽� UI �Ŵ��� ��ü
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

            // ������ ���
            loginButton.onClick.AddListener(OnLoginButtonClick);
        }

        /// <summary>
        /// �α��� ��ư�� Ŭ�� ���� �� ����� �޼���
        /// </summary>
        public void OnLoginButtonClick()
        {
            CDatabaseManager.Instance.Login(emailInput.text, pwInput.text, OnLoginSuccess, OnLoginFailure);
        }

        /// <summary>
        /// ������ ��ư�� Ŭ�� ���� �� ����� �޼���
        /// </summary>
        public void OnLevelUpButtonClick()
        {
            CDatabaseManager.Instance.LevelUp(userData, OnLevelSuccess);
        }

        /// <summary>
        /// ȸ������ ��ư�� Ŭ�� ���� �� ����� �޼���
        /// </summary>
        public void OnSignInButtonClick()
        {
            CDatabaseManager.Instance.SignIn(emailInput.text, pwInput.text, OnSignInSuccess, OnSignInFailure);
        }

        /// <summary>
        /// �̸� ���� Input Field�� Submit ���� �� ����� �޼���
        /// </summary>
        public void OnSubmitNameInputField()
        {
            CDatabaseManager.Instance.ChangeName(userData, changeNameInput.text, OnChangeNameSuccess);
        }

        /// <summary>
        /// Ŭ���� Dropdown�� ���� ������� �� ����� �޼���
        /// </summary>
        public void OnClassDropdownValueChange(int index)
        {
            CDatabaseManager.Instance.ChangeClass(userData, index, OnChangeClassSuccess);
        }

        /// <summary>
        /// ������ ���� Input Field�� Submit ���� �� ����� �޼���
        /// </summary>
        public void OnSubmitProfileInputField()
        {
            CDatabaseManager.Instance.ChangeProfile(userData, changeProfileInput.text, OnChangeProfileSuccess);
        }

        /// <summary>
        /// �α��� ������ ȣ���� �޼���
        /// </summary>
        /// <param name="data">���� ������</param>
        void OnLoginSuccess(CUserData data)
        {
            print("�α��� ����!");

            userData = data;

            loginPanel.SetActive(false);
            infoPanel.SetActive(true);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"�ȳ��ϼ���, {data.name}");
            sb.AppendLine($"�̸��� : {data.email}");
            sb.AppendLine($"���� : {data.charClass}");
            sb.AppendLine($"�Ұ��� : {data.profileText}");

            infoText.text = sb.ToString();
            levelText.text = $"���� : {data.level}";
        }

        /// <summary>
        /// �α��� ���н� ȣ���� �޼���
        /// </summary>
        void OnLoginFailure()
        {
            print("�α��� ���� �Ф�");
        }

        /// <summary>
        /// ȸ������ ������ ȣ���� �޼���
        /// </summary>
        /// <param name="data">���� ������</param>
        void OnSignInSuccess()
        {
            signinResultText.text = "ȸ������ ����!";
        }

        /// <summary>
        /// ȸ������ ���н� ȣ���� �޼���
        /// </summary>
        void OnSignInFailure()
        {
            signinResultText.text = "ȸ������ ����...";
        }

        /// <summary>
        /// ������ ���� �ø��� �Ϳ� �������� �� ȣ���� �޼���
        /// </summary>
        void OnLevelSuccess()
        {
            levelText.text = $"���� : {userData.level}";
        }

        /// <summary>
        /// ������ �̸��� �ٲٴ� �Ϳ� �������� �� ȣ���� �޼���
        /// </summary>
        void OnChangeNameSuccess(CUserData data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"�ȳ��ϼ���, {data.name}");
            sb.AppendLine($"�̸��� : {data.email}");
            sb.AppendLine($"���� : {data.charClass}");
            sb.AppendLine($"�Ұ��� : {data.profileText}");

            infoText.text = sb.ToString();
            levelText.text = $"���� : {data.level}";
        }

        /// <summary>
        /// ������ ������ �ٲٴ� �Ϳ� �������� �� ȣ���� �޼���
        /// </summary>
        /// <param name="data"></param>
        void OnChangeClassSuccess(CUserData data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"�ȳ��ϼ���, {data.name}");
            sb.AppendLine($"�̸��� : {data.email}");
            sb.AppendLine($"���� : {data.charClass}");
            sb.AppendLine($"�Ұ��� : {data.profileText}");

            infoText.text = sb.ToString();
            levelText.text = $"���� : {data.level}";
        }

        /// <summary>
        /// ������ �������� �ٲٴ� �Ϳ� �������� �� ȣ���� �޼���
        /// </summary>
        void OnChangeProfileSuccess(CUserData data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"�ȳ��ϼ���, {data.name}");
            sb.AppendLine($"�̸��� : {data.email}");
            sb.AppendLine($"���� : {data.charClass}");
            sb.AppendLine($"�Ұ��� : {data.profileText}");

            infoText.text = sb.ToString();
            levelText.text = $"���� : {data.level}";
        }
    }
}
