using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MyProject.Homework0809
{
    public class CWebTextureRequestCoroutine : MonoBehaviour
    {
        #region public ����
        public Image[] images;

        public string[] urls;
        #endregion

        void Start()
        {
            _ = StartCoroutine(DownloadTexture());
        }

        /// <summary>
        /// �ؽ��ĸ� �ٿ�ε� �ϴ� �ڷ�ƾ
        /// </summary>
        /// <returns></returns>
        IEnumerator DownloadTexture()
        {
            for (int i = 0; i < images.Length; i++)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(urls[i]);

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    print("���� �ҷ����� ����");
                }

                else
                {
                    print($"{i + 1}�� ���� �ҷ����� ����");

                    Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                    Sprite sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    images[i].sprite = sprite;
                }
            }
        }
    }
}
