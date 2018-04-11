using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD
{
    public class ListEnumerator : IEnumerator<int>
    {
        private Node head;
        private Node currentElement;

        public ListEnumerator(Node head)
        {
            this.head = head;
            //this.currentElement = currentElement;
        }

        public int Current => currentElement.Value;

        object IEnumerator.Current => currentElement;

        public void Dispose()
        {
            return;
        }

        public bool MoveNext()
        {
            if (currentElement == null)
            {
                currentElement = head;
                if (currentElement == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //if (currentElement.Next == null)
            //{
            //    return false;
            //}
            currentElement = currentElement.Next;
            return currentElement != null;
        }

        public void Reset()
        {
            currentElement = head;
        }
    }
}
