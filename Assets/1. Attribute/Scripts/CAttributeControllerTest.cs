using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CAttributeControllerTest : MonoBehaviour
{
    #region public 변수
    [Color(0.0f, 1.0f, 0.0f, 1.0f), Size(1.0f, 2.0f, 1.0f)]
    public new Renderer renderer;

    [Color, Size]
    public float notRendererOrGraphic;
    #endregion

    #region private 변수
    //[Color] // 기본적으로 중복을 허용하지 않지만 AllowMultiple을 활용해 중복을 허용할 수 있다.
    [SerializeField, Color(r: 1.0f, b: 0.5f), Size(0.5f, 0.5f, 0.5f, ESizeMode.Size)]
    Graphic graphicsSize;

    [SerializeField, Color(r: 0.5f, b: 0.5f), Size(0.5f, 0.5f, 0.5f, ESizeMode.Scale)]
    Graphic graphicsScale;
    #endregion
}