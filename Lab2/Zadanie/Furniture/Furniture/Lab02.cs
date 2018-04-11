using System;
using System.Collections;
using System.Collections.Generic;
namespace ASD
{

    public class CarpentersBench : MarshalByRefObject
    {
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
        /// 

        public int Cut((int length, int width) sheet, (int length, int width, int price)[] elements, out Cut cuts)
        {
            int[,] optimalValues = new int[sheet.length + 1, sheet.width + 1];
            ASD.Cut[,] optimalCuts = new ASD.Cut[sheet.length + 1, sheet.width + 1];

            foreach (var piece in elements)
            {
                if (piece.length <= sheet.length && piece.width <= sheet.width)
                    if (piece.price > optimalValues[piece.length, piece.width])
                    {
                        optimalValues[piece.length, piece.width] = piece.price;
                        optimalCuts[piece.length, piece.width] = new Cut(piece.length, piece.width, piece.price);
                    }
            }

            for (int length = 0; length < sheet.length + 1; length++)
            {
                for (int width = 0; width < sheet.width + 1; width++)
                {
                    if (optimalCuts[length, width] == null)
                        optimalCuts[length, width] = new Cut(length, width, 0);
                    int bestPrice = optimalValues[length, width];
                    ASD.Cut bestCut = optimalCuts[length, width];

                    for (int leftWidth = 1; leftWidth < width; leftWidth++)
                    {
                        int leftPrice = optimalValues[length, leftWidth];
                        int rightWidth = width - leftWidth;
                        int rightPrice = optimalValues[length, rightWidth];
                        int totalPrice = leftPrice + rightPrice;
                        if (totalPrice > bestPrice)
                        {
                            bestPrice = totalPrice;
                            optimalValues[length, width] = totalPrice;
                            optimalCuts[length, width] = new Cut(length, width, totalPrice, true, leftWidth, optimalCuts[length, leftWidth], optimalCuts[length, rightWidth]);
                        }
                    }

                    for (int topLength = 1; topLength < length; topLength++)
                    {
                        int topPrice = optimalValues[topLength, width];
                        int bottomLength = length - topLength;
                        int bottomPrice = optimalValues[bottomLength, width];
                        int totalPrice = topPrice + bottomPrice;
                        if (totalPrice > bestPrice)
                        {
                            bestPrice = totalPrice;
                            optimalValues[length, width] = totalPrice;
                            optimalCuts[length, width] = new Cut(length, width, totalPrice, false, topLength, optimalCuts[topLength, width], optimalCuts[bottomLength, width]);
                        }
                    }
                }
            }

            cuts = optimalCuts[sheet.length, sheet.width];
            return optimalValues[sheet.length, sheet.width];
        }
    }
}
