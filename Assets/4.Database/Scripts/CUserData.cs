using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum EClass
{
    NONE = 0,
    WARRIOR = 1,
    WIZARD = 2,
    ROGUE = 3
}

namespace MyProject
{
    public class CUserData
    {
        #region public 변수
        public string email;
        public string name;
        public int level;
        public EClass charClass;
        public string profileText;

        public int UID => uid;
        #endregion

        #region private 변수
        int uid;
        string passwd;
        #endregion

        public CUserData(DataRow row) : this
            (
                int.Parse(row["uid"].ToString()),
                row["email"].ToString(),
                row["pw"].ToString(),
                int.Parse(row["level"].ToString()),
                row["name"].ToString(),
                (EClass)int.Parse(row["class"].ToString()),
                row["profile_text"].ToString()
            )
        { }

        public CUserData(int uid, string email, string password, int level, string name, EClass charClass, string profileText)
        {
            this.uid = uid;
            this.email = email;
            this.passwd = password;
            this.level = level;
            this.name = name;
            this.charClass = charClass;
            this.profileText = profileText;
        }

        /// <summary>
        /// 현재 계정의 패스워드와 비교한다.
        /// </summary>
        /// <param name="password">비교할 패스워드</param>
        /// <returns></returns>
        public bool ComparePasswd(string password)
        {
            return this.passwd.Equals(password);
        }
    }
}
