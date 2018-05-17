using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab08
{

    public class Lab08 : MarshalByRefObject
    {

        /// <summary>
        /// funkcja do sprawdzania czy da się ustawić k elementów w odległości co najmniej dist od siebie
        /// </summary>
        /// <param name="a">posortowana tablica elementów</param>
        /// <param name="dist">zadany dystans</param>
        /// <param name="k">liczba elementów do wybrania</param>
        /// <param name="exampleSolution">Wybrane elementy</param>
        /// <returns>true - jeśli zadanie da się zrealizować</returns>
        public bool CanPlaceElementsInDistance(int[] a, int dist, int k, out List<int> exampleSolution)
        {
            exampleSolution = new List<int>();
            //exampleSolution.Add(a[0]);
            int possibleBuildings = 0;
            int previousBuildingIndex = 0;
            //exampleSolution.Add(a[0]);
            for (int i = 0; i < a.Length - 1; i++)
            {
                if (Math.Abs(a[previousBuildingIndex] - a[i + 1]) >= dist)
                {
                    if (exampleSolution.Count == 0)
                    {
                        possibleBuildings++;
                        exampleSolution.Add(a[previousBuildingIndex]);
                    }

                    previousBuildingIndex = i + 1;
                    exampleSolution.Add(a[i + 1]);
                    possibleBuildings++;
                }
            }
            if (possibleBuildings >= k)
                return true;
            exampleSolution = null;
            return false;
        }

        /// <summary>
        /// Funkcja wybiera k elementów tablicy a, tak aby minimalny dystans pomiędzy dowolnymi dwiema liczbami (spośród k) był maksymalny
        /// </summary>
        /// <param name="a">posortowana tablica elementów</param>
        /// <param name="k">liczba elementów do wybrania</param>
        /// <param name="exampleSolution">Wybrane elementy</param>
        /// <returns>Maksymalny możliwy dystans między wybranymi elementami</returns>
        public int LargestMinDistance(int[] a, int k, out List<int> exampleSolution)
        {
            if (a.Length > 100000 || a.Length < 2 || k <= 1 || k > a.Length || a.Last() > int.MaxValue)
            {
                throw new ArgumentException();
            }
            if (a[0] < 0 || a[a.Length - 1] > int.MaxValue)
                throw new ArgumentException();
            List<int> bestSolution = new List<int>();
            long maxDistance = a.Last();
            long bestDistance = -5;
            long minDistance = 0;
            while (Math.Abs(maxDistance - minDistance) > 1)
            {
                long dist = (maxDistance + minDistance) / 2;
                bool isPossible = CanPlaceElementsInDistance(a, (int)dist, k, out List<int> tempSolution1);
                if (isPossible)
                {
                    bestSolution = tempSolution1;
                    bestDistance = dist;
                    minDistance = dist;
                }
                else
                {
                    maxDistance = dist;
                }
            }
            bool isBetterPossible = CanPlaceElementsInDistance(a, (int)maxDistance, k, out List<int> tempSolution2);
            if (isBetterPossible)
            {
                bestDistance = maxDistance;
                bestSolution = tempSolution2;
            }
            bool isBetterPossible2 = CanPlaceElementsInDistance(a, (int)minDistance, k, out List<int> tempSolution3);
            {
                bestDistance = minDistance;
                bestSolution = tempSolution3;
            }
            {
                if (Math.Abs(maxDistance - minDistance) <= 1)
                {
                    exampleSolution = bestSolution;
                    return (int)bestDistance;
                }
                exampleSolution = null;
                return -1;
            }

        }
    }
}
