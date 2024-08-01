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

            // Linq�� ������� ���� ���
            // �÷���(�迭, ����Ʈ ��)���� Ư�� ���ǿ� �����ϴ� ��Ҹ� �������� �� ��� foreach �Ǵ� List.Find�� �ټ� ������ ������ ���ľ� �Ѵ�.
            //List<FieldInfo> fields = new List<FieldInfo>(type.GetFields(bind));
            //fields.FindAll(null);

            // Linq�� ����� ���
            // 1. Linq�� ���ǵ� Ȯ�� �޼��带 �̿��ϴ� ���
            IEnumerable<FieldInfo> colorAttachedFields = type.GetFields(bind).Where(field => field.GetCustomAttribute<ColorAttribute>() != null);

            // 2. Linq�� ���� �������� ����� ������ Ȱ���ϴ� ���
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
                    //throw new Exception("����, Color Attribute�� �߸� ���̼̳׿� ����");
                    Debug.LogError("����, Color Attribute�� �߸� ���̼̳׿� ����");
                }
            }
        }


        // ����
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
                    Debug.LogError("SizeAttribute�� ��������!");
                }
            }
        }
    }
}


// Color�� ������ �� �ִ� ������Ʈ �Ǵ� ������Ʈ�� [Color]��� Attribute�� �ٿ��� ���� ����
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ColorAttribute : Attribute
{
    public Color color;

    // Attribute�� �����ڿ����� ���ͷ� Ÿ���� �Ű������� �Ҵ��� �����ϴ�.
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
    // Ư�� Attribute�� ������ �ִ� ���θ� Ȯ���ϰ� ���� �� �� Ȯ�� �޼���
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