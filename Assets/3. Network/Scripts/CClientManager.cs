using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class CClientManager : MonoBehaviour
    {
        #region static 변수
        public static Queue<string> log = new Queue<string>();
        #endregion

        #region public 변수
        public Text messagePrefab;
        public RectTransform textArea;

        public Button connectButton;
        public InputField ip;
        public InputField port;

        public InputField messageInput;
        public Button sendButton;
        #endregion

        #region private 변수
        Thread clientThread;
        StreamReader reader;
        StreamWriter writer;

        bool isConneted = false;
        #endregion

        void Awake()
        {
            // 코드로 Action등록
            //connectButton.onClick.AddListener(ConnectButtonClick);
            //messageInput.onSubmit.AddListener(MessageToServer);
        }

        void Update()
        {
            if (log.Count > 0)
            {
                Instantiate(messagePrefab, textArea).text = log.Dequeue();
            }
        }

        /// <summary>
        /// 연결 버튼 클릭
        /// </summary>
        public void ConnectButtonClick()
        {
            if (false == isConneted)
            {
                // 서버랑 접속 시도
                clientThread = new Thread(ClientThread);
                clientThread.IsBackground = true;
                clientThread.Start();

                isConneted = false;
            }

            else
            {
                // 접속 끊기
                clientThread.Abort();

                isConneted = true;
            }
        }

        /// <summary>
        /// 서버랑 연결
        /// </summary>
        void ClientThread()
        {
            TcpClient tcpClient = new TcpClient();

            IPAddress serverAddress = IPAddress.Parse(ip.text);
            int portNum = int.Parse(port.text);

            IPEndPoint endPoint = new IPEndPoint(serverAddress, portNum);

            tcpClient.Connect(endPoint);

            log.Enqueue($"서버에 접속됨. IP : {endPoint.Address}");

            reader = new StreamReader(tcpClient.GetStream());
            writer = new StreamWriter(tcpClient.GetStream());
            writer.AutoFlush = true;

            while (tcpClient.Connected)
            {
                string readString = reader.ReadLine();

                if (string.IsNullOrEmpty(readString))
                {
                    continue;
                }

                log.Enqueue(readString);
            }

            log.Enqueue("접속 종료");
        }

        /// <summary>
        /// 서버로 메세지를 보낸다. (inputField의 OnSubmit에서 호출)
        /// </summary>
        /// <param name="message">보낼 메세지</param>
        public void MessageToServer(string message)
        {
            writer.WriteLine(message);

            messageInput.text = "";
        }
    }
}
