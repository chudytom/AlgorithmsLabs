using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD
{
    /// <summary>
    /// struktura przechowująca punkt
    /// </summary>
    [Serializable]
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point (int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }

    public class AdventurePlanner: MarshalByRefObject
    {
        /// <summary>
        /// największy rozmiar tablicy, którą wyświetlamy
        /// ustaw na 0, żeby nic nie wyświetlać
        /// </summary>
        public int MaxToShow = 0;

      
        /// <summary>
        /// Znajduje optymalną pod względem liczby znalezionych skarbów ścieżkę,
        /// zaczynającą się w lewym górnym rogu mapy (0,0), a kończącą się w prawym
        /// dolnym rogu (X-Y-1).
        /// Za każdym razem możemy wykonać albo krok w prawo albo krok w dół.
        /// Pierwszym polem ścieżki powinno być (0,0), a ostatnim polem (X-1,Y-1).        
        /// </summary>
        /// <param name="treasure">liczba znalezionych skarbów</param>
        /// <param name="path">znaleziona ścieżka</param>
        /// <remarks>
        /// Złożoność rozwiązania to O(X * Y).
        /// </remarks>
        /// <returns></returns>
        public int FindPathThere(int[,] treasure, out List<Point> path)
        {
            path = new List<Point>();
            return -1;
        }

      
        /// <summary>
        /// Znajduje optymalną pod względem liczby znalezionych skarbów ścieżkę,
        /// zaczynającą się w lewym górnym rogu mapy (0,0), dochodzącą do prawego dolnego rogu (X-1,Y-1), a 
        /// następnie wracającą do lewego górnego rogu (0,0).
        /// W pierwszy etapie możemy wykonać albo krok w prawo albo krok w dół. Po osiągnięciu pola (x-1,Y-1)
        /// zacynamy wracać - teraz możemy wykonywać algo krok w prawo albo krok w górę.
        /// Pierwszym i ostatnim polem ścieżki powinno być (0,0).
        /// Możemy założyć, że X,Y >= 2.
        /// </summary>
        /// <param name="treasure">liczba znalezionych skarbów</param>
        /// <param name="path">znaleziona ścieżka</param>
        /// <remarks>
        /// Złożoność rozwiązania to O(X^2 * Y) lub O(X * Y^2).
        /// </remarks>
        /// <returns></returns>
        public int FindPathThereAndBack(int[,] treasure, out List<Point> path)
        {

            path = new List<Point>();
            return -1;
        }
    }
}
