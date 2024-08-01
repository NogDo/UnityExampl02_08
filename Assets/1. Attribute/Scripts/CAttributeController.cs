using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using UnityEngine.UI;

public enum ESizeMode
{
    Scale,
    Size
}

public class CAttributeController : MonoBehaviour
{
    void Start()
    {
        BindingFlags bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (MonoBehaviour mono in monoBehaviours)
        {
            Type type = mono.GetType();

            // Linq를 사용하지 않은 방식
            // 컬렉션(배열, 리스트 등)에서 특정 조건에 부합하는 요소만 가져오려 할 경우 foreach 또는 List.Find등 다소 복잡한 절차를 거쳐야 한다.
            //List<FieldInfo> fields = new List<FieldInfo>(type.GetFields(bind));
            //fields.FindAll(null);

            // Linq를 사용한 방식
            // 1. Linq에 정의된 확장 메서드를 이용하는 방법
            IEnumerable<FieldInfo> colorAttachedFields = type.GetFields(bind).Where(field => field.GetCustomAttribute<ColorAttribute>() != null);

            // 2. Linq를 통해 쿼리문과 비슷한 문법을 활용하는 방법
            colorAttachedFields = from field in type.GetFields(bind)
                                  where field.HasAttribute<ColorAttribute>()
                                  select field;

            foreach (FieldInfo feild in colorAttachedFields)
            {
                ColorAttribute att = feild.GetCustomAttribute<ColorAttribute>();

                object value = feild.GetValue(mono);

                if (value is Renderer rend)
                {
                    rend.material.color = att.color;
                }

                else if (value is Graphic graph)
                {
                    graph.color = att.color;
                }

                else
                {
                    //throw new Exception("저런, Color Attribute를 잘못 붙이셨네요 ㅎㅎ");
                    Debug.LogError("저런, Color Attribute를 잘못 붙이셨네요 ㅎㅎ");
                }
            }
        }


        // 과제
        foreach (MonoBehaviour mono in monoBehaviours)
        {
            Type type = mono.GetType();

            IEnumerable<FieldInfo> sizeAttachedFields = from field in type.GetFields(bind)
                                                        where field.HasAttribute<ColorAttribute>()
                                                        select field;

            foreach (FieldInfo field in sizeAttachedFields)
            {
                SizeAttribute att = field.GetCustomAttribute<SizeAttribute>();

                object value = field.GetValue(mono);

                if (value is Renderer rend)
                {
                    rend.transform.localScale = att.size;
                }

                else if (value is Graphic graphic)
                {
                    switch (att.mode)
                    {
                        case ESizeMode.Scale:
                            graphic.rectTransform.localScale = att.size;
                            break;

                        case ESizeMode.Size:
                            graphic.rectTransform.sizeDelta = 
                                new Vector2(graphic.rectTransform.sizeDelta.x * att.size.x, graphic.rectTransform.sizeDelta.y * att.size.y);
                            break;
                    }
                }

                else
                {
                    Debug.LogError("SizeAttribute가 붙지못함!");
                }
            }
        }
    }
}


// Color를 조절할 수 있는 컴포넌트 또는 오브젝트에 [Color]라는 Attribute를 붙여서 색을 설정
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ColorAttribute : Attribute
{
    public Color color;

    // Attribute의 생성자에서는 리터럴 타입의 매개변수만 할당이 가능하다.
    public ColorAttribute(float r = 0, float g = 0, float b = 0, float a = 1)
    {
        color = new Color(r, g, b, a);
    }

    public ColorAttribute()
    {
        color = Color.black;
    }
}

public static class AttributeHelper
{
    // 특정 Attribute를 가지고 있는 여부만 확인하고 싶을 때 쓸 확장 메서드
    public static bool HasAttribute<T>(this MemberInfo info) where T : Attribute
    {
        return info.GetCustomAttributes(typeof(T), true).Length > 0;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class SizeAttribute : Attribute
{
    public Vector3 size;
    public ESizeMode mode;

    public SizeAttribute(float x, float y, float z)
    {
        size = new Vector3(x, y, z);
    }

    public SizeAttribute(float x, float y, float z, ESizeMode mode)
    {
        size = new Vector3(x, y, z);
        this.mode = mode;
    }

    public SizeAttribute()
    {
        size = Vector3.one;
    }
}