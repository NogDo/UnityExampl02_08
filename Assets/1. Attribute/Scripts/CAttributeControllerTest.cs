using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CAttributeControllerTest : MonoBehaviour
{
    #region public ����
    [Color(0.0f, 1.0f, 0.0f, 1.0f), Size(1.0f, 2.0f, 1.0f)]
    public new Renderer renderer;

    [Color, Size]
    public float notRendererOrGraphic;
    #endregion

    #region private ����
    //[Color] // �⺻������ �ߺ��� ������� ������ AllowMultiple�� Ȱ���� �ߺ��� ����� �� �ִ�.
    [SerializeField, Color(r: 1.0f, b: 0.5f), Size(0.5f, 0.5f, 0.5f, ESizeMode.Size)]
    Graphic graphicsSize;

    [SerializeField, Color(r: 0.5f, b: 0.5f), Size(0.5f, 0.5f, 0.5f, ESizeMode.Scale)]
    Graphic graphicsScale;
    #endregion
}