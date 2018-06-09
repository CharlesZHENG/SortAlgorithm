﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortAlgorithm.Logics
{
    /// <summary>
    /// 基数の数だけバケットを用意して、基数の桁ごとにバケットソートを行うことで、バケットソートにあった値の範囲制限をゆるくしたもの。
    /// これにより、桁ごとにバケットを用意すれば良くなり対象が楽になる。
    /// </summary>
    /// <remarks>
    /// stable : yes
    /// inplace : no
    /// Compare : 2kn
    /// Swap : 0
    /// Order : O(kn)
    /// </remarks>
    /// <typeparam name="T"></typeparam>

    public class RadixLSD4Sort<T> : SortBase<T> where T : IComparable<T>
    {
        public override SortType SortType => SortType.Distributed;

        public int[] Sort(int[] array)
        {
            base.Statics.Reset(array.Length, SortType, nameof(RadixLSD4Sort<T>));
            var bucket = new List<int>[256];
            var digit = 4;

            for (int d = 0, logR = 0; d < digit; ++d, logR += 8)
            {
                // make bucket for possibly assigned number of int
                for (var i = 0; i < array.Length; i++)
                {
                    base.Statics.AddIndexAccess();
                    base.Statics.AddCompareCount();
                    // pick 256 radix d's digit number
                    var key = (array[i] >> logR) & 255;
                    if (bucket[key] == null) bucket[key] = new List<int>();
                    bucket[key].Add(array[i]);
                }

                // put array int to each bucket.
                for (int j = 0, i = 0; j < bucket.Length; ++j)
                {
                    if (bucket[j] != null)
                    {
                        foreach (var item in bucket[j])
                        {
                            base.Statics.AddIndexAccess();
                            array[i++] = item;
                        }
                    }
                    else
                    {
                        base.Statics.AddIndexAccess();
                    }
                }

                for (var j = 0; j < bucket.Length; ++j)
                {
                    base.Statics.AddIndexAccess();
                    bucket[j] = null;
                }
            }

            return array;
        }
    }

    public class RadixLSD10Sort<T> : SortBase<T> where T : IComparable<T>
    {
        public override SortType SortType => SortType.Distributed;

        public int[] Sort(int[] array)
        {
            base.Statics.Reset(array.Length, SortType, nameof(RadixLSD10Sort<T>));
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
            var digit = 1 + (int)array.Max(x => Math.Log10(x));

            var bucket = new List<int>[10];

            for (int d = 0, r = 1; d < digit; ++d, r *= 10)
            {
                // make bucket for possibly assigned number of int
                for (var i = 0; i < array.Length; i++)
                {
                    base.Statics.AddIndexAccess();
                    base.Statics.AddCompareCount();
                    var key = (array[i] / r) % 10;
                    if (bucket[key] == null) bucket[key] = new List<int>();
                    bucket[key].Add(array[i]);
                }

                // put array int to each bucket.
                for (int j = 0, i = 0; j < bucket.Length; ++j)
                {
                    if (bucket[j] != null)
                    {
                        foreach (var item in bucket[j])
                        {
                            base.Statics.AddIndexAccess();
                            array[i++] = item;
                        }
                    }
                    else
                    {
                        base.Statics.AddIndexAccess();
                    }
                }

                for (var j = 0; j < bucket.Length; ++j)
                {
                    base.Statics.AddIndexAccess();
                    bucket[j] = null;
                }
            }

            return array;
        }

        private int[] SortImplNegative(int[] array)
        {
            var bucket = new List<int>[20];
            // findmax
            var max = 0;
            int digit = 0;
            for (var i = 0; i < array.Length; i++)
            {
                base.Statics.AddIndexAccess();
                digit = GetDigit(array[i]);
                if (digit > max)
                {
                    max = digit;
                }
            }

            for (var r = 1; r <= max; r++)
            {
                for (var i = 0; i < array.Length; i++)
                {
                    base.Statics.AddIndexAccess();
                    var tmp = array[i];
                    var radix = tmp < 0
                        ? -(int)(Math.Abs(tmp) / Math.Pow(10, r - 1)) % 10
                        : (int)(tmp / Math.Pow(10, r - 1)) % 10;
                    radix += 9;
                    if (bucket[radix] == null) bucket[radix] = new List<int>();
                    bucket[radix].Add(tmp);
                }

                for (int j = 0, i = 0; j < bucket.Length; ++j)
                {
                    if (bucket[j] != null)
                    {
                        foreach (var item in bucket[j])
                        {
                            base.Statics.AddIndexAccess();
                            array[i++] = item;
                        }
                    }
                    else
                    {
                        base.Statics.AddIndexAccess();
                    }
                }

                for (var j = 0; j < bucket.Length; ++j)
                {
                    base.Statics.AddIndexAccess();
                    bucket[j] = null;
                }
            }

            return array;
        }

        private int GetDigit(int i)
        {
            return Math.Abs(i) < 10 ? 1 : 1 + GetDigit(i / 10);
        }
    }
}
