using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class CAsyncByTask : MonoBehaviour
    {
        #region public 변수
        public Text text;
        #endregion

        #region private 변수
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

            text.text = $"빵 개수 : {bread}, 패티 개수 : {patty}, 피클 개수 : {pickle}, 양상추 개수 : {lettuce}";
        }

        /// <summary>
        /// 햄버거 준비 완료인지 판단
        /// </summary>
        /// <returns></returns>
        bool HambergerReady()
        {
            return bread >= 2 && patty >= 2 && pickle >= 8 && lettuce >= 4;
        }

        /// <summary>
        /// 햄버거를 만들고 소요시간을 print 한다.
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

            print($"햄버거가 만들어 졌습니다. 소요시간 : {Time.time}");
        }

        /// <summary>
        /// 햄버거를 만들 수 있는지 확인하는 코루틴; 비동식으로 작동하게 된다.
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckHamberger()
        {
            while (true)
            {
                // 조건이 만족될 때까지 아무것도 안하도록 null을 반환
                yield return new WaitUntil(HambergerReady);
                MakeHamberger();
            }
        }
    }

    public class FoodMakerTask
    {
        #region public 변수
        public int amount = 0;
        #endregion

        /// <summary>
        /// 식재료를 만드는 Task를 시작한다.
        /// </summary>
        public void StartCook(int count)
        {
            Task<int> cookTask = Cook(count);
            cookTask.ContinueWith(task => { amount = task.Result; });
        }

        /// <summary>
        /// 식재료를 만드는 Task
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
