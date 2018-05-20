﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SortAlgorithm.Logics
{
    /// <summary>
    /// <see cref="BubbleSort{T}"/>の<see cref="ShellSort{T}"/>と同様の概念の導入版。配列を1.3で割り、小数点以下を切り捨てた数(h)の間隔ととる。先頭(i=0)から順にiとi+hで比較、小さい場合に入れ替え。i+h>nとなるまで栗加須。hが1の場合は交換が発生しなくなるまで繰り返す。Comb11の最適化によりhが9,10の場合は11とする。配列要素の入れ替えが生じるため不安定ソートとなる。<see cref="InsertSort{T}"/>に同様のロジックを適用したのが<see cref="ShellSort{T}"/>である。
    /// 単純だが低速
    /// </summary>
    /// <remarks>
    /// stable : no
    /// inplace : yes
    /// Compare : O(n log n)
    /// Swap : O(n log n)
    /// sortKind : CombSort, ArraySize : 100, IndexAccessCount : 1171, CompareCount : 1171, SwapCount : 258
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class CombSort<T> : SortBase<T> where T : IComparable<T>
    {
        public override T[] Sort(T[] array)
        {
            base.sortStatics.Reset(array.Length);

            // same logic as ShellSort, but CombSort use divide by 1.3.
            // divide by 1.3
            var h = array.Length * 10 / 13;

            while (true)
            {
                if (h != 1)
                {
                    h = h * 10 / 13;
                    // comb11
                    if (h == 9 || h == 10)
                    {
                        h = 11;
                    }
                }

                var swapped = false;
                for (var i = 0; i + h < array.Length; i++)
                {
                    base.sortStatics.AddIndexAccess();
                    base.sortStatics.AddCompareCount();
                    if (array[i].CompareTo(array[i + h]) > 0)
                    {
                        base.sortStatics.AddSwapCount();
                        Swap(ref array[i], ref array[i + h]);
                        swapped = true;
                    }
                }

                if (!swapped) break;
            }
            return array;
        }
    }
}
