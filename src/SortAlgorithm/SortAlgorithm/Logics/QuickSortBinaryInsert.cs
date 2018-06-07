﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SortAlgorithm.Logics
{
    /// <summary>
    /// QuickSort + InsertSortによる IntroSort(HeapSortが入っていないので微妙)。閾値以下の要素になった時にInsertSortに切り替わることでワーストケースをつぶす。
    /// </summary>
    /// <remarks>
    /// stable : no
    /// inplace : yes
    /// Compare : 
    /// Swap : 
    /// Order : O(n log n) (Worst case : O(n nlog n))
    /// ArraySize : 100, IsSorted : True, sortKind : QuickSortInsert, IndexAccessCount : 304, CompareCount : 315, SwapCount : 105
    /// ArraySize : 1000, IsSorted : True, sortKind : QuickSortInsert, IndexAccessCount : 5707, CompareCount : 5953, SwapCount : 1620
    /// ArraySize : 10000, IsSorted : True, sortKind : QuickSortInsert, IndexAccessCount : 83644, CompareCount : 86039, SwapCount : 23737
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class QuickSortBinaryInsert<T> : SortBase<T> where T : IComparable<T>
    {
        // ref : https://github.com/nlfiedler/burstsort4j/blob/master/src/org/burstsort4j/Introsort.java
        private const int IntroThreshold = 16;
        private BinaryInsertSort<T> insertSort = new BinaryInsertSort<T>();

        public override T[] Sort(T[] array)
        {
            base.Statics.Reset(array.Length);
            var result = Sort(array, 0, array.Length - 1);
            base.Statics.AddCompareCount(insertSort.Statics.CompareCount);
            base.Statics.AddIndexAccess(insertSort.Statics.IndexAccessCount);
            base.Statics.AddSwapCount(insertSort.Statics.SwapCount);
            return result;
        }

        private T[] Sort(T[] array, int left, int right)
        {
            if (left >= right) return array;

            // switch to insert sort
            if (right - left < IntroThreshold)
            {
                return insertSort.Sort(array, left, right + 1);
            }

            // fase 1. decide pivot
            base.Statics.AddIndexAccess();
            var pivot = Median(array[left], array[(left + (right - left)) / 2], array[right]);
            var l = left;
            var r = right;

            while (l <= r)
            {
                while (l < right && array[l].CompareTo(pivot) < 0)
                {
                    base.Statics.AddIndexAccess();
                    base.Statics.AddCompareCount();
                    l++;
                }

                while (r > left && array[r].CompareTo(pivot) > 0)
                {
                    base.Statics.AddIndexAccess();
                    base.Statics.AddCompareCount();
                    r--;
                }

                if (l > r) break;
                Swap(ref array[l], ref array[r]);
                l++;
                r--;
            }

            // fase 2. Sort Left and Right
            Sort(array, left, l - 1);
            Sort(array, l, right);
            return array;
        }

        private T Median(T low, T mid, T high)
        {
            base.Statics.AddCompareCount();
            if (low.CompareTo(mid) > 0)
            {
                base.Statics.AddCompareCount();
                if (mid.CompareTo(high) > 0)
                {
                    return mid;
                }
                else
                {
                    base.Statics.AddCompareCount();
                    return low.CompareTo(high) > 0 ? high : low;
                }
            }
            else
            {
                base.Statics.AddCompareCount();
                if (mid.CompareTo(high) > 0)
                {
                    base.Statics.AddCompareCount();
                    return low.CompareTo(high) > 0 ? low : high;
                }
                else
                {
                    return mid;
                }
            }
        }
    }
}
