using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MyProject.Homework0809
{
    public class CWebTextureRequestAsync : MonoBehaviour
    {
        #region public ����
        public Image[] images;

        public string[] urls;
        #endregion

        void Start()
        {
            for (int i = 0; i < images.Length; i++)
            {
                DownloadTexture(i);
            }
        }

        /// <summary>
        /// �ؽ��ĸ� �ٿ�޴� �޼���
        /// </summary>
        /// <param name="index">�ٿ���� �ؽ��� �ε���</param>
        async void DownloadTexture(int index)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(urls[index]);
            UnityWebRequestAsyncOperation operation = www.SendWebRequest();

            await operation;

            if (www.result != UnityWebRequest.Result.Success)
            {
                print("���� �ҷ����� ����");
            }

            else
            {
                print($"{index + 1}�� ���� �ҷ����� ����");

                Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                Sprite sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                images[index].sprite = sprite;
            }
        }
    }

    public struct UnityWebRequestAwaiter : INotifyCompletion
    {
        private UnityWebRequestAsyncOperation asyncOp;
        private Action continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation op)
        {
            asyncOp = op;
            continuation = null;
        }

        public bool IsCompleted { get { return asyncOp.isDone; } }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
            asyncOp.completed += OnRequestCompleted;
        }

        private void OnRequestCompleted(AsyncOperation op)
        {
            continuation();
        }
    }

    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}
