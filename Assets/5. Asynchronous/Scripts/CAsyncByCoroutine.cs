using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class CAsyncByCoroutine : MonoBehaviour
    {
        #region public ����
        public Text text;
        #endregion

        #region private ����
        int bread = 0;
        int patty = 0;
        int pickle = 0;
        int lettuce = 0;

        FoodMakerThread breadMaker = new FoodMakerThread();
        FoodMakerThread pattyMaker = new FoodMakerThread();
        FoodMakerThread pickleMaker = new FoodMakerThread();
        FoodMakerThread lettuceMaker = new FoodMakerThread();
        #endregion

        void Start()
        {
            breadMaker.StartCook();
            pattyMaker.StartCook();
            pickleMaker.StartCook();
            lettuceMaker.StartCook();

            StartCoroutine(CheckHamberger());
        }

        void Update()
        {
            bread = breadMaker.amount;
            patty = pattyMaker.amount;
            pickle = pickleMaker.amount;
            lettuce = lettuceMaker.amount;

            text.text = $"�� ���� : {bread}, ��Ƽ ���� : {patty}, ��Ŭ ���� : {pickle}, ����� ���� : {lettuce}";
        }

        /// <summary>
        /// �ܹ��� �غ� �Ϸ����� �Ǵ�
        /// </summary>
        /// <returns></returns>
        bool HambergerReady()
        {
            return bread >= 2 && patty >= 2 && pickle >= 8 && lettuce >= 4;
        }

        /// <summary>
        /// �ܹ��Ÿ� ����� �ҿ�ð��� print �Ѵ�.
        /// </summary>
        void MakeHamberger()
        {
            //bread -= 2;
            //patty -= 2;
            //pickle -= 8;
            //lettuce -= 4;

            breadMaker.amount -= 2;
            pattyMaker.amount -= 2;
            pickleMaker.amount -= 8;
            lettuceMaker.amount -= 4;

            bread = breadMaker.amount;
            patty = pattyMaker.amount;
            pickle = pickleMaker.amount;
            lettuce = lettuceMaker.amount;

            print($"�ܹ��Ű� ����� �����ϴ�. �ҿ�ð� : {Time.time}");
        }

        /// <summary>
        /// �ܹ��Ÿ� ���� �� �ִ��� Ȯ���ϴ� �ڷ�ƾ; �񵿽����� �۵��ϰ� �ȴ�.
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckHamberger()
        {
            while (true)
            {
                // ������ ������ ������ �ƹ��͵� ���ϵ��� null�� ��ȯ
                yield return new WaitUntil(HambergerReady);
                MakeHamberger();
            }
        }
    }


    public class FoodMakerThread
    {
        #region public ����
        public int amount;  // ����� ��
        #endregion

        #region private ����
        private System.Random rand = new System.Random();
        #endregion

        /// <summary>
        /// ����Ḧ ����� Thread�� �����Ѵ�.
        /// </summary>
        public void StartCook()
        {
            Thread cookThread = new Thread(Cook);
            cookThread.IsBackground = true;
            cookThread.Start();
        }

        /// <summary>
        /// ����Ḧ ����� Thread.
        /// </summary>
        void Cook()
        {
            while (true)
            {
                int time = rand.Next(1000, 3000);
                // Thread�� ����ڰ� ���� ���̱� ������ ����Ƽ ���� Random�� ����� �� ����.
                //int times = Random.Range(1000, 3000);

                Thread.Sleep(time);

                amount++;
            }
        }
    }
}
