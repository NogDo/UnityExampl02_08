using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MyProject
{
    public class CWebRequest : MonoBehaviour
    {
        #region public ����
        public RawImage rawImage;
        public Image image;

        public string imageURL;
        #endregion

        void Start()
        {
            // StartCoroutine�� Coroutine�� return�ϱ� ������ ������� �޸𸮸� �����ϰ� �Ǵµ�
            // return�� �ʿ���ٸ� _�� �Ἥ return�� ������ �� �ִ�.
            _ = StartCoroutine(GetWebTexture(imageURL));
        }

        IEnumerator GetWebTexture(string url)
        {
            // http�� �� ��û(Request)�� ���� ��ü ����
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

            // �񵿱��(�� ����� �ڷ�ƾ)���� Response�� ���� ������ ���
            var operation = www.SendWebRequest();
            yield return operation;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"HTTP ��� ���� : {www.error}");
            }

            else
            {
                Debug.Log("�ؽ��� �ٿ�ε� ����!");
                //rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                Sprite sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                image.sprite = sprite;
                image.SetNativeSize();
            }
        }
    }
}
