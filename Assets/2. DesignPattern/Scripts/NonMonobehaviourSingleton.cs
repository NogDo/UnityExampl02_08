using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour�� ������� ���� �Ϲ����� ��ü�� �̱��� ���� ���� ���
public class NonMonobehaviourSingleton
{
    #region private ����
    static NonMonobehaviourSingleton instance;
    #endregion

    // �����ڴ� private���� ������ �ܺο��� �����ڸ� ȣ������ ���ϵ��� ��ȣ
    NonMonobehaviourSingleton() { }

    // instance�� �����ϱ� ���� property�� ����� �б��������� ������ �� �ֵ��� ��
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
