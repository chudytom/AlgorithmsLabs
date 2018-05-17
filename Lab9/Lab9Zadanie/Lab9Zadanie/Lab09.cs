using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{

    // DEFINICJA
    // Skojarzeniem indukowanym grafu G nazywamy takie skojarzenie M,
    // ze żadne dwa konce roznych krawedzi z M nie sa polaczone krawedzia w G

    // Uwagi do obu metod
    // 1) Grafow bedacych parametrami nie wolno zmieniac
    // 2) Parametrami są zawsze grafy nieskierowane (nie trzeba tego sprawdzac)

    public class Lab09 : MarshalByRefObject
    {

        /// <summary>
        /// Funkcja znajduje dowolne skojarzenie indukowane o rozmiarze k w grafie graph
        /// </summary>
        /// <param name="graph">Badany graf nieskierowany</param>
        /// <param name="k">Rozmiar szukanego skojarzenia indukowanego</param>
        /// <param name="matching">Znalezione skojarzenie (lub null jeśli nie ma)</param>
        /// <returns>true jeśli znaleziono skojarzenie, false jesli nie znaleziono</returns>
        /// <remarks>
        /// Punktacja:  2 pkt, w tym
        ///     1.5  -  dzialajacy algorytm (testy podstawowe)
        ///     0.5  -  testy wydajnościowe
        /// </remarks>
        public bool InducedMatching(Graph graph, int k, out Edge[] matching)
        {
            if (graph.Directed)
                throw new ArgumentException();
            matching = null;

            HashSet<Edge> allEdges = new HashSet<Edge>();
            List<Edge> matchingSoFar = new List<Edge>();
            List<Edge> usedEdges = new List<Edge>();


            for (int i = 0; i < graph.VerticesCount; i++)
            {
                foreach (var edge in graph.OutEdges(i))
                {
                    if (allEdges.Contains(edge) || allEdges.Contains(new Edge(edge.To, edge.From, edge.Weight)))
                        continue;
                    allEdges.Add(edge);
                }
            }
            Edge[] edgesArray = allEdges.ToArray();

            //Console.WriteLine($"Edges count: {allEdges.Count}");
            if (allEdges.Count != graph.EdgesCount)
                throw new ArgumentException();

            bool isMatchingPossible = false;
            for (int i = 0; i < edgesArray.Length; i++)
            {
                isMatchingPossible = TryAddEdge(edgesArray[i], i);
                if (isMatchingPossible)
                    break;
            }



            if(isMatchingPossible)
            {
                matching = matchingSoFar.ToArray();
            }


            return isMatchingPossible;

            bool TryAddEdge(Edge edge, int currentEdgeIndex)
            {
                matchingSoFar.Add(edge);
                if (matchingSoFar.Count >= k)
                    return true;
                for (int i = currentEdgeIndex + 1; i < graph.VerticesCount; i++)
                {
                    if (i >= edgesArray.Length)
                        continue;
                    bool isPossible = IsEdgePossible(edgesArray[i], graph, matchingSoFar);
                    if(isPossible)
                    {
                        bool isSuccess = TryAddEdge(edgesArray[i], i);
                        if (isSuccess)
                            return true;
                    }
                }
                matchingSoFar.Remove(edge);
                return false;
            }

        }

        

        private bool IsEdgePossible(Edge edge, Graph graph, List<Edge> matchingSoFar)
        {
            foreach (var matchingEdge in matchingSoFar)
            {
                if (matchingEdge.From == edge.From || matchingEdge.From == edge.To || matchingEdge.To == edge.From || matchingEdge.To == edge.To)
                    return false;
            }



            foreach (var outEdge1 in graph.OutEdges(edge.To))
            {
                //if (outEdge1.To == edge.From)
                //    continue;
                foreach (var matchingEdge in matchingSoFar)
                {
                    if (matchingEdge.To == outEdge1.From)
                        continue;
                    if (matchingEdge.From == outEdge1.To || matchingEdge.To == outEdge1.To)
                        return false;
                }
            }
            foreach (var outEdge1 in graph.OutEdges(edge.From))
            {
                //if (outEdge1.To == edge.To)
                //    continue;
                foreach (var matchingEdge in matchingSoFar)
                {
                    if (matchingEdge.To == outEdge1.From)
                        continue;
                    if (matchingEdge.From == outEdge1.To || matchingEdge.To == outEdge1.To)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Funkcja znajduje skojarzenie indukowane o maksymalnej sumie wag krawedzi w grafie graph
        /// </summary>
        /// <param name="graph">Badany graf nieskierowany</param>
        /// <param name="matching">Znalezione skojarzenie (jeśli puste to tablica 0-elementowa)</param>
        /// <returns>Waga skojarzenia</returns>
        /// <remarks>
        /// Punktacja:  2 pkt, w tym
        ///     1.5  -  dzialajacy algorytm (testy podstawowe)
        ///     0.5  -  testy wydajnościowe
        /// </remarks>
        public double MaximalInducedMatching(Graph graph, out Edge[] matching)
        {
            if (graph.Directed)
                throw new ArgumentException();
            matching = null;

            HashSet<Edge> allEdges = new HashSet<Edge>();
            List<Edge> matchingSoFar = new List<Edge>();
            List<Edge> usedEdges = new List<Edge>();

            List<Edge[]> solutions = new List<Edge[]>();
            double bestMatchingValue = 0;
            Edge[] bestMatching = new Edge[0];

            for (int i = 0; i < graph.VerticesCount; i++)
            {
                foreach (var edge in graph.OutEdges(i))
                {
                    if (allEdges.Contains(edge) || allEdges.Contains(new Edge(edge.To, edge.From, edge.Weight)))
                        continue;
                    allEdges.Add(edge);
                }
            }
            Edge[] edgesArray = allEdges.ToArray();

            //Console.WriteLine($"Edges count: {allEdges.Count}");
            if (allEdges.Count != graph.EdgesCount)
                throw new ArgumentException();

            bool isMatchingPossible = false;
            for (int i = 0; i < edgesArray.Length; i++)
            {
                isMatchingPossible = TryAddEdge(edgesArray[i], i);
                if (isMatchingPossible)
                    break;
            }



            //if (isMatchingPossible)
            //{
            //    matching = matchingSoFar.ToArray();
            //}
            matching = bestMatching;

            return bestMatchingValue;

            bool TryAddEdge(Edge edge, int currentEdgeIndex)
            {
                matchingSoFar.Add(edge);
                double tempValue = 0;
                foreach (var tempEdge in matchingSoFar)
                {
                    tempValue += tempEdge.Weight;
                }
                if(tempValue > bestMatchingValue)
                {
                    bestMatchingValue = tempValue;
                    bestMatching = matchingSoFar.ToArray();
                }
                for (int i = currentEdgeIndex + 1; i < graph.VerticesCount; i++)
                {
                    if (i >= edgesArray.Length)
                        continue;
                    bool isPossible = IsEdgePossible(edgesArray[i], graph, matchingSoFar);
                    if (isPossible)
                    {
                        bool isSuccess = TryAddEdge(edgesArray[i], i);
                        //if (isSuccess)
                        //    return true;
                    }
                }
                matchingSoFar.Remove(edge);
                return false;
            }
        }

        //funkcje pomocnicze

    }
}


