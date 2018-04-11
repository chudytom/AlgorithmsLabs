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
        private (int price, ASD.Cut cut)[,] optimalValues;

        public int Cut((int length, int width) sheet, (int length, int width, int price)[] elements, out Cut cuts)
        {
            optimalValues = new(int, ASD.Cut)[sheet.length + 1, sheet.width + 1];
            for (int i = 0; i < optimalValues.GetLength(0); i++)
            {
                for (int j = 0; j < optimalValues.GetLength(1); j++)
                {
                    optimalValues[i, j].price = -1;
                }
            }
            return CutDynamic(sheet, elements, out cuts);
        }

        public int CutDynamic((int length, int width) sheet, (int length, int width, int price)[] elements, out Cut cuts)
        {
            if (optimalValues == null)
            {
                optimalValues = new(int, ASD.Cut)[sheet.length + 1, sheet.width + 1];
                for (int i = 0; i < optimalValues.GetLength(0); i++)
                {
                    for (int j = 0; j < optimalValues.GetLength(1); j++)
                    {
                        optimalValues[i, j].price = -1;
                    }
                }
            }

            cuts = new ASD.Cut(sheet.length, sheet.width, 0);  // zmienic w czesci B
            if (ContainsSheet(optimalValues, sheet))
            {
                int price = 0;
                (price, cuts) = GetValues(optimalValues, sheet);
                return price;
            }
            else
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    if (sheet.length >= elements[i].length && sheet.width >= elements[i].width)
                    {
                        if (sheet.length == elements[i].length && sheet.width == elements[i].width)
                        {
                            cuts.price = elements[i].price;
                            if (GetValues(optimalValues, sheet).price > 0)
                            {
                                if (elements[i].price > GetValues(optimalValues, sheet).price)
                                {
                                    //optimalValues.SetValues((elements[i].length, elements[i].width), (elements[i].price, cuts));
                                    SetValues(optimalValues, (elements[i].length, elements[i].width), (elements[i].price, cuts));

                                }
                            }
                            else
                            {
                                //optimalValues.SetValues((elements[i].length, elements[i].width), (elements[i].price, cuts));
                                SetValues(optimalValues, (elements[i].length, elements[i].width), (elements[i].price, cuts));
                            }
                        }
                        else
                        {
                            ASD.Cut cuts1Top = null, cuts1Bottom = null, cuts2Left = null, cuts2Right = null;
                            int result1 = -1, result2 = -1;
                            if (sheet.length > elements[i].length)
                                result1 = CutDynamic((length: elements[i].length, width: sheet.width), elements, out cuts1Top) +
                                    CutDynamic((length: sheet.length - elements[i].length, width: sheet.width), elements, out cuts1Bottom);

                            if (sheet.width > elements[i].width)
                                result2 = CutDynamic((length: sheet.length, width: elements[i].width), elements, out cuts2Left) +
                                    CutDynamic((length: sheet.length, width: sheet.width - elements[i].width), elements, out cuts2Right);


                            int maxValue = Math.Max(result1, result2);
                            if (maxValue > 0)
                            {
                                if (result1 >= result2)
                                {
                                    cuts = new ASD.Cut(sheet.length, sheet.width, result1, false, elements[i].length, cuts1Top, cuts1Bottom);
                                }
                                else
                                {
                                    cuts = new ASD.Cut(sheet.length, sheet.width, result2, true, elements[i].width, cuts2Left, cuts2Right);
                                }
                                if (ContainsSheet(optimalValues, sheet))
                                {
                                    if (maxValue > GetValues(optimalValues, sheet).price)
                                    {
                                        SetValues(optimalValues, sheet, (maxValue, cuts));
                                        //optimalValues.SetValues(sheet, (maxValue, cuts));
                                    }
                                }
                                else
                                {
                                    SetValues(optimalValues, sheet, (maxValue, cuts));
                                    //optimalValues.SetValues(sheet, (maxValue, cuts));
                                }
                            }
                        }
                    }
                }
            }

            if (ContainsSheet(optimalValues, sheet))
            {
                int price = 0;
                (price, cuts) = GetValues(optimalValues, sheet);
                return price;
            }


            return 0;    // zmienic w czesci A
        }

        private (int price, ASD.Cut cut) GetValues((int price, ASD.Cut cut)[,] arr, (int length, int width) sheet)
        {
            return arr[sheet.length, sheet.width];
        }

        private bool ContainsSheet((int price, ASD.Cut cut)[,] arr, (int length, int width) sheet)
        {
            var item = GetValues(arr, sheet);
            return item.price > 0 || item.cut != null;
        }

        private void SetValues((int price, ASD.Cut cut)[,] arr, (int length, int width) indices, (int price, ASD.Cut cut) value)
        {
            arr[indices.length, indices.width] = value;
        }
    }

    //internal static class ArrayExtensions
    //{
    //    internal static (int price, ASD.Cut cut) GetValues(this (int price, ASD.Cut cut)[,] arr, (int length, int width) sheet)
    //    {
    //        return arr[sheet.length, sheet.width];
    //    }

    //    internal static bool ContainsSheet(this (int price, ASD.Cut cut)[,] arr, (int length, int width) sheet)
    //    {
    //        var item = arr.GetValues(sheet);
    //        return item.price > 0 || item.cut != null;
    //    }

    //    internal static void SetValues(this (int price, ASD.Cut cut)[,] arr, (int length, int width) indices, (int price, ASD.Cut cut) value)
    //    {
    //        arr[indices.length, indices.width] = value;
    //    }
    //}
}
