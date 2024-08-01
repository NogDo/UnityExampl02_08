using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#region Reflection
/*

Reflection (�ݻ�)

- System.Reflection ���ӽ����̽��� ���Ե� ����̴�.

- ������ Ÿ�ӿ��� ������ Ŭ����, �޼ҵ�, ��� ���� ���� �����͸� ����ϴ� Class

- Attribute�� Ư�� ��ҿ� ���� ��Ÿ������ �̹Ƿ�, Reflection�� ���ؼ� ������ �����ϴ�.
 
*/
#endregion

public class CReflectionTest : MonoBehaviour
{
    #region private ����
    CAttributeTest attributeTest;
    #endregion

    void Awake()
    {
        attributeTest = GetComponent<CAttributeTest>();
    }

    void Start()
    {
        // ���� Ŭ������ boxing
        MonoBehaviour attTestBoxingForm = attributeTest;

        // Attribute�� Type�� Ȯ��
        // ���� Ŭ������ boxing�� �ص� ���� ��ü�� type�� ��ȯ�Ѵ�.
        Type attributeTestType = attTestBoxingForm.GetType();


        BindingFlags bind = BindingFlags.Public | BindingFlags.Instance;
        // ������ Ȯ��
        // �ʵ� (��� ����)�� �����´�.
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

        // TestMethod�� MethodInfo �Ǵ� MemberInfo�� Ž���ؼ� MethodMessageAttribute.msg�� ����غ�����.
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