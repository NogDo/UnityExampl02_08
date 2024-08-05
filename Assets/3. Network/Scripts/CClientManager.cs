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
        #region static ����
        public static Queue<string> log = new Queue<string>();
        #endregion

        #region public ����
        public Text messagePrefab;
        public RectTransform textArea;

        public Button connectButton;
        public InputField ip;
        public InputField port;

        public InputField messageInput;
        public Button sendButton;
        #endregion

        #region private ����
        Thread clientThread;
        StreamReader reader;
        StreamWriter writer;

        bool isConneted = false;
        #endregion

        void Awake()
        {
            // �ڵ�� Action���
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
        /// ���� ��ư Ŭ��
        /// </summary>
        public void ConnectButtonClick()
        {
            if (false == isConneted)
            {
                // ������ ���� �õ�
                clientThread = new Thread(ClientThread);
                clientThread.IsBackground = true;
                clientThread.Start();

                isConneted = false;
            }

            else
            {
                // ���� ����
                clientThread.Abort();

                isConneted = true;
            }
        }

        /// <summary>
        /// ������ ����
        /// </summary>
        void ClientThread()
        {
            TcpClient tcpClient = new TcpClient();

            IPAddress serverAddress = IPAddress.Parse(ip.text);
            int portNum = int.Parse(port.text);

            IPEndPoint endPoint = new IPEndPoint(serverAddress, portNum);

            tcpClient.Connect(endPoint);

            log.Enqueue($"������ ���ӵ�. IP : {endPoint.Address}");

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

            log.Enqueue("���� ����");
        }

        /// <summary>
        /// ������ �޼����� ������. (inputField�� OnSubmit���� ȣ��)
        /// </summary>
        /// <param name="message">���� �޼���</param>
        public void MessageToServer(string message)
        {
            writer.WriteLine(message);

            messageInput.text = "";
        }
    }
}
