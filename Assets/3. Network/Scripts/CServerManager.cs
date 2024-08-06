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
        List<TcpClient> clients = new List<TcpClient>(); // 08.05 과제용 리스트
        List<ClientHandler> clientHandlers = new List<ClientHandler>(); // 08.06 수업시간용
        List<Thread> threads = new List<Thread>();

        bool isConnected = false;
        int nClientId = 0;
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
                serverMainThread = new Thread(ServerThread);
                serverMainThread.IsBackground = true;
                serverMainThread.Start();

                // 과제 서버 열기
                //serverMainThread = new Thread(ServerThreadHW);
                //serverMainThread.IsBackground = true;
                //serverMainThread.Start();

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
            // try/catch문
            // 용도 : Exception 발생시 메시지를 수동으로 활용 할 수 있도록 함.
            // 잘 제어된 if-else문과 비슷하다.
            try
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start(); // tcp 서버를 가동시킨다.

                // 스레드는 다른 스레드의 있는 데이터를 알 수 없기 때문에 사용할 수 없음
                //Text logText = Instantiate(messagePrefab, textArea);
                //logText.text = "서버 시작";

                // 따라서 Data영역에 있는 정적 변수를 사용
                log.Enqueue("서버 시작");

                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();

                    ClientHandler handler = new ClientHandler();
                    handler.Connect(nClientId++, this, client);

                    clientHandlers.Add(handler);

                    Thread clientThread = new Thread(handler.Run);
                    clientThread.IsBackground = true;
                    clientThread.Start();

                    threads.Add(clientThread);

                    log.Enqueue($"{handler.id}번 클라이언트가 접속됨.");
                }
            }
            catch (ArgumentException ex)
            {
                log.Enqueue("파라미터 에러 발생");
                log.Enqueue(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                log.Enqueue("널 포인터 에러 발생");
                log.Enqueue(ex.Message);
            }
            catch (Exception ex) // try문 내의 구문중에 에러가 발생할 시 호출
            {
                log.Enqueue("에러 발생");
                log.Enqueue(ex.Message);
            }
            finally // try문 내에서 에러가 발생해도 실행되고, 발생하지 않아도 실행된다.
                    // 중간에 흐름이 끊기지 않고 생성된 객체를 해제 하는 등의 반드시 필요한 절차를 여기서 수행하게 됨.
            {
                foreach (Thread thread in threads)
                {
                    thread?.Abort();
                }
            }
        }

        /// <summary>
        /// 클라이언트가 해제됐을 때 리스트에서 해당 클라이언트를 삭제한다
        /// </summary>
        /// <param name="client">삭제할 클라이언트</param>
        public void Disconnect(ClientHandler client)
        {
            clientHandlers.Remove(client);
        }

        /// <summary>
        /// 클라이언트에게 메세지를 받았을 때 서버와 다른 모든 클라이언트에게 메세지를 뿌리는 메서드
        /// </summary>
        /// <param name="message">메세지</param>
        public void BroadcastToClients(string message)
        {
            log.Enqueue(message);

            foreach (ClientHandler client in clientHandlers)
            {
                client.MessageToClient(message);
            }
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

                log.Enqueue($"{index}번 플레이어의 채팅 : {readString}");
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

    /// <summary>
    /// 클라이언트가 TCP 접속 요청을 할 때마다 해당 클라이언트를 붙들고 잇는 객체를 생성한다.
    /// </summary>
    public class ClientHandler
    {
        public int id;
        public CServerManager server;
        public TcpClient tcpClient;
        public StreamReader reader;
        public StreamWriter writer;

        /// <summary>
        /// 서버에 클라이언트를 접속시키기 위해 초기화를 진행
        /// </summary>
        /// <param name="id">클라이언트 id</param>
        /// <param name="server">서버매니저</param>
        /// <param name="tcpClient">클라이언트</param>
        public void Connect(int id, CServerManager server, TcpClient tcpClient)
        {
            this.id = id;
            this.server = server;
            this.tcpClient = tcpClient;
            reader = new StreamReader(tcpClient.GetStream());
            writer = new StreamWriter(tcpClient.GetStream());
            writer.AutoFlush = true;
        }

        /// <summary>
        /// 클라이언트가 접속을 끊었을 때 GC가 메모리를 수집할 수 있도록 하기 위한 메서드
        /// </summary>
        public void Disconnect()
        {
            writer.Close();
            reader.Close();
            tcpClient.Close();
            server.Disconnect(this);
        }

        /// <summary>
        /// 클라이언트로 메세지를 보낸다.
        /// </summary>
        /// <param name="message">메세지</param>
        public void MessageToClient(string message)
        {
            writer.WriteLine(message);
        }

        /// <summary>
        /// 연결이 된 후 실행할 메서드
        /// </summary>
        public void Run()
        {
            try
            {
                while (tcpClient.Connected)
                {
                    string readString = reader.ReadLine();

                    if (string.IsNullOrEmpty(readString))
                    {
                        continue;
                    }

                    // 읽어온 메세지가 있으면 서버에게 전달
                    server.BroadcastToClients($"{id} 님의 말 : {readString}");
                }
            }
            catch (Exception ex)
            {
                CServerManager.log.Enqueue($"{id}번 클라이언트 오류 발생 : {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
