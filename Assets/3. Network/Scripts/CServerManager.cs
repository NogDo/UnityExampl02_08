using System;
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
    public class CServerManager : MonoBehaviour
    {
        #region static 변수
        // 모든 스레드가 접근할 수 있는 Data 영역의 Queue
        public static Queue<string> log = new Queue<string>();

        static CServerManager instance;
        #endregion

        #region public 변수
        public Button connectButton;
        public Text messagePrefab;
        public RectTransform textArea;

        public string ipAddress = "127.0.0.1"; // IPv6와의 호환성을 위해 string을 주로 사용한다.
                                               //public byte[] ipAddressArray = { 127, 0, 0, 1 };

        // 0 ~ 65,535 => ushort 사이즈의 숫자만 취급할 수 있으나 (port 주소는 2바이트의 부호 없는 정수를 사용)
        // C#에서는 int로 사용
        // 80 이전의 포트는 사실상 거의 선점이 되어있음
        public int port = 9999;
        #endregion

        #region private 변수
        Thread serverMainThread;
        // 통신에도 활용되지만, 데이터 입출력 등 데이터의 전송을 책임지는 Input, Output 스트림이 필요함
        StreamReader reader;
        StreamWriter writer;
        List<TcpClient> clients = new List<TcpClient>();

        bool isConnected = false;
        #endregion

        /// <summary>
        /// 서버매니저 Instance
        /// </summary>
        public static CServerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CServerManager>();
                }

                return instance;
            }
        }

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            connectButton.onClick.AddListener(ServerConnetButtonClick);
        }

        void Update()
        {
            if (log.Count > 0)
            {
                Text logText = Instantiate(messagePrefab, textArea);
                logText.text = log.Dequeue();
            }
        }

        /// <summary>
        /// 서버 연결 버튼 클릭
        /// </summary>
        public void ServerConnetButtonClick()
        {
            if (false == isConnected)
            {
                // 서버 열기
                //serverMainThread = new Thread(ServerThread);
                //serverMainThread.IsBackground = true;
                //serverMainThread.Start();

                // 과제 서버 열기
                serverMainThread = new Thread(ServerThreadHW);
                serverMainThread.IsBackground = true;
                serverMainThread.Start();

                isConnected = true;
            }

            else
            {
                // 서버 닫기
                serverMainThread.Abort(); // 생성된 스레드를 중단한다.

                isConnected = false;
            }
        }

        /// <summary>
        /// 서버 실행, 멀티 스레드로 생성이 되어야 함 (TcpClient 생성할 때 대기가 걸리기 때문)
        /// </summary>
        void ServerThread()
        {

            /*
            오늘의 과제
            서버 스레드를 List로 관리하여 다중 연결이 가능한 서버로 만들어보세요.
            */

            TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            tcpListener.Start(); // tcp 서버를 가동시킨다.

            // 스레드는 다른 스레드의 있는 데이터를 알 수 없기 때문에 사용할 수 없음
            //Text logText = Instantiate(messagePrefab, textArea);
            //logText.text = "서버 시작";

            // 따라서 Data영역에 있는 정적 변수를 사용
            log.Enqueue("서버 시작");

            TcpClient tcpClient = tcpListener.AcceptTcpClient(); // return이 올 때까지 대기가 걸린다.

            //Text logText2 = Instantiate(messagePrefab, textArea);
            //logText2.text = "클라이언트 연결됨";
            log.Enqueue("클라이언트 연결됨");

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

                //Text messageText = Instantiate(messagePrefab, textArea);
                //messageText.text = readString;

                // 받은 메세지를 그대로 writer에 쓴다.
                writer.WriteLine($"당신의 메세지 : {readString}");

                log.Enqueue($"client message : {readString}");
            }

            log.Enqueue("클라이언트 연결 종료");
        }

        #region 08.05 과제
        /// <summary>
        /// 서버 스레드 실행
        /// </summary>
        void ServerThreadHW()
        {
            try
            {
                /*
                오늘의 과제
                서버 스레드를 List로 관리하여 다중 연결이 가능한 서버로 만들어보세요.
                */

                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start();

                log.Enqueue("서버 시작");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    clients.Add(tcpClient);
                    log.Enqueue("클라이언트 연결됨");

                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(tcpClient);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /// <summary>
        /// 접속한 클라이언트의 스레드
        /// </summary>
        /// <param name="tcpClient"></param>
        void HandleClient(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            int index = clients.Count;

            while (client.Connected)
            {
                string readString = reader.ReadLine();

                if (string.IsNullOrEmpty(readString))
                {
                    continue;
                }

                log.Enqueue($"client message : {readString}");
                foreach (TcpClient c in clients)
                {
                    StreamWriter clientWriter = new StreamWriter(c.GetStream());
                    clientWriter.WriteLine($"{index}번 플레이어의 채팅 : {readString}");
                    clientWriter.Flush();
                }
            }

            log.Enqueue("클라이언트 연결 종료");
        }
        #endregion
    }
}
