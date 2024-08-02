using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour를 상속하지 않은 일반적인 객체의 싱글톤 패턴 적용 방법
public class NonMonobehaviourSingleton
{
    #region private 변수
    static NonMonobehaviourSingleton instance;
    #endregion

    // 생성자는 private으로 선언해 외부에서 생성자를 호출하지 못하도록 보호
    NonMonobehaviourSingleton() { }

    // instance에 접근하기 위해 property를 사용해 읽기전용으로 접근할 수 있도록 함
    public static NonMonobehaviourSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NonMonobehaviourSingleton();
            }

            return instance;
        }
    }
}
