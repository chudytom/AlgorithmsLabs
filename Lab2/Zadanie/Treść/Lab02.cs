
namespace ASD
{

public class CarpentersBench
    {

    /// <summary>
    /// Wyznaczanie optymalnego sposobu pocięcia płyty
    /// </summary>
    /// <param name="sheet">Rozmiary płyty</param>
    /// <param name="elements">Tablica zawierająca informacje o wymiarach i wartości przydatnych elementów</param>
    /// <param name="cuts">Opis cięć prowadzących do uzyskania optymalnego rozwiązania</param>
    /// <returns>Maksymalna sumaryczna wartość wszystkich uzyskanych w wyniku cięcia elementów</returns>
    public int Cut((int length, int width) sheet, (int length, int width, int price)[] elements, out Cut cuts)
        {
        cuts = null;  // zmienic w czesci B
        return -1;    // zmienic w czesci A
        }

    }

}
