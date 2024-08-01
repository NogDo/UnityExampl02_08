using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


#region Attribute
/*

Attribute (속성, 특성)

- C#에서의 Attribute의 정확한 의미는 필드, 메소드등 멤버에 대한 메타데이터를 생성할 수 있는 [클래스]이다.

- Attribute를 활용하기 위해서는 System.Attribute 클래스를 상속하고, 클래스명 뒤에 Attribute를 붙인다.

- Attribute 클래스를 활용하기 위해서는 특정 멤버(클래스, 변수, 함수(Propertie 포함))등의 선언 앞에 [Attribute 이름에서 Attribute를 뺸 이름]
 
*/
#endregion

public class CAttributeTest : MonoBehaviour
{
    #region public 변수 (멤버 변수)
    [MyCustom(name = "MyIntager", value = 1.0f)]
    public int myInt;
    [MyCustom]  // MyCustomAttribute의 기본 생성자를 호출하여 메타데이터 생성
    public int myInt2;

    public string myString; // TextArea Attribute가 부착되지 않은 string 멤버 변수
    [TextArea(minLines:1, maxLines:10)]
    public string myTextArea; // TextArea Attribute가 부착된 string 멤버 변수

    [Space(300)]
    public int anotherInt;
    #endregion

    #region private 변수
    static int myStaticInt;
    static int myStaticInt2;
    #endregion

    [MethodMessage("이건 private 메소드입니다.")]
    void TestMethod()
    {
        print($"비밀스러운 Test중. {myInt}");
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
/// 메소드에 붙일 Attribute, AttributeUsage Attribute를 사용해 제약조건을 걸 수 있다.
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