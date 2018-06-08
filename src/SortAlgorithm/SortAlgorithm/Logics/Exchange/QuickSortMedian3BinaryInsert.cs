﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SortAlgorithm.Logics
{
    /// <summary>
    /// QuickSort + BinaryInsertSortによる Quick Searchでだいたいソート済みになった時に最速を目指すが、InsertSortの方がわずかに効率が良くBinarySearchのコストが目立つ
    /// </summary>
    /// <remarks>
    /// stable : no
    /// inplace : yes
    /// Compare :
    /// Swap :
    /// Order : O(n log n) (Worst case : O(n nlog n))
    /// ArraySize : 100, IsSorted : True, sortKind : QuickSortInsert, IndexAccessCount : 301, CompareCount : 286, SwapCount : 123
    /// ArraySize : 1000, IsSorted : True, sortKind : QuickSortInsert, IndexAccessCount : 6867, CompareCount : 7051, SwapCount : 1565
    /// ArraySize : 10000, IsSorted : True, sortKind : QuickSortInsert, IndexAccessCount : 87913, CompareCount : 90256, SwapCount : 23686
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class QuickSortMedian3BinaryInsert<T> : SortBase<T> where T : IComparable<T>
    {
        public override SortType SortType => SortType.Exchange;

        // ref : https://github.com/nlfiedler/burstsort4j/blob/master/src/org/burstsort4j/Introsort.java
        private const int InsertThreshold = 16;
        private BinaryInsertSort<T> insertSort = new BinaryInsertSort<T>();

        public override T[] Sort(T[] array)
        {
            base.Statics.Reset(array.Length, SortType, nameof(QuickSortMedian3BinaryInsert<T>));
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
            if (right - left < InsertThreshold)
            {
                return insertSort.Sort(array, left, right + 1);
            }

            // fase 1. decide pivot
            base.Statics.AddIndexAccess();
            var pivot = Median3(array[left], array[(left + (right - left)) / 2], array[right]);
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

        private T Median3(T low, T mid, T high)
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