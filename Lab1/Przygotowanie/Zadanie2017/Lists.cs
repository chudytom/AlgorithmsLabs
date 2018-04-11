
using System.Collections.Generic;

namespace ASD
{
    public interface IList : IEnumerable<int>
    {
        // Je�li element v jest na li�cie to zwraca true
        // Je�li elementu v nie ma na li�cie to zwraca false
        bool Search(int v);

        // Je�li element v jest na li�cie to zwraca false (elementu nie dodaje)
        // Je�li elementu v nie ma na li�cie to dodaje go do listy i zwraca true
        bool Add(int v);

        // Je�li element v jest na li�cie to usuwa go z listy i zwraca true
        // Je�li elementu v nie ma na li�cie to zwraca false
        bool Remove(int v);

    }

    //
    // dopisa� klas� opisuj�c� element listy
    //


    // Zwyk�a lista (nie samoorganizuj�ca si�)
    public class SimpleList : IList
    {
        // doda� niezb�dne pola
        private Node head;
        private Node tail;
        private Node currentElement;
        // Lista si� nie zmienia
        public bool Search(int v)
        {
            if (head == null)
            {
                return false;
            }

            currentElement = head;
            while (currentElement != null)
            {
                if (currentElement.Value == v)
                {
                    return true;
                }
                currentElement = currentElement.Next;
            }

            return false;  // zmieni�
        }

        // Element jest dodawany na koniec listy
        public bool Add(int v)
        {
            if (Search(v))
            {
                return false;
            }

            if (tail == null || head == null)
            {
                head = new Node() { Value = v, Next = null };
                tail = head;
            }
            else
            {
                tail.Next = new Node() { Value = v, Next = null };
                tail = tail.Next;
            }

            return true;  // zmieni�
        }

        // Pozosta�e elementy nie zmieniaj� kolejno�ci
        public bool Remove(int v)
        {
            if (!Search(v))
            {
                return false;
            }
            if (head.Value == v)
            {
                head = head.Next;
                if (head == null)
                    tail = null;
                return true;
            }
            currentElement = head;
            while (currentElement != null)
            {
                if (currentElement.Next != null && currentElement.Next.Value == v)
                {
                    var nodeToRemove = currentElement.Next;
                    if (nodeToRemove.Next == null)
                    {
                        currentElement.Next = null;
                        tail = currentElement;
                    }
                    else
                    {
                        currentElement.Next = nodeToRemove.Next;
                    }

                    return true;
                }
                currentElement = currentElement.Next;
            }
            return false;
        }

        // Wymagane przez interfejs IEnumerable<int>
        public IEnumerator<int> GetEnumerator()
        {
            // nie wolno modyfikowa� kolekcji
            return new ListEnumerator(head);
        }

        // Wymagane przez interfejs IEnumerable<int> - nie zmmienia� (jest gotowe!)
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    } // class SimpleList


    // Lista z przesnoszeniem elementu, do kt�rego by� dost�p na pocz�tek
    public class MoveToFrontList : IList
    {

        // doda� niezb�dne pola
        private Node head;
        private Node tail;
        private Node currentElement;


        // Znaleziony element jest przenoszony na pocz�tek
        public bool Search(int v)
        {
            if (head ==null)
            {
                return false;
            }
            currentElement = head;
            if (head.Value == v)
            {
                return true;
            }
            while (currentElement != null)
            {
                if (currentElement.Next !=null &&  currentElement.Next.Value == v)
                {
                    var nodeToMove = currentElement.Next;
                    currentElement.Next = nodeToMove.Next;
                    if (currentElement.Next == null)
                    {
                        tail = currentElement;
                    }

                    nodeToMove.Next = head;
                    head = nodeToMove;

                    return true;
                }
                currentElement = currentElement.Next;
            }
            return false;
        }

        // Element jest dodawany na pocz�tku, a je�li ju� by� na li�cie to jest przenoszony na pocz�tek
        public bool Add(int v)
        {
            if (Search(v))
            {
                return false;
            }

            else
            {
                var nodeToAdd = new Node() { Value = v, Next = head };
                head = nodeToAdd;
                return true;
            }
        }

        // Pozosta�e elementy nie zmieniaj� kolejno�ci
        public bool Remove(int v)
        {
            if (!Search(v))
            {
                return false;
            }
            else
            {
                head = head.Next;
                return true;
            }
        }

        // Wymagane przez interfejs IEnumerable<int>
        public IEnumerator<int> GetEnumerator()
        {
            return new ListEnumerator(head);
        }

        // Wymagane przez interfejs IEnumerable<int> - nie zmmienia� (jest gotowe!)
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    } // class MoveToFrontList


} // namespace ASD
