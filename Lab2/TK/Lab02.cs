
using System.Collections.Generic;

namespace ASD
{

    public class CarpentersBench : System.MarshalByRefObject
    {
        /// <summary>
        /// Metoda pomocnicza - wymagana przez system
        /// </summary>
        public int Cut(int length, int width, int[,] elements, out Cut cuts)
        {
            (int length, int width, int price)[] _elements = new(int length, int width, int price)[elements.GetLength(0)];
            for (int i = 0; i < _elements.Length; ++i)
            {
                _elements[i].length = elements[i, 0];
                _elements[i].width = elements[i, 1];
                _elements[i].price = elements[i, 2];
            }
            return Cut((length, width), _elements, out cuts);
        }

        /// <summary>
        /// Wyznaczanie optymalnego sposobu pocięcia płyty
        /// </summary>
        /// <param name="sheet">Rozmiary płyty</param>
        /// <param name="elements">Tablica zawierająca informacje o wymiarach i wartości przydatnych elementów</param>
        /// <param name="cuts">Opis cięć prowadzących do uzyskania optymalnego rozwiązania</param>
        /// <returns>Maksymalna sumaryczna wartość wszystkich uzyskanych w wyniku cięcia elementów</returns>
        public int Cut((int length, int width) sheet, (int length, int width, int price)[] elements, out Cut cuts)
        {
            Cut[,] cutTab = new Cut[sheet.length + 1,sheet.width + 1];

            foreach (var elem in elements)
            {
                if (elem.length > sheet.length || elem.width > sheet.width)
                    continue;
                if (cutTab[elem.length, elem.width] is null || cutTab[elem.length, elem.width].price < elem.price)
                {
                    cutTab[elem.length, elem.width] = new Cut(elem.length, elem.width, elem.price);
                }
            }

            for (int i = 1; i <= sheet.length; i++)
            {
                for (int j = 1; j <= sheet.width; j++)
                {
                    Cut bestCut = (cutTab[i, j] is null) ? new Cut(i, j, 0) : cutTab[i, j];

                    for (int k = 1; k < i; k++)
                    {
                        int price1 = cutTab[k, j].price;
                        int price2 = cutTab[i - k, j].price;
                        if (price1 + price2 > bestCut.price)
                        {
                            bestCut = new Cut(i, j, price1 + price2, false, k, cutTab[k, j], cutTab[i - k, j]);
                        }
                    }
                    for (int k = 1; k < j; k++)
                    {
                        int price1 = cutTab[i, k].price;
                        int price2 = cutTab[i, j - k].price;
                        if (price1 + price2 > bestCut.price)
                        {
                            bestCut = new Cut(i, j, price1 + price2, true, k, cutTab[i, k], cutTab[i, j - k]);
                        }
                    }
                    cutTab[i, j] = bestCut;
                }
            }
            cuts = cutTab[sheet.length, sheet.width];
            return cutTab[sheet.length, sheet.width].price;
        }

    }

}
