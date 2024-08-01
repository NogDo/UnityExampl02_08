using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#region Reflection
/*

Reflection (반사)

- System.Reflection 네임스페이스에 포함된 기능이다.

- 컴파일 타임에서 생성된 클래스, 메소드, 멤버 변수 등의 데이터를 취급하는 Class

- Attribute는 특정 요소에 대한 메타데이터 이므로, Reflection에 의해서 접근이 가능하다.
 
*/
#endregion

public class CReflectionTest : MonoBehaviour
{
    #region private 변수
    CAttributeTest attributeTest;
    #endregion

    void Awake()
    {
        attributeTest = GetComponent<CAttributeTest>();
    }

    void Start()
    {
        // 상위 클래스로 boxing
        MonoBehaviour attTestBoxingForm = attributeTest;

        // Attribute의 Type을 확인
        // 상위 클래스로 boxing을 해도 원래 객체의 type을 반환한다.
        Type attributeTestType = attTestBoxingForm.GetType();


        BindingFlags bind = BindingFlags.Public | BindingFlags.Instance;
        // 데이터 확인
        // 필드 (멤버 변수)를 가져온다.
        FieldInfo[] fis = attributeTestType.GetFields(bind);

        print(fis.Length);
        foreach (FieldInfo fi in fis)
        {
            if (fi.GetCustomAttribute<MyCustomAttribute>() == null)
            {
                continue;
            }

            MyCustomAttribute customAtt = fi.GetCustomAttribute<MyCustomAttribute>();

            print($"Name : {fi.Name}, Type : {fi.FieldType}, AttName : {customAtt.name}, AttValue : {customAtt.value}");
        }

        // TestMethod의 MethodInfo 또는 MemberInfo를 탐색해서 MethodMessageAttribute.msg를 출력해보세요.
        MethodInfo[] mis = attributeTestType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (MethodInfo mi in mis)
        {
            if (mi.GetCustomAttribute<MethodMessageAttribute>() == null)
            {
                continue;
            }

            MethodMessageAttribute custom = mi.GetCustomAttribute<MethodMessageAttribute>();
            print(custom.msg);
            mi.Invoke(attributeTest, null);
        }
    }
}