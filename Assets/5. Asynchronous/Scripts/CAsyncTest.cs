using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyProject
{
    public class CAsyncTest : MonoBehaviour
    {
        // async는 네트워크에 비동기식으로 구현된 것이 많기때문에 미리 알아둬야한다.
        //async void Start()
        //{
        //    // void async는 함수 자체는 대기 가능하지만, 다른 함수에서 대기 가능 형식으로 호출이 불가능하다.
        //    //await WaitRandom();
        //    WaitRandom();
        //    print("WaitRandom 호출함");

        //    // Task async는 함수 자체도 대기 가능이며, 다른 대기 가능 함수에서 대기 형식으로 호출이 가능하다.
        //    // return이 없어도 프로세스를 Task로 묶어 반환한다.
        //    await Wait3Seconds();
        //    print("Wait3Seconds 호출함");
        
        //    // Task<T> async는 대기 가능 함수인건 Task를 반환하는 함수와 같으나, T return이 있어야만 한다.
        //    int delay = await WaitRandomAndReturn();
        //    print($"{delay}초 WaitRandomAndReturn 호출함");
        //}

        void Start()
        {
            // Start가 async가 아닌데도 Task가 끝난후에 무언가를 해야 할 경우
            Wait3Seconds().ContinueWith(result => 
            { 
                if (result.IsCanceled || result.IsFaulted)
                {
                    print("Task 실패");
                }

                else if (result.IsCompleted)
                {
                    print("Task 성공");
                }

                print("3초후");
            });
        }

        /// <summary>
        /// 1. void를 반환하는 async 함수
        /// </summary>
        async void WaitRandom()
        {
            print($"대기 시작 {Time.time}");
            await Task.Delay(Random.Range(1000, 2000));
            print($"대기 종료 {Time.time}");
        }

        /// <summary>
        /// 2. Task를 반환하는 async 함수
        /// </summary>
        /// <returns></returns>
        async Task Wait3Seconds()
        {
            print($"3초 대기 시작 {Time.time}");
            await Task.Delay(3000);
            print($"3초 대기 종료 {Time.time}");
        }

        /// <summary>
        /// 3. Task<T>를 반환하는 async 함수
        /// </summary>
        /// <returns></returns>
        async Task<int> WaitRandomAndReturn()
        {
            int delay = Random.Range(1000, 2000);

            print($"{(float)delay / 1000} 초 대기 시작 {Time.time}");
            await Task.Delay(delay);
            print($"{(float)delay / 1000} 초 대기 종료 {Time.time}");

            return delay;
        }

        async Task WaitAndCallback(Action callback)
        {
            await Task.Delay(1000);
            callback?.Invoke();
        }
    }
}
