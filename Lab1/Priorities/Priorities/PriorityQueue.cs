
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{

    public interface IPriorityQueue
    {
        void Put(int p);     // wstawia element do kolejki
        int GetMax();        // pobiera maksymalny element z kolejki (element jest usuwany z kolejki)
        int ShowMax();       // pokazuje maksymalny element kolejki (element pozostaje w kolejce)
        int Count { get; }   // liczba elementów kolejki
    }


    public class LazyPriorityQueue : MarshalByRefObject, IPriorityQueue
    {
        List<int> list;
        public LazyPriorityQueue()
        {
            list = new List<int>();
        }

        public void Put(int p)
        {
            list.Add(p);
        }

        public int GetMax()
        {
            (int maxValue, int maxIndex) = GetMaxValueAndIndex();
            list.RemoveAt(maxIndex);

            return maxValue;
        }

        public int ShowMax()
        {
            return GetMaxValueAndIndex().Value;
        }

        private (int Value, int Index) GetMaxValueAndIndex()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Access to empty queue");
            }
            int maxIndex = 0;
            int maxValue = list[0];

            for (int i = 0; i < Count; i++)
            {
                if (list[i] > maxValue)
                {
                    maxIndex = i;
                    maxValue = list[i];
                }
            }

            return (Value: maxValue, Index: maxIndex);
        }

        public int Count => list.Count;

    } // LazyPriorityQueue


    public class EagerPriorityQueue : MarshalByRefObject, IPriorityQueue
    {
        List<int> list;

        public EagerPriorityQueue()
        {
            list = new List<int>();
        }

        public void Put(int p)
        {
            if (list.Count == 0)
            {
                list.Add(p);
            }
            else
            {
                list.Add(p);
                for (int i = Count - 1; i > 0; i--)
                {

                    if (list[i] < list[i - 1])
                    {
                        int temp = list[i];
                        list[i] = list[i - 1];
                        list[i - 1] = temp;
                    }
                }
            }
        }

        public int GetMax()
        {
            var result = ShowMax();
            list.RemoveAt(Count - 1);
            return result;
        }

        public int ShowMax()
        {
            if (Count <= 0)
            {
                throw new InvalidOperationException("Access to empty queue");
            }
            return list.Last();
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

    } // EagerPriorityQueue


    public class HeapPriorityQueue : MarshalByRefObject, IPriorityQueue
    {
        private int[] tab;
        private int count = 0;

        public HeapPriorityQueue()
        {
            tab = new int[2];
        }

        public void Put(int p)
        {
            if (count >= Size)
            {
                var newTab = new int[2 * Size];
                Array.Copy(tab, newTab, Size);
                tab = newTab;
            }
            tab[count] = p;
            var childIndex = count;
            var parentIndex = GetParentIndex(childIndex);
            while (parentIndex >= 0 && tab[childIndex] > tab[parentIndex])
            {
                var temp = tab[parentIndex];
                tab[parentIndex] = tab[childIndex];
                tab[childIndex] = temp;
                childIndex = parentIndex;
                parentIndex = GetParentIndex(childIndex);
            }
            count++;
        }

        public int GetMax()
        {
            var item = ShowMax();
            tab[0] = tab[count - 1];
            tab[count - 1] = int.MinValue;
            count--;

            var parentIndex = 0;
            List<int> childrenIndices;
            do
            {
                childrenIndices = GetChildrenIndices(parentIndex);
                if (childrenIndices[0] < count)
                {
                    var maxIndex = childrenIndices[0];
                    if (childrenIndices[1] < count)
                    {
                        if (tab[childrenIndices[1]] > tab[childrenIndices[0]])
                        {
                            maxIndex = childrenIndices[1];
                        }
                    }
                    if (tab[parentIndex] < tab[maxIndex])
                    {
                        var temp = tab[parentIndex];
                        tab[parentIndex] = tab[maxIndex];
                        tab[maxIndex] = temp;
                        parentIndex = maxIndex;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            while (childrenIndices[0] < count || childrenIndices[1] < count);

            return item;
        }

        public int ShowMax()
        {
            if (count <= 0)
            {
                throw new InvalidOperationException("Access to empty queue");
            }
            return tab[0];
        }

        private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

        private List<int> GetChildrenIndices(int parentIndex) => new List<int>() { 2 * parentIndex + 1, 2 * parentIndex + 2 };

        public int Count
        {
            get

            {
                return count;
            }
        }

        public int Size => tab.Length;

    } // HeapPriorityQueue

}
