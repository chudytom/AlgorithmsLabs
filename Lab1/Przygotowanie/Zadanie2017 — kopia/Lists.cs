
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
    public class Elem
    {
        
    }


// Zwyk³a lista (nie samoorganizuj¹ca siê)
public class SimpleList : IList
    {
    // dodaæ niezbêdne pola

    // Lista siê nie zmienia
    public bool Search(int v)
        {

        return false;  // zmieniæ
        }

    // Element jest dodawany na koniec listy
    public bool Add(int v)
        {
            return true;  // zmieniæ

        }

    // Pozosta³e elementy nie zmieniaj¹ kolejnoœci
    public bool Remove(int v)
        {
            return false;
        }

    // Wymagane przez interfejs IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
        {
            // nie wolno modyfikowaæ kolekcji
        yield break;  // zmieniæ
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

    // Znaleziony element jest przenoszony na pocz¹tek
    public bool Search(int v)
        {
        return false;  // zmieniæ
        }

    // Element jest dodawany na pocz¹tku, a jeœli ju¿ by³ na liœcie to jest przenoszony na pocz¹tek
    public bool Add(int v)
        {
        return false;  // zmieniæ
        }

    // Pozosta³e elementy nie zmieniaj¹ kolejnoœci
    public bool Remove(int v)
        {
        return false;  // zmieniæ
        }

    // Wymagane przez interfejs IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
        {
        // nie wolno modyfikowaæ kolekcji
        yield break;  // zmieniæ
        }

    // Wymagane przez interfejs IEnumerable<int> - nie zmmieniaæ (jest gotowe!)
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
        return this.GetEnumerator();
        }

    } // class MoveToFrontList


} // namespace ASD
