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
        // async�� ��Ʈ��ũ�� �񵿱������ ������ ���� ���⶧���� �̸� �˾Ƶ־��Ѵ�.
        //async void Start()
        //{
        //    // void async�� �Լ� ��ü�� ��� ����������, �ٸ� �Լ����� ��� ���� �������� ȣ���� �Ұ����ϴ�.
        //    //await WaitRandom();
        //    WaitRandom();
        //    print("WaitRandom ȣ����");

        //    // Task async�� �Լ� ��ü�� ��� �����̸�, �ٸ� ��� ���� �Լ����� ��� �������� ȣ���� �����ϴ�.
        //    // return�� ��� ���μ����� Task�� ���� ��ȯ�Ѵ�.
        //    await Wait3Seconds();
        //    print("Wait3Seconds ȣ����");
        
        //    // Task<T> async�� ��� ���� �Լ��ΰ� Task�� ��ȯ�ϴ� �Լ��� ������, T return�� �־�߸� �Ѵ�.
        //    int delay = await WaitRandomAndReturn();
        //    print($"{delay}�� WaitRandomAndReturn ȣ����");
        //}

        void Start()
        {
            // Start�� async�� �ƴѵ��� Task�� �����Ŀ� ���𰡸� �ؾ� �� ���
            Wait3Seconds().ContinueWith(result => 
            { 
                if (result.IsCanceled || result.IsFaulted)
                {
                    print("Task ����");
                }

                else if (result.IsCompleted)
                {
                    print("Task ����");
                }

                print("3����");
            });
        }

        /// <summary>
        /// 1. void�� ��ȯ�ϴ� async �Լ�
        /// </summary>
        async void WaitRandom()
        {
            print($"��� ���� {Time.time}");
            await Task.Delay(Random.Range(1000, 2000));
            print($"��� ���� {Time.time}");
        }

        /// <summary>
        /// 2. Task�� ��ȯ�ϴ� async �Լ�
        /// </summary>
        /// <returns></returns>
        async Task Wait3Seconds()
        {
            print($"3�� ��� ���� {Time.time}");
            await Task.Delay(3000);
            print($"3�� ��� ���� {Time.time}");
        }

        /// <summary>
        /// 3. Task<T>�� ��ȯ�ϴ� async �Լ�
        /// </summary>
        /// <returns></returns>
        async Task<int> WaitRandomAndReturn()
        {
            int delay = Random.Range(1000, 2000);

            print($"{(float)delay / 1000} �� ��� ���� {Time.time}");
            await Task.Delay(delay);
            print($"{(float)delay / 1000} �� ��� ���� {Time.time}");

            return delay;
        }

        async Task WaitAndCallback(Action callback)
        {
            await Task.Delay(1000);
            callback?.Invoke();
        }
    }
}
