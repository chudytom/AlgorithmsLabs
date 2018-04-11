
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
    public class Elem
    {
        
    }


// Zwyk�a lista (nie samoorganizuj�ca si�)
public class SimpleList : IList
    {
    // doda� niezb�dne pola

    // Lista si� nie zmienia
    public bool Search(int v)
        {

        return false;  // zmieni�
        }

    // Element jest dodawany na koniec listy
    public bool Add(int v)
        {
            return true;  // zmieni�

        }

    // Pozosta�e elementy nie zmieniaj� kolejno�ci
    public bool Remove(int v)
        {
            return false;
        }

    // Wymagane przez interfejs IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
        {
            // nie wolno modyfikowa� kolekcji
        yield break;  // zmieni�
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

    // Znaleziony element jest przenoszony na pocz�tek
    public bool Search(int v)
        {
        return false;  // zmieni�
        }

    // Element jest dodawany na pocz�tku, a je�li ju� by� na li�cie to jest przenoszony na pocz�tek
    public bool Add(int v)
        {
        return false;  // zmieni�
        }

    // Pozosta�e elementy nie zmieniaj� kolejno�ci
    public bool Remove(int v)
        {
        return false;  // zmieni�
        }

    // Wymagane przez interfejs IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
        {
        // nie wolno modyfikowa� kolekcji
        yield break;  // zmieni�
        }

    // Wymagane przez interfejs IEnumerable<int> - nie zmmienia� (jest gotowe!)
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
        return this.GetEnumerator();
        }

    } // class MoveToFrontList


} // namespace ASD
