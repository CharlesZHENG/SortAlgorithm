﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortAlgorithm.Logics
{
    /// <summary>
    /// 値の分布状況を数え上げることを利用してインデックスを導きソートする。バケットソート同様に、とりえる値の範囲を知っていないといけないことと、カウントと結果用の配列分メモリを食う。
    /// </summary>
    /// <remarks>
    /// stable : yes
    /// inplace : no
    /// Compare : 0
    /// Swap : 0
    /// Order : O(n + k) (k = helper array = counting + result)
    /// </remarks>
    /// <typeparam name="T"></typeparam>

    public class CountingSort<T> : SortBase<T> where T : IComparable<T>
    {
        public override SortType SortType => SortType.Distributed;

        public int[] Sort(int[] array)
        {
            base.Statistics.Reset(array.Length, SortType, nameof(CountingSort<T>));
            if (array.Min() >= 0)
            {
                return SortImplPositive(array);
            }
            else
            {
                return SortImplNegative(array);
            }
        }

        private int[] SortImplPositive(int[] array)
        {
            var min = 0;
            var max = 0;

            for (var i = 1; i < array.Length; i++)
            {
                base.Statistics.AddIndexAccess();
                if (array[i] < min)
                {
                    min = array[i];
                }
                else if (array[i] > max)
                {
                    max = array[i];
                }
            }

            var resultArray = new int[array.Length];
            var countArray = new int[max - min + 1 + 1];

            // count up each number of element to countArray
            for (var i = 0; i < array.Length; i++)
            {
                base.Statistics.AddIndexAccess();
                ++countArray[array[i]];
            }

            // change current index element counter by adding previous index counter.
            for (var i = 1; i < countArray.Length; i++)
            {
                base.Statistics.AddIndexAccess();
                countArray[i] += countArray[i - 1];
            }

            // set countArrayed index element into resultArray, then decrement countArray.
            for (var i = 0; i < array.Length; i++)
            {
                base.Statistics.AddIndexAccess();
                resultArray[countArray[array[i]] - 1] = array[i];
                --countArray[array[i]];
            }

            return resultArray;
        }
        private int[] SortImplNegative(int[] array)
        {
            var max = -1;
            for (var i = 0; i < array.Length; i++)
            {
                base.Statistics.AddIndexAccess();
                if (Math.Abs(array[i]) > max)
                {
                    base.Statistics.AddIndexAccess();
                    max = Math.Abs(array[i]);
                }
            }
            var stack = new int[max * 2 + 1];

            for (var i = 0; i < array.Length; i++)
            {
                base.Statistics.AddIndexAccess();
                stack[array[i] + max]++;
            }

            var j = stack.Length - 1;
            var k = array.Length - 1;
            while (k >= 0)
            {
                base.Statistics.AddIndexAccess();
                if (stack[j] > 0)
                {
                    base.Statistics.AddIndexAccess();
                    stack[j]--;
                    array[k] = j - max;
                    k--;
                }
                else
                {
                    j--;
                }
            }
            return array;
        }
    }
}
