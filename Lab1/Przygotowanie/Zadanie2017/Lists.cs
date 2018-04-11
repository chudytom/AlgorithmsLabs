
using System.Collections.Generic;

namespace ASD
{
    public interface IList : IEnumerable<int>
    {
        // Jeœli element v jest na liœcie to zwraca true
        // Jeœli elementu v nie ma na liœcie to zwraca false
        bool Search(int v);

        // Jeœli element v jest na liœcie to zwraca false (elementu nie dodaje)
        // Jeœli elementu v nie ma na liœcie to dodaje go do listy i zwraca true
        bool Add(int v);

        // Jeœli element v jest na liœcie to usuwa go z listy i zwraca true
        // Jeœli elementu v nie ma na liœcie to zwraca false
        bool Remove(int v);

    }

    //
    // dopisaæ klasê opisuj¹c¹ element listy
    //


    // Zwyk³a lista (nie samoorganizuj¹ca siê)
    public class SimpleList : IList
    {
        // dodaæ niezbêdne pola
        private Node head;
        private Node tail;
        private Node currentElement;
        // Lista siê nie zmienia
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

            return false;  // zmieniæ
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

            return true;  // zmieniæ
        }

        // Pozosta³e elementy nie zmieniaj¹ kolejnoœci
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
            // nie wolno modyfikowaæ kolekcji
            return new ListEnumerator(head);
        }

        // Wymagane przez interfejs IEnumerable<int> - nie zmmieniaæ (jest gotowe!)
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    } // class SimpleList


    // Lista z przesnoszeniem elementu, do którego by³ dostêp na pocz¹tek
    public class MoveToFrontList : IList
    {

        // dodaæ niezbêdne pola
        private Node head;
        private Node tail;
        private Node currentElement;


        // Znaleziony element jest przenoszony na pocz¹tek
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

        // Element jest dodawany na pocz¹tku, a jeœli ju¿ by³ na liœcie to jest przenoszony na pocz¹tek
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

        // Pozosta³e elementy nie zmieniaj¹ kolejnoœci
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

        // Wymagane przez interfejs IEnumerable<int> - nie zmmieniaæ (jest gotowe!)
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    } // class MoveToFrontList


} // namespace ASD
