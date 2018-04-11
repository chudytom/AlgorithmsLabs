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
            if (g.Directed)
            {
                throw new ArgumentException();
            }
            g.GeneralSearchAll<EdgesStack>(null, null, null, out int cc);

            //if (g.VerticesCount - cc == g.EdgesCount && cc == 1)
            //{
            //    return true;
            //}
            //return false;
            return !FindCycle(g, out Edge[] cycle) && cc == 1;
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
            if (g.Directed)
            {
                throw new ArgumentException();
            }

            if (!IsProperSpanningTree(g, t))
                throw new ArgumentException();

            var cyclesList = new List<Edge[]>();
            HashSet<Edge> usedEdges = new HashSet<Edge>();
            Graph newTree = t.Clone();
            for (int currentVertexIndex = 0; currentVertexIndex < g.VerticesCount; currentVertexIndex++)
            {
                foreach (var edge in g.OutEdges(currentVertexIndex))
                {
                    if (newTree.AddEdge(edge) == false)
                    {
                        //newTree.DelEdge(edge);
                        continue;
                    }

                    if (usedEdges.Contains(edge) || usedEdges.Contains(new Edge(edge.To, edge.From)))
                    {
                        newTree.DelEdge(edge);
                        continue;
                    }

                    usedEdges.Add(edge);
                    bool isCycle = FindCycle(newTree, out Edge[] cycle);
                    if (isCycle)
                    {
                        cyclesList.Add(cycle);
                    }
                    newTree.DelEdge(edge);
                }
            }
            return cyclesList.ToArray();
        }

        private bool IsProperSpanningTree(Graph g, Graph t)
        {
            bool isProper = true;
            bool shouldContinue = true;
            if (g.VerticesCount != t.VerticesCount || !IsTree(t))
            {
                isProper =  false;
                shouldContinue = false;
            }

            //Graph copiedGraph = g.Clone();
            for (int vertex = 0; shouldContinue && vertex < t.VerticesCount ; vertex++)
            {
                foreach (var edge in t.OutEdges(vertex))
                {
                    if(g.AddEdge(edge))
                    {
                        g.DelEdge(edge);
                        isProper = false;
                        shouldContinue = false;
                        break;
                    }
                }
            }

            return isProper;
        }

        public bool FindCycle(Graph g, out Edge[] cycle)
        {
            cycle = null;
            List<Edge> cycleList = new List<Edge>();
            int cc;
            int[] fromVerts = new int[g.VerticesCount];
            for (int i = 0; i < fromVerts.Length; i++)
            {
                fromVerts[i] = -1;
            }
            int previousVertex = -1;

            bool isCycle = false;
            bool[] vistedVertices = new bool[g.VerticesCount];
            List<int> indexes = new List<int>();
            Predicate<int> beforeV = delegate (int n)
            {
                fromVerts[n] = previousVertex;
                isCycle = false;
                int end = -1;
                foreach (var edge in g.OutEdges(n))
                {
                    if (vistedVertices[edge.To] && edge.To != fromVerts[n] && fromVerts[n] >= 0)
                    {
                        isCycle = true;
                        end = edge.To;
                    }
                }

                if (!isCycle)
                {
                    vistedVertices[n] = true;
                    indexes.Add(n);
                    previousVertex = n;
                    return true;
                }
                else
                {
                    indexes.Add(n);
                    int previous = end;
                    Stack<int> st = new Stack<int>(indexes);
                    int current = st.Pop();
                    while (current != end)
                    {
                        cycleList.Add(new Edge(current, previous));
                        previous = current;
                        current = st.Pop();
                    }
                    cycleList.Add(new Edge(current, previous));
                    return false;
                }
            };

            Predicate<int> afterV = delegate (int n)
            {
                vistedVertices[n] = false;
                indexes.Remove(n);
                previousVertex = fromVerts[n];
                return true;
            };

            cycle = null;
            g.GeneralSearchAll<EdgesStack>(beforeV, afterV, null, out cc);
            if (isCycle)
            {
                cycleList.Reverse();
                cycle = cycleList.ToArray();
            }
            cycleList = new List<Edge>();
            return isCycle;
        }

        /// <summary>
        /// Dodaje 2 cykle fundamentalne
        /// </summary>
        /// <param name="c1">Pierwszy cykl</param>
        /// <param name="c2">Drugi cykl</param>
        /// <returns>null, jeśli wynikiem nie jest cykl i suma cykli, jeśli wynik jest cyklem</returns>
        public Edge[] AddFundamentalCycles(Edge[] c1, Edge[] c2)
        {
            HashSet<int> vertices = new HashSet<int>();

            foreach (var edge in c1)
            {
                vertices.Add(edge.To);
                vertices.Add(edge.From);
            }
            foreach (var edge in c2)
            {
                vertices.Add(edge.To);
                vertices.Add(edge.From);
            }

            //There might be problems if the vertices numbers in cycles are not in consecutive order
            //Should be easy to debug
            Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(false, vertices.Max() + 1);


            foreach (var edge in c1)
            {
                g.AddEdge(edge);
            }

            foreach (var edge in c2)
            {
                bool result = g.AddEdge(edge);
                if(result == false)
                {
                    g.DelEdge(edge);
                    //g.DelEdge(new Edge(edge.From, edge.To));
                }
            }

            bool isCycle = FindCycle(g, out Edge[] cycle);
            if (isCycle == false || cycle == null || cycle.Length == 0 || cycle.Length!= g.EdgesCount)
                return null;
            else
            {
                return cycle;
            }

            //HashSet<int> vertices = new HashSet<int>();
            //HashSet<Edge> uniqueEdgesC1 = new HashSet<Edge>(c1);
            //HashSet<Edge> uniqueEdgesC2 = new HashSet<Edge>(c2);

            //foreach (var edge in c1)
            //{
            //    if (uniqueEdgesC2.Contains(edge))
            //    {
            //        uniqueEdgesC2.Remove(edge);
            //    }
            //    if (uniqueEdgesC2.Contains(new Edge(edge.To, edge.From)))
            //    {
            //        uniqueEdgesC2.Remove(new Edge(edge.To, edge.From));
            //    }
            //}
            //foreach (var edge in c2)
            //{
            //    if (uniqueEdgesC1.Contains(edge))
            //    {
            //        uniqueEdgesC1.Remove(edge);
            //    }
            //    if (uniqueEdgesC1.Contains(new Edge(edge.To, edge.From)))
            //    {
            //        uniqueEdgesC1.Remove(new Edge(edge.To, edge.From));
            //    }
            //}

            //foreach (var edge in uniqueEdgesC1)
            //{
            //    vertices.Add(edge.From);
            //    vertices.Add(edge.To);
            //}

            //foreach (var edge in uniqueEdgesC2)
            //{
            //    vertices.Add(edge.From);
            //    vertices.Add(edge.To);
            //}

            //Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(false, );
            //foreach (var edge in uniqueEdgesC1)
            //{
            //    g.AddEdge(edge);
            //}
            //foreach (var edge in uniqueEdgesC2)
            //{
            //    g.AddEdge(edge);
            //}
            //bool isCycle = FindCycle(g, out Edge[] cycleSum);
            //if (!isCycle || cycleSum == null || cycleSum.Length == 0)
            //{
            //    return null;
            //}
            //else
            //{
            //    return cycleSum;
            //}
        }

    }

}
