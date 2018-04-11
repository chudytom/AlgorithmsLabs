using System;
using System.Collections.Generic;
using ASD.Graphs;

namespace ASD
{

    // Klasy Lab03Helper NIE WOLNO ZMIENIAĆ !!!
    public class Lab03Helper : System.MarshalByRefObject
    {
        public Graph SquareOfGraph(Graph graph) => graph.SquareOfGraph();
        public Graph LineGraph(Graph graph, out int[,] names)
        {
            Graph g = graph.LineGraph(out (int x, int y)[] _names);
            if (_names == null)
                names = null;
            else
            {
                names = new int[_names.Length, 2];
                for (int i = 0; i < _names.Length; ++i)
                {
                    names[i, 0] = _names[i].x;
                    names[i, 1] = _names[i].y;
                }
            }
            return g;
        }
        public Graph LineGraph(Graph graph, out (int x, int y)[] names) => graph.LineGraph(out names);
        public int VertexColoring(Graph graph, out int[] colors) => graph.VertexColoring(out colors);
        public int StrongEdgeColoring(Graph graph, out Graph coloredGraph) => graph.StrongEdgeColoring(out coloredGraph);
    }



    // Uwagi do wszystkich metod
    // 1) Grafy wynikowe powinny być reprezentowane w taki sam sposób jak grafy będące parametrami
    // 2) Grafów będących parametrami nie wolno zmieniać
    static class Lab03
    {

        // 0.5 pkt
        // Funkcja zwracajaca kwadrat grafu graph.
        // Kwadratem grafu nazywamy graf o takim samym zbiorze wierzcholkow jak graf bazowy,
        // 2 wierzcholki polaczone sa krawedzia jesli w grafie bazowym byly polaczone krawedzia badz sciezka zlozona z 2 krawedzi
        public static Graph SquareOfGraph(this Graph graph)
        {

            Graph g2 = graph.IsolatedVerticesGraph();
            for (int vertex = 0; vertex < g2.VerticesCount; vertex++)
            {
                foreach (var edge in graph.OutEdges(vertex))
                {
                    g2.AddEdge(edge);
                    foreach (var edge2 in graph.OutEdges(edge.To))
                    {
                        if (edge.From == edge2.To)
                        {
                            continue;
                        }
                        g2.AddEdge(edge.From, edge2.To);
                    }
                }
            }

            return g2;

        }

        // 2 pkt
        // Funkcja zwracająca Graf krawedziowy grafu graph
        // Wierzcholki grafu krawedziwego odpowiadaja krawedziom grafu bazowego,
        // 2 wierzcholki grafu krawedziwego polaczone sa krawedzia
        // jesli w grafie bazowym z krawędzi odpowiadającej pierwszemu z nich można przejść 
        // na krawędź odpowiadającą drugiemu z nich przez wspólny wierzchołek.
        //
        // (w grafie skierowanym: 2 wierzcholki grafu krawedziwego polaczone sa krawedzia
        // jesli wierzcholek koncowy krawedzi odpowiadajacej pierwszemu z nich
        // jest wierzcholkiem poczatkowym krawedzi odpowiadajacej drugiemu z nich)
        //
        // do tablicy names nalezy wpisac numery wierzcholkow grafu krawedziowego,
        // np. dla wierzcholka powstalego z krawedzi <0,1> do tabeli zapisujemy krotke (0, 1) - przyda się w dalszych etapach
        //
        // UWAGA: Graf bazowy może być skierowany lub nieskierowany, graf krawędziowy zawsze jest nieskierowany.
        public static Graph LineGraph(this Graph graph, out (int x, int y)[] names)
        {
            // Moze warto stworzyc...
            // graf pomocniczy o takiej samej strukturze krawedzi co pierwotny, 
            // waga krawedzi jest numer krawedzi w grafie (taka sztuczka - to beda numery wierzcholkow w grafie krawedzowym)
            names = null;
            List<(int x, int y)> namesList = new List<(int x, int y)>();

            Graph lineGraph = graph.IsolatedVerticesGraph(false, graph.EdgesCount);
            Graph helperGraph = graph.Clone();

            if (graph.Directed)
            {
                int edgeDirectedNumber = 0;
                for (int vertex = 0; vertex < helperGraph.VerticesCount; vertex++)
                {
                    foreach (var edge in helperGraph.OutEdges(vertex))
                    {
                        helperGraph.ModifyEdgeWeight(edge.From, edge.To, edgeDirectedNumber - edge.Weight);
                        edgeDirectedNumber++;
                        namesList.Add((edge.From, edge.To));
                    }
                }

                for (int vertex = 0; vertex < helperGraph.VerticesCount; vertex++)
                {
                    foreach (var edge in helperGraph.OutEdges(vertex))
                    {
                        foreach (var edge2 in helperGraph.OutEdges(edge.To))
                        {
                            //if (edge2.To == edge.From)
                            //    continue;
                            lineGraph.AddEdge((int)edge.Weight, (int)edge2.Weight);
                        }
                    }
                }
                names = namesList.ToArray();
                return lineGraph;
            }

            int edgeNumber = 0;
            bool[] visitedVertices = new bool[helperGraph.VerticesCount];
            for (int vertex = 0; vertex < helperGraph.VerticesCount; vertex++)
            {
                visitedVertices[vertex] = true;
                foreach (var edge in helperGraph.OutEdges(vertex))
                {
                    if (visitedVertices[edge.To] == false)
                    {
                        var value = helperGraph.ModifyEdgeWeight(edge.From, edge.To, edgeNumber - edge.Weight);
                        edgeNumber++;
                        namesList.Add((edge.From, edge.To));
                    }
                }
            }
            visitedVertices = new bool[helperGraph.VerticesCount];
            for (int vertex = 0; vertex < helperGraph.VerticesCount; vertex++)
            {
                visitedVertices[vertex] = true;
                foreach (var edge in helperGraph.OutEdges(vertex))
                {
                    if (visitedVertices[edge.To] == false)
                    {
                        //Console.WriteLine($"Realna waga krawędzi {edge.Weight}");
                        edgeNumber = (int)edge.Weight;
                        foreach (var incomingEdge in helperGraph.OutEdges(edge.From))
                        {
                            if (incomingEdge.To == edge.To)
                                continue;
                            lineGraph.AddEdge(edgeNumber, (int)incomingEdge.Weight);
                        }

                        foreach (var outgoingEdge in helperGraph.OutEdges(edge.To))
                        {
                            if (outgoingEdge.To == edge.From)
                                continue;
                            lineGraph.AddEdge(edgeNumber, (int)outgoingEdge.Weight);
                        }
                    }
                }
            }

            names = namesList.ToArray();
            return lineGraph;
        }

        // 1 pkt
        // Funkcja znajdujaca poprawne kolorowanie wierzcholkow grafu graph
        // Kolorowanie wierzcholkow jest poprawne, gdy kazde dwa sasiadujace wierzcholki maja rozne kolory
        // Funkcja ma szukać kolorowania wedlug nastepujacego algorytmu zachlannego:
        //
        // Dla wszystkich wierzcholkow (od 0 do n-1) 
        //      pokoloruj wierzcholek v na najmniejszy mozliwy kolor (czyli taki, na ktory nie sa pomalowani jego sasiedzi)
        //
        // Nalezy zwrocic liczbe kolorow, a w tablicy colors zapamietac kolory dla poszczegolnych wierzcholkow
        //
        // UWAGA: Dla grafów skierowanych metoda powinna zgłaszać wyjątek ArgumentException
        public static int VertexColoring(this Graph graph, out int[] colors)
        {
            if (graph.Directed)
            {
                throw new ArgumentException();
            }
            int[] colorOfVertex = new int[graph.VerticesCount];
            for (int i = 0; i < colorOfVertex.Length; i++)
            {
                colorOfVertex[i] = -1;
            }
            HashSet<int> usedColors = new HashSet<int>();

            bool ColorVertex(int n)
            {
                HashSet<int> neighboursColors = new HashSet<int>();
                foreach (var edge in graph.OutEdges(n))
                {
                    if (colorOfVertex[edge.To] >= 0)
                    {
                        neighboursColors.Add(colorOfVertex[edge.To]);
                    }
                }
                bool isColored = false;
                for (int i = 0; i < usedColors.Count; i++)
                {
                    if (neighboursColors.Contains(i))
                    {
                        continue;
                    }
                    else
                    {
                        colorOfVertex[n] = i;
                        isColored = true;
                        break;
                    }
                }
                if (!isColored)
                {
                    int newColor = usedColors.Count;
                    colorOfVertex[n] = newColor;
                    usedColors.Add(newColor);
                }

                return true;
            }
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                ColorVertex(i);
            }

            colors = colorOfVertex;
            return usedColors.Count;
        }

        // 0.5 pkt
        // Funkcja znajdujaca silne kolorowanie krawedzi grafu graph
        // Silne kolorowanie krawedzi grafu jest poprawne gdy kazde dwie krawedzie, ktore sa ze soba sasiednie
        // albo sa polaczone inna krawedzia, maja rozne kolory.
        //
        // Nalezy zwrocic nowy graf, ktory bedzie kopia zadanego grafu, ale w wagach krawedzi zostana zapisane znalezione kolory
        // 
        // Wskazowka - to bardzo proste. Nalezy tu wykorzystac wszystkie poprzednie funkcje. 
        // Zastanowic sie co mozemy powiedziec o kolorowaniu wierzcholkow kwadratu grafu krawedziowego - jak sie ma do silnego kolorowania krawedzi grafu bazowego
        public static int StrongEdgeColoring(this Graph graph, out Graph coloredGraph)
        {
            coloredGraph = graph.Clone();
            Graph lineGraph = LineGraph(graph, out (int x, int y)[] edgesNames);
            Graph squareGraph = lineGraph.SquareOfGraph();
            int colorsCount = squareGraph.VertexColoring(out int[] colors);
            //Console.WriteLine($"Colors in this graph {colorsCount}");


            Dictionary<(int x, int y), int> names = new Dictionary<(int x, int y), int>();
            for (int i = 0; i < edgesNames.Length; i++)
            {
                names.Add(edgesNames[i], i);
            }

            for (int i = 0; i < edgesNames.Length; i++)
            {
                var edge = edgesNames[i];
                int color = colors[i];
                double currentWeight = coloredGraph.GetEdgeWeight(edge.x, edge.y);
                coloredGraph.ModifyEdgeWeight(edge.x, edge.y, color - currentWeight);
            }



            return colorsCount;
        }
    }
}
