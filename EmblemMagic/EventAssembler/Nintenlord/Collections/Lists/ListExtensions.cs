using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Collections.Lists
{
    public static class ListExtensions
    {
        public static void BubbleSort<T>(this IList<T> list) where T : IComparable<T>
        {
            list.BubbleSort<T>(Comparer<T>.Default);
        }

        public static void BubbleSort<T>(this IList<T> list, IComparer<T> comp)
        {
            bool isSorted = true;
            int length = list.Count;
            do
            {
                for (int i = 0; i < length - 1; i++)
                {
                    if (comp.Compare(list[i], list[i+1]) > 0)
                    {
                        Swap(list, i, i + 1);
                        isSorted = false;
                    }
                }
            } while (!isSorted);
        }


        public static void SelectionSort<T>(this IList<T> list) where T : IComparable<T>
        {
            list.SelectionSort(Comparer<T>.Default);
        }

        public static void SelectionSort<T>(this IList<T> list, IComparer<T> comp)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (comp.Compare(list[minIndex], list[j]) > 0)
                    {
                        minIndex = j;
                    }
                }
                if (minIndex != i)
                {
                    list.Swap(minIndex, i);
                }
            }
        }
        

        public static void InsertionSort<T>(this IList<T> list) where T : IComparable<T>
        {
            list.InsertionSort(Comparer<T>.Default);
        }

        public static void InsertionSort<T>(this IList<T> list, IComparer<T> comp)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (comp.Compare(list[j], list[i]) > 0)
                    {
                        T temp = list[i];
                        list.RemoveAt(i);
                        list.Insert(j, temp);
                        break;
                    }
                }
            }
        }


        public static void ShellSort<T>(this IList<T> list) where T : IComparable<T>
        {
            list.ShellSort(Comparer<T>.Default);
        }

        public static void ShellSort<T>(this IList<T> list, IComparer<T> comp)
        {
            int inc = list.Count / 2;
            while (inc > 0)
            {
                for (int i = inc; i < list.Count; i++)
                {
                    T temp = list[i];
                    int j = i; 
                    while (j < inc && comp.Compare(list[j - inc], temp) > 0)
                    {
                        list[j] = list[j - inc];
                        j -= inc;
                    }
                    list[j] = temp;
                }
                inc = (int)(inc / 2.2);
            }
        }

        
        public static void CombSort<T>(this IList<T> list) where T : IComparable<T>
        {
            list.CombSort(Comparer<T>.Default);
        }

        public static void CombSort<T>(this IList<T> list, IComparer<T> comp)
        {
            int gap = list.Count;
            bool swapped = true;
            while (gap > 1 || swapped)
            {
                if (gap > 1)
                    gap = (int)(gap / 1.247330950103979);

                swapped = false;
                for (int i = 0; i < list.Count - gap; i++)
                {
                    if (comp.Compare(list[i], list[i + gap]) > 0)
                    {
                        list.Swap(i, i + gap);
                        swapped = true;
                    }
                }
            }
        }


        public static void MergeSort<T>(this IList<T> list) where T : IComparable<T>
        {
            list.MergeSort(Comparer<T>.Default);
        }

        public static void MergeSort<T>(this IList<T> list, IComparer<T> comp)
        {
            if (list.Count > 1)
            {
                SubList<T> first = new SubList<T>(list, 0, list.Count / 2);
                SubList<T> second = new SubList<T>(list, first.Length, list.Count - first.Length);
                first.MergeSort(comp);
                second.MergeSort(comp);
                SubList<T>.SortedMerge(first, second, comp);
            }
        }
                
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static int LastIndexOf<T>(this IList<T> list, Predicate<T> predicate)
        {
            int result = list.Count;
            while (result > 0)
            {
                result--;
                if (predicate(list[result]))
                {
                    break;
                }
            }
            return result;
        }

        public static int IndexOf<T>(this IList<T> list, Predicate<T> predicate)
        {
            int result = 0;
            while (result < list.Count)
            {
                if (predicate(list[result]))
                {
                    break;
                }
                result++;
            }
            return result != list.Count ? result : -1;
        }
    }
}
