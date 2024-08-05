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
        List<TcpClient> clients = new List<TcpClient>();

        bool isConnected = false;
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
                //serverMainThread = new Thread(ServerThread);
                //serverMainThread.IsBackground = true;
                //serverMainThread.Start();

                // ���� ���� ����
                serverMainThread = new Thread(ServerThreadHW);
                serverMainThread.IsBackground = true;
                serverMainThread.Start();

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

            /*
            ������ ����
            ���� �����带 List�� �����Ͽ� ���� ������ ������ ������ ��������.
            */

            TcpListener tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
            tcpListener.Start(); // tcp ������ ������Ų��.

            // ������� �ٸ� �������� �ִ� �����͸� �� �� ���� ������ ����� �� ����
            //Text logText = Instantiate(messagePrefab, textArea);
            //logText.text = "���� ����";

            // ���� Data������ �ִ� ���� ������ ���
            log.Enqueue("���� ����");

            TcpClient tcpClient = tcpListener.AcceptTcpClient(); // return�� �� ������ ��Ⱑ �ɸ���.

            //Text logText2 = Instantiate(messagePrefab, textArea);
            //logText2.text = "Ŭ���̾�Ʈ �����";
            log.Enqueue("Ŭ���̾�Ʈ �����");

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

                // ���� �޼����� �״�� writer�� ����.
                writer.WriteLine($"����� �޼��� : {readString}");

                log.Enqueue($"client message : {readString}");
            }

            log.Enqueue("Ŭ���̾�Ʈ ���� ����");
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

                log.Enqueue($"client message : {readString}");
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
}
