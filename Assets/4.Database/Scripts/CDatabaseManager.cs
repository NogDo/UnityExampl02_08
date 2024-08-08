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
        #region static 변수
        static CDatabaseManager instance;
        #endregion

        #region private 변수
        MySqlConnection conn;   // mysql DB와 연결상태를 유지하는 객체.

        string serverIP = "127.0.0.1";
        string awsServerIP = "3.35.220.113";
        string dbName = "game";
        string tableName = "users";
        string rootPasswd = "1234"; // 테스트시에 활용할 수 있지만 보안에 취약하므로 주의
        #endregion

        /// <summary>
        /// 데이터베이스 매니저 객체
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
        /// 데이터베이스와 연결한다.
        /// </summary>
        public void DBConnect()
        {
            string config = $"server={awsServerIP};port=3306;database={dbName};uid=root;pwd={rootPasswd};charset=utf8";

            conn = new MySqlConnection(config);
            conn.Open();
        }

        /// <summary>
        /// 로그인을 하려고 할 때, 로그인 쿼리를 날린 즉시 데이터가 오지 않을 수 있으므로
        /// 로그인이 완료 되었을 때 호출될 함수를 파라미터로 함게 받아주도록 함
        /// </summary>
        public void Login(string email, string password, Action<CUserData> successCallback, Action failureCallback)
        {
            // 패스워드를 해쉬값으로 만들기
            string pwhash = "";

            //SHA256 sha256 = SHA256.Create();
            //byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            //StringBuilder st = new StringBuilder();
            //foreach (byte b in hashArray)
            //{
            //    st.Append(b.ToString("X2"));
            //}
            //pwhash = st.ToString();

            //sha256.Dispose();   // 해쉬값 해제하기 방법1

            // 해쉬값 해제하기 방법2 using을 사용! IDisposable 인터페이스를 상속한 것만 가능
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
                // 로그인 성공 (email과  pw 값이 동시에 일치하는 행이 존재함)
                DataRow row = set.Tables[0].Rows[0];
                CUserData data = new CUserData(row);

                successCallback?.Invoke(data);
            }

            else
            {
                // 로그인 실패
                failureCallback?.Invoke();
            }
        }

        /// <summary>
        /// 회원가입 시 데이터베이스에 회원가입한 데이터를 추가한다.
        /// </summary>
        /// <param name="email">이메일</param>
        /// <param name="password">비밀번호</param>
        /// <param name="successCallback">성공시 실행할 메서드</param>
        /// <param name="failureCallback">실패시 실행할 메서드</param>
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
        /// 레벨업 시 데이터베이스의 데이터를 업데이트 한다.
        /// </summary>
        /// <param name="data">유저 데이터</param>
        /// <param name="successCallback">성공시 실행할 메서드</param>
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
                // 쿼리가 정상적으로 수행됨
                data.level = nextLevel;
                successCallback?.Invoke();
            }

            else
            {
                // 쿼리 수행 실패
            }
        }

        /// <summary>
        /// 데이터베이스에 변경된 이름을 업데이트 한다.
        /// </summary>
        /// <param name="data">유저 데이터</param>
        /// <param name="name">변경할 이름</param>
        /// <param name="succesCallback">성공시 실행할 메서드</param>
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
        /// 데이터베이스에 변경된 직업을 업데이트 한다.
        /// </summary>
        /// <param name="data">유저 데이터</param>
        /// <param name="index">직업 번호</param>
        /// <param name="successCallback">성공시 실행할 메서드</param>
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
        /// 데이터베이스에 변경된 프로필을 업데이트 한다.
        /// </summary>
        /// <param name="data">유저 데이터</param>
        /// <param name="profile">프로필 문자열</param>
        /// <param name="successCallback">성공시 실행할 메서드</param>
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
        /// 현재 유저 데이터를 삭제한다.
        /// </summary>
        /// <param name="uid">유저 아이디</param>
        /// <param name="successCallback">성공시 실행할 메서드</param>
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
        /// 데이터베이스에서 유저의 정보를 찾는다.
        /// </summary>
        /// <param name="email">찾을 유저의 이메일</param>
        /// <param name="successCallback">성공시 실행할 메서드</param>
        /// <param name="failureCallback">실패시 실행할 메서드</param>
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