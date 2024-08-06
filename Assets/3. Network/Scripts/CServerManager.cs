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
        #region static ����
        // ��� �����尡 ������ �� �ִ� Data ������ Queue
        public static Queue<string> log = new Queue<string>();

        static CServerManager instance;
        #endregion

        #region public ����
        public Button connectButton;
        public Text messagePrefab;
        public RectTransform textArea;

        public string ipAddress = "127.0.0.1"; // IPv6���� ȣȯ���� ���� string�� �ַ� ����Ѵ�.
                                               //public byte[] ipAddressArray = { 127, 0, 0, 1 };

        // 0 ~ 65,535 => ushort �������� ���ڸ� ����� �� ������ (port �ּҴ� 2����Ʈ�� ��ȣ ���� ������ ���)
        // C#������ int�� ���
        // 80 ������ ��Ʈ�� ��ǻ� ���� ������ �Ǿ�����
        public int port = 9999;
        #endregion

        #region private ����
        Thread serverMainThread;
        // ��ſ��� Ȱ�������, ������ ����� �� �������� ������ å������ Input, Output ��Ʈ���� �ʿ���
        StreamReader reader;
        StreamWriter writer;
        List<TcpClient> clients = new List<TcpClient>(); // 08.05 ������ ����Ʈ
        List<ClientHandler> clientHandlers = new List<ClientHandler>(); // 08.06 �����ð���
        List<Thread> threads = new List<Thread>();

        bool isConnected = false;
        int nClientId = 0;
        #endregion

        /// <summary>
        /// �����Ŵ��� Instance
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
        /// ���� ���� ��ư Ŭ��
        /// </summary>
        public void ServerConnetButtonClick()
        {
            if (false == isConnected)
            {
                // ���� ����
                serverMainThread = new Thread(ServerThread);
                serverMainThread.IsBackground = true;
                serverMainThread.Start();

                // ���� ���� ����
                //serverMainThread = new Thread(ServerThreadHW);
                //serverMainThread.IsBackground = true;
                //serverMainThread.Start();

                isConnected = true;
            }

            else
            {
                // ���� �ݱ�
                serverMainThread.Abort(); // ������ �����带 �ߴ��Ѵ�.

                isConnected = false;
            }
        }

        /// <summary>
        /// ���� ����, ��Ƽ ������� ������ �Ǿ�� �� (TcpClient ������ �� ��Ⱑ �ɸ��� ����)
        /// </summary>
        void ServerThread()
        {
            // try/catch��
            // �뵵 : Exception �߻��� �޽����� �������� Ȱ�� �� �� �ֵ��� ��.
            // �� ����� if-else���� ����ϴ�.
            try
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start(); // tcp ������ ������Ų��.

                // ������� �ٸ� �������� �ִ� �����͸� �� �� ���� ������ ����� �� ����
                //Text logText = Instantiate(messagePrefab, textArea);
                //logText.text = "���� ����";

                // ���� Data������ �ִ� ���� ������ ���
                log.Enqueue("���� ����");

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

                    log.Enqueue($"{handler.id}�� Ŭ���̾�Ʈ�� ���ӵ�.");
                }
            }
            catch (ArgumentException ex)
            {
                log.Enqueue("�Ķ���� ���� �߻�");
                log.Enqueue(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                log.Enqueue("�� ������ ���� �߻�");
                log.Enqueue(ex.Message);
            }
            catch (Exception ex) // try�� ���� �����߿� ������ �߻��� �� ȣ��
            {
                log.Enqueue("���� �߻�");
                log.Enqueue(ex.Message);
            }
            finally // try�� ������ ������ �߻��ص� ����ǰ�, �߻����� �ʾƵ� ����ȴ�.
                    // �߰��� �帧�� ������ �ʰ� ������ ��ü�� ���� �ϴ� ���� �ݵ�� �ʿ��� ������ ���⼭ �����ϰ� ��.
            {
                foreach (Thread thread in threads)
                {
                    thread?.Abort();
                }
            }
        }

        /// <summary>
        /// Ŭ���̾�Ʈ�� �������� �� ����Ʈ���� �ش� Ŭ���̾�Ʈ�� �����Ѵ�
        /// </summary>
        /// <param name="client">������ Ŭ���̾�Ʈ</param>
        public void Disconnect(ClientHandler client)
        {
            clientHandlers.Remove(client);
        }

        /// <summary>
        /// Ŭ���̾�Ʈ���� �޼����� �޾��� �� ������ �ٸ� ��� Ŭ���̾�Ʈ���� �޼����� �Ѹ��� �޼���
        /// </summary>
        /// <param name="message">�޼���</param>
        public void BroadcastToClients(string message)
        {
            log.Enqueue(message);

            foreach (ClientHandler client in clientHandlers)
            {
                client.MessageToClient(message);
            }
        }

        #region 08.05 ����
        /// <summary>
        /// ���� ������ ����
        /// </summary>
        void ServerThreadHW()
        {
            try
            {
                /*
                ������ ����
                ���� �����带 List�� �����Ͽ� ���� ������ ������ ������ ��������.
                */

                TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
                tcpListener.Start();

                log.Enqueue("���� ����");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    clients.Add(tcpClient);
                    log.Enqueue("Ŭ���̾�Ʈ �����");

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
        /// ������ Ŭ���̾�Ʈ�� ������
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

                log.Enqueue($"{index}�� �÷��̾��� ä�� : {readString}");
                foreach (TcpClient c in clients)
                {
                    StreamWriter clientWriter = new StreamWriter(c.GetStream());
                    clientWriter.WriteLine($"{index}�� �÷��̾��� ä�� : {readString}");
                    clientWriter.Flush();
                }
            }

            log.Enqueue("Ŭ���̾�Ʈ ���� ����");
        }
        #endregion
    }

    /// <summary>
    /// Ŭ���̾�Ʈ�� TCP ���� ��û�� �� ������ �ش� Ŭ���̾�Ʈ�� �ٵ�� �մ� ��ü�� �����Ѵ�.
    /// </summary>
    public class ClientHandler
    {
        public int id;
        public CServerManager server;
        public TcpClient tcpClient;
        public StreamReader reader;
        public StreamWriter writer;

        /// <summary>
        /// ������ Ŭ���̾�Ʈ�� ���ӽ�Ű�� ���� �ʱ�ȭ�� ����
        /// </summary>
        /// <param name="id">Ŭ���̾�Ʈ id</param>
        /// <param name="server">�����Ŵ���</param>
        /// <param name="tcpClient">Ŭ���̾�Ʈ</param>
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
        /// Ŭ���̾�Ʈ�� ������ ������ �� GC�� �޸𸮸� ������ �� �ֵ��� �ϱ� ���� �޼���
        /// </summary>
        public void Disconnect()
        {
            writer.Close();
            reader.Close();
            tcpClient.Close();
            server.Disconnect(this);
        }

        /// <summary>
        /// Ŭ���̾�Ʈ�� �޼����� ������.
        /// </summary>
        /// <param name="message">�޼���</param>
        public void MessageToClient(string message)
        {
            writer.WriteLine(message);
        }

        /// <summary>
        /// ������ �� �� ������ �޼���
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

                    // �о�� �޼����� ������ �������� ����
                    server.BroadcastToClients($"{id} ���� �� : {readString}");
                }
            }
            catch (Exception ex)
            {
                CServerManager.log.Enqueue($"{id}�� Ŭ���̾�Ʈ ���� �߻� : {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
