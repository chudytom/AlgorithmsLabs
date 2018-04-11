using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{
    public class CyclesFinder : MarshalByRefObject
    {
        /// <summary>
        /// Sprawdza czy graf jest drzewem
        /// </summary>
        /// <param name="g">Graf</param>
        /// <returns>true jeśli graf jest drzewem</returns>
        public bool IsTree(Graph g)
        {
            return false;
        }

        /// <summary>
        /// Wyznacza cykle fundamentalne grafu g względem drzewa t.
        /// Każdy cykl fundamentalny zawiera dokadnie jedną krawędź spoza t.
        /// </summary>
        /// <param name="g">Graf</param>
        /// <param name="t">Drzewo rozpinające grafu g</param>
        /// <returns>Tablica cykli fundamentalnych</returns>
        /// <remarks>W przypadku braku cykli zwracać pustą (0-elementową) tablicę, a nie null</remarks>
        public Edge[][] FindFundamentalCycles(Graph g, Graph t)
        {
            return null;
        }

        /// <summary>
        /// Dodaje 2 cykle fundamentalne
        /// </summary>
        /// <param name="c1">Pierwszy cykl</param>
        /// <param name="c2">Drugi cykl</param>
        /// <returns>null, jeśli wynikiem nie jest cykl i suma cykli, jeśli wynik jest cyklem</returns>
        public Edge[] AddFundamentalCycles(Edge[] c1, Edge[] c2)
        {
            return null;
        }

    }

}
