using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class CAsyncByTask : MonoBehaviour
    {
        #region public ����
        public Text text;
        #endregion

        #region private ����
        int bread = 0;
        int patty = 0;
        int pickle = 0;
        int lettuce = 0;

        FoodMakerTask breadMaker = new FoodMakerTask();
        FoodMakerTask pattyMaker = new FoodMakerTask();
        FoodMakerTask pickleMaker = new FoodMakerTask();
        FoodMakerTask lettuceMaker = new FoodMakerTask();
        #endregion

        void Start()
        {
            breadMaker.StartCook(2);
            pattyMaker.StartCook(2);
            pickleMaker.StartCook(8);
            lettuceMaker.StartCook(4);

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

    public class FoodMakerTask
    {
        #region public ����
        public int amount = 0;
        #endregion

        /// <summary>
        /// ����Ḧ ����� Task�� �����Ѵ�.
        /// </summary>
        public void StartCook(int count)
        {
            Task<int> cookTask = Cook(count);
            cookTask.ContinueWith(task => { amount = task.Result; });
        }

        /// <summary>
        /// ����Ḧ ����� Task
        /// </summary>
        /// <returns></returns>
        async Task<int> Cook(int count)
        {
            int result = 0;

            for (int i = 0; i < count; i++)
            {
                int time = Random.Range(1000, 3000);
                await Task.Delay(time);
                result++;
            }

            return result;
        }
    }
}
