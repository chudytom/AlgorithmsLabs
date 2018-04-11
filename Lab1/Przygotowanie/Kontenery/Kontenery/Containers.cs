
using System;

namespace ASD
{

    public interface IContainer
    {
        void Put(int x);      //  dodaje element do kontenera

        int Get();           //  zwraca pierwszy element kontenera i usuwa go z kontenera
                             //  w przypadku pustego kontenera zg³asza wyj¹tek typu EmptyException (zdefiniowany w Lab01_Main.cs)

        int Peek();          //  zwraca pierwszy element kontenera (ten, który bêdzie pobrany jako pierwszy),
                             //  ale pozostawia go w kontenerze (czyli nie zmienia zawartoœci kontenera)
                             //  w przypadku pustego kontenera zg³asza wyj¹tek typu EmptyException (zdefiniowany w Lab01_Main.cs)

        int Count { get; }   //  zwraca liczbê elementów w kontenerze

        int Size { get; }   //  zwraca rozmiar kontenera (rozmiar wewnêtznej tablicy)
    }

    public class Stack : IContainer
    {
        private int[] tab;      // wewnêtrzna tablica do pamiêtania elementów
        private int count = 0;  // liczba elementów kontenera - metody Put i Get powinny (musz¹) to aktualizowaæ
                                // nie wolno dodawaæ ¿adnych pól ani innych sk³adowych

        public Stack(int n = 2)
        {
            tab = new int[n > 2 ? n : 2];
        }

        public void Put(int x)
        {
            if (count >= Size)
            {
                var newTab = new int[Size * 2];
                Array.Copy(tab, newTab, tab.Length);
                tab = newTab;
            }
            tab[count] = x;
            count++;
        }

        public int Get()
        {
            var item = Peek();
            tab[count - 1] = int.MinValue;
            count--;
            return item;
        }

        public int Peek()
        {
            if (count <= 0)
            {
                throw new EmptyException();
            }
            var item = tab[count - 1];
            return item;
        }

        public int Count => count;

        public int Size => tab.Length;

    } // class Stack


    public class Queue : IContainer
    {
        private int[] tab;      // wewnêtrzna tablica do pamiêtania elementów
        private int count = 0;  // liczba elementów kontenera - metody Put i Get powinny (musz¹) to aktualizowaæ
        int firstElementIndex = 0;   // mo¿na dodaæ jedno pole (wiêcej nie potrzeba)

        public Queue(int n = 2)
        {
            tab = new int[n > 2 ? n : 2];
        }

        public void Put(int x)
        {
            if (count >= Size)
            {
                var newTab = new int[Size * 2];
                for (int i = 0; i < (newTab.Length / 2); i++)
                {
                    newTab[i] = Get();
                }
                tab = newTab;
                tab[count] = x;
            }
            else
            {
                tab[GetFirstEmptyIndex()] = x;
            }
            count++;
        }

        public int Get()
        {
            var item = Peek();
            tab[firstElementIndex] = int.MinValue;
            if (firstElementIndex == Size - 1)
            {
                firstElementIndex = 0;
            }
            else
            {
                firstElementIndex++;
            }
            count--;
            return item;
        }

        public int Peek()
        {
            if (count <= 0)
            {
                throw new EmptyException();
            }
            return tab[firstElementIndex];
        }

        private int GetFirstEmptyIndex()
        {
            int sum = firstElementIndex + count;
            if (sum < Size)
            {
                return sum;
            }
            else
            {
                return sum - Size;
            }
        }

        public int Count => count;

        public int Size => tab.Length;

    } // class Queue


    public class LazyPriorityQueue : IContainer
    {
        private int[] tab;      // wewnêtrzna tablica do pamiêtania elementów
        private int count = 0;  // liczba elementów kontenera - metody Put i Get powinny (musz¹) to aktualizowaæ
                                // nie wolno dodawaæ ¿adnych pól ani innych sk³adowych

        public LazyPriorityQueue(int n = 2)
        {
            tab = new int[n > 2 ? n : 2];
        }

        public void Put(int x)
        {
            // uzupe³niæ
        }

        public int Get()
        {
            return 0; // zmieniæ
        }

        public int Peek()
        {
            return 0; // zmieniæ
        }

        public int Count => count;

        public int Size => tab.Length;

    } // class LazyPriorityQueue


    public class HeapPriorityQueue : IContainer
    {
        private int[] tab;      // wewnêtrzna tablica do pamiêtania elementów
        private int count = 0;  // liczba elementów kontenera - metody Put i Get powinny (musz¹) to aktualizowaæ
                                // nie wolno dodawaæ ¿adnych pól ani innych sk³adowych

        public HeapPriorityQueue(int n = 2)
        {
            tab = new int[n > 2 ? n : 2];
        }

        public void Put(int x)
        {
            if (count>=Size)
            {
                var newTab = new int[2 * Size];
                Array.Copy(tab, newTab, Size);
                tab = newTab;
            }
            tab[count] = x;
            var childIndex = count;
            var parentIndex = GetParentIndex(childIndex);
            while (parentIndex>=0 && tab[childIndex]> tab[parentIndex])
            {
                var temp = tab[parentIndex];
                tab[parentIndex] = tab[childIndex];
                tab[childIndex] = temp;
                childIndex = parentIndex;
                parentIndex = GetParentIndex(childIndex);
            }
            count++;
        }

        public int Get()
        {
            var item = Peek();
            tab[0] = tab[count - 1];
            tab[count - 1] = int.MinValue;
            count--;

            var parentIndex = 0;
            (var leftChildIndex, var rightChildIndex) = (int.MinValue, int.MinValue);
            do
            {
                (leftChildIndex, rightChildIndex) = GetChildrenIndices(parentIndex);

            }
            while (tab[leftChildIndex] > tab[parentIndex] || tab[rightChildIndex] > tab[parentIndex]);
            
        }

        public int Peek()
        {
            return tab[0];
        }

        public int Count => count;

        public int Size => tab.Length;

        private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

        private (int, int) GetChildrenIndices(int parentIndex) => (2 * parentIndex + 1, 2 * parentIndex + 2);

    } // class HeapPriorityQueue

} // namespace ASD
