using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


#region Attribute
/*

Attribute (�Ӽ�, Ư��)

- C#������ Attribute�� ��Ȯ�� �ǹ̴� �ʵ�, �޼ҵ�� ����� ���� ��Ÿ�����͸� ������ �� �ִ� [Ŭ����]�̴�.

- Attribute�� Ȱ���ϱ� ���ؼ��� System.Attribute Ŭ������ ����ϰ�, Ŭ������ �ڿ� Attribute�� ���δ�.

- Attribute Ŭ������ Ȱ���ϱ� ���ؼ��� Ư�� ���(Ŭ����, ����, �Լ�(Propertie ����))���� ���� �տ� [Attribute �̸����� Attribute�� �A �̸�]
 
*/
#endregion

public class CAttributeTest : MonoBehaviour
{
    #region public ���� (��� ����)
    [MyCustom(name = "MyIntager", value = 1.0f)]
    public int myInt;
    [MyCustom]  // MyCustomAttribute�� �⺻ �����ڸ� ȣ���Ͽ� ��Ÿ������ ����
    public int myInt2;

    public string myString; // TextArea Attribute�� �������� ���� string ��� ����
    [TextArea(minLines:1, maxLines:10)]
    public string myTextArea; // TextArea Attribute�� ������ string ��� ����

    [Space(300)]
    public int anotherInt;
    #endregion

    #region private ����
    static int myStaticInt;
    static int myStaticInt2;
    #endregion

    [MethodMessage("�̰� private �޼ҵ��Դϴ�.")]
    void TestMethod()
    {
        print($"��н����� Test��. {myInt}");
    }
}

public class MyCustomAttribute : Attribute
{
    public string name;
    public float value;

    public MyCustomAttribute()
    {
        name = "No Name";
        value = -1;
    }
}

/// <summary>
/// �޼ҵ忡 ���� Attribute, AttributeUsage Attribute�� ����� ���������� �� �� �ִ�.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class MethodMessageAttribute : Attribute
{
    public string msg;

    public MethodMessageAttribute(string msg)
    {
        this.msg = msg;
    }
}