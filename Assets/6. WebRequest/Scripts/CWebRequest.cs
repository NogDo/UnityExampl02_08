using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MyProject
{
    public class CWebRequest : MonoBehaviour
    {
        #region public 변수
        public RawImage rawImage;
        public Image image;

        public string imageURL;
        #endregion

        void Start()
        {
            // StartCoroutine은 Coroutine을 return하기 때문에 쓸모없는 메모리를 낭비하게 되는데
            // return이 필요없다면 _를 써서 return을 무시할 수 있다.
            _ = StartCoroutine(GetWebTexture(imageURL));
        }

        IEnumerator GetWebTexture(string url)
        {
            // http로 웹 요청(Request)를 보낼 객체 생성
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

            // 비동기식(을 모방한 코루틴)으로 Response를 받을 때까지 대기
            var operation = www.SendWebRequest();
            yield return operation;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"HTTP 통신 실패 : {www.error}");
            }

            else
            {
                Debug.Log("텍스쳐 다운로드 성공!");
                //rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                Sprite sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                image.sprite = sprite;
                image.SetNativeSize();
            }
        }
    }
}
