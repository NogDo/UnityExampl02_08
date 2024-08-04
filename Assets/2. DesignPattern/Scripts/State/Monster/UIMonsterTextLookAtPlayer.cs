using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class UIMonsterTextLookAtPlayer : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
}