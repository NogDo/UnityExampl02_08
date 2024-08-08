using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySqlConnector;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace MyProject
{
    public class CDatabaseManager : MonoBehaviour
    {
        #region static ����
        static CDatabaseManager instance;
        #endregion

        #region private ����
        MySqlConnection conn;   // mysql DB�� ������¸� �����ϴ� ��ü.

        string serverIP = "127.0.0.1";
        string awsServerIP = "3.35.220.113";
        string dbName = "game";
        string tableName = "users";
        string rootPasswd = "1234"; // �׽�Ʈ�ÿ� Ȱ���� �� ������ ���ȿ� ����ϹǷ� ����
        #endregion

        /// <summary>
        /// �����ͺ��̽� �Ŵ��� ��ü
        /// </summary>
        public static CDatabaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CDatabaseManager>();
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
        }

        void Start()
        {
            DBConnect();
        }

        /// <summary>
        /// �����ͺ��̽��� �����Ѵ�.
        /// </summary>
        public void DBConnect()
        {
            string config = $"server={awsServerIP};port=3306;database={dbName};uid=root;pwd={rootPasswd};charset=utf8";

            conn = new MySqlConnection(config);
            conn.Open();
        }

        /// <summary>
        /// �α����� �Ϸ��� �� ��, �α��� ������ ���� ��� �����Ͱ� ���� ���� �� �����Ƿ�
        /// �α����� �Ϸ� �Ǿ��� �� ȣ��� �Լ��� �Ķ���ͷ� �԰� �޾��ֵ��� ��
        /// </summary>
        public void Login(string email, string password, Action<CUserData> successCallback, Action failureCallback)
        {
            // �н����带 �ؽ������� �����
            string pwhash = "";

            //SHA256 sha256 = SHA256.Create();
            //byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            //StringBuilder st = new StringBuilder();
            //foreach (byte b in hashArray)
            //{
            //    st.Append(b.ToString("X2"));
            //}
            //pwhash = st.ToString();

            //sha256.Dispose();   // �ؽ��� �����ϱ� ���1

            // �ؽ��� �����ϱ� ���2 using�� ���! IDisposable �������̽��� ����� �͸� ����
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder st = new StringBuilder();
                foreach (byte b in hashArray)
                {
                    st.Append(b.ToString("X2"));
                }
                pwhash = st.ToString();
            }

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT * FROM {tableName} WHERE email = '{email}' AND pw = '{pwhash}'";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            DataSet set = new DataSet();

            dataAdapter.Fill(set);

            bool isLoginSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

            if (isLoginSuccess)
            {
                // �α��� ���� (email��  pw ���� ���ÿ� ��ġ�ϴ� ���� ������)
                DataRow row = set.Tables[0].Rows[0];
                CUserData data = new CUserData(row);

                successCallback?.Invoke(data);
            }

            else
            {
                // �α��� ����
                failureCallback?.Invoke();
            }
        }

        /// <summary>
        /// ȸ������ �� �����ͺ��̽��� ȸ�������� �����͸� �߰��Ѵ�.
        /// </summary>
        /// <param name="email">�̸���</param>
        /// <param name="password">��й�ȣ</param>
        /// <param name="successCallback">������ ������ �޼���</param>
        /// <param name="failureCallback">���н� ������ �޼���</param>
        public void SignIn(string email, string password, Action successCallback, Action failureCallback)
        {
            string passwordHash = "";

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder st = new StringBuilder();
                foreach (byte b in hashArray)
                {
                    st.Append(b.ToString("X2"));
                }
                passwordHash = st.ToString();
            }

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"INSERT INTO users(email, pw, level, class) VALUES('{email}', '{passwordHash}', '1', '0')";

            int queryCount = cmd.ExecuteNonQuery();

            if (queryCount > 0)
            {
                successCallback?.Invoke();
            }

            else
            {
                failureCallback?.Invoke();
            }
        }

        /// <summary>
        /// ������ �� �����ͺ��̽��� �����͸� ������Ʈ �Ѵ�.
        /// </summary>
        /// <param name="data">���� ������</param>
        /// <param name="successCallback">������ ������ �޼���</param>
        public void LevelUp(CUserData data, Action successCallback)
        {
            int level = data.level;
            int nextLevel = level + 1;

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE users SET level = {nextLevel} WHERE uid = {data.UID}";

            int queryCount = cmd.ExecuteNonQuery();

            if (queryCount > 0)
            {
                // ������ ���������� �����
                data.level = nextLevel;
                successCallback?.Invoke();
            }

            else
            {
                // ���� ���� ����
            }
        }

        /// <summary>
        /// �����ͺ��̽��� ����� �̸��� ������Ʈ �Ѵ�.
        /// </summary>
        /// <param name="data">���� ������</param>
        /// <param name="name">������ �̸�</param>
        /// <param name="succesCallback">������ ������ �޼���</param>
        public void ChangeName(CUserData data, string name, Action<CUserData> successCallback)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE users SET name = '{name}' WHERE uid = {data.UID}";

            int queryCount = cmd.ExecuteNonQuery();

            if (queryCount > 0)
            {
                data.name = name;
                successCallback?.Invoke(data);
            }
        }

        /// <summary>
        /// �����ͺ��̽��� ����� ������ ������Ʈ �Ѵ�.
        /// </summary>
        /// <param name="data">���� ������</param>
        /// <param name="index">���� ��ȣ</param>
        /// <param name="successCallback">������ ������ �޼���</param>
        public void ChangeClass(CUserData data, int index, Action<CUserData> successCallback)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE users SET class = {index} WHERE uid = {data.UID}";

            int queryCount = cmd.ExecuteNonQuery();

            if (queryCount > 0)
            {
                data.charClass = (EClass)index;
                successCallback?.Invoke(data);
            }
        }

        /// <summary>
        /// �����ͺ��̽��� ����� �������� ������Ʈ �Ѵ�.
        /// </summary>
        /// <param name="data">���� ������</param>
        /// <param name="profile">������ ���ڿ�</param>
        /// <param name="successCallback">������ ������ �޼���</param>
        public void ChangeProfile(CUserData data, string profile, Action<CUserData> successCallback) 
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE users SET profile_text = '{profile}' WHERE uid = {data.UID}";

            int queryCount = cmd.ExecuteNonQuery();

            if (queryCount > 0)
            {
                data.profileText = profile;
                successCallback?.Invoke(data);
            }
        }

        /// <summary>
        /// ���� ���� �����͸� �����Ѵ�.
        /// </summary>
        /// <param name="uid">���� ���̵�</param>
        /// <param name="successCallback">������ ������ �޼���</param>
        public void DeleteUserInfo(int uid, Action successCallback)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"DELETE FROM users WHERE uid = {uid}";

            int queryCount = cmd.ExecuteNonQuery();

            if (queryCount > 0)
            {
                successCallback?.Invoke();
            }
        }

        /// <summary>
        /// �����ͺ��̽����� ������ ������ ã�´�.
        /// </summary>
        /// <param name="email">ã�� ������ �̸���</param>
        /// <param name="successCallback">������ ������ �޼���</param>
        /// <param name="failureCallback">���н� ������ �޼���</param>
        public void SearchOtherUserInfo(string email, Action<CUserData> successCallback, Action failureCallback)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT * FROM {tableName} WHERE email = '{email}'";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            DataSet set = new DataSet();

            dataAdapter.Fill(set);

            bool isSelectSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

            if (isSelectSuccess)
            {
                DataRow row = set.Tables[0].Rows[0];
                CUserData data = new CUserData(row);

                successCallback?.Invoke(data);
            }

            else
            {
                failureCallback?.Invoke();
            }
        }
    }
}