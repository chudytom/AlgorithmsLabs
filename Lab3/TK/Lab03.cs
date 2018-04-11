using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ASD
{

    //public class Lab03Helper : System.MarshalByRefObject
    //{
    //    public Graph SquareOfGraph(Graph graph) => graph.SquareOfGraph();
    //    public Graph LineGraph(Graph graph, out (int x, int y)[] names) => graph.LineGraph(out names);
    //    public int VertexColoring(Graph graph, out int[] colors) => graph.VertexColoring(out colors);
    //    public int StrongEdgeColoring(Graph graph, out Graph coloredGraph) => graph.StrongEdgeColoring(out coloredGraph);
    //}

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
        public int VertexColoring(Graph graph, out int[] colors) => graph.VertexColoring(out colors);
        public int StrongEdgeColoring(Graph graph, out Graph coloredGraph) => graph.StrongEdgeColoring(out coloredGraph);
    }

    // Uwagi do wszystkich metod
    // 1) Grafy wynikowe powinny być reprezentowane w taki sam sposób jak grafy będące parametrami
    // 2) Grafów będących parametrami nie wolno zmieniać
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
            var newG = graph.IsolatedVerticesGraph();
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                foreach (var edge in graph.OutEdges(i))
                {
                    newG.AddEdge(edge.From, edge.To);
                    foreach (var secEdge in graph.OutEdges(edge.To))
                    {
                        if (secEdge.To == edge.From)
                            continue;
                        newG.AddEdge(edge.From, secEdge.To);
                    }
                }
            }
            return newG;
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
            Graph copy = graph.IsolatedVerticesGraph();
            int num = 0;
            names = new(int x, int y)[graph.EdgesCount];
            //var a = Stopwatch.StartNew();
            if (!graph.Directed)
            {
                for (int i = 0; i < graph.VerticesCount; i++)
                {
                    foreach (var edge in graph.OutEdges(i))
                    {
                        if (edge.To >= edge.From)
                        {
                            names[num] = (edge.From, edge.To);
                            copy.AddEdge(edge.From, edge.To, num++);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < graph.VerticesCount; i++)
                {
                    foreach (var edge in graph.OutEdges(i))
                    {
                        names[num] = (edge.From, edge.To);
                        copy.AddEdge(edge.From, edge.To, num++);
                    }
                }
            }
            //a.Stop();

            Graph newG = new AdjacencyListsGraph<HashTableAdjacencyList>(false, num);
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                foreach (var edge in copy.OutEdges(i))
                {
                    foreach (var secEdge in copy.OutEdges(edge.To))
                    {
                        if (edge.Weight != secEdge.Weight)
                        {
                            newG.AddEdge((int)edge.Weight, (int)secEdge.Weight);
                        }
                    }
                }
            }
            //var c = a.Elapsed;
            return newG;
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
            int numberOfColors = 0;
            colors = new int[graph.VerticesCount];
            if (graph.VerticesCount == 0)
                return 0;
            if (graph.Directed)
                throw new ArgumentException("Graf powinien być nieskierowany");
            for (int i = 0; i < graph.VerticesCount; i++)
                colors[i] = -1;

            for (int i = 0; i < graph.VerticesCount; i++)
            {
                bool[] neighborColors = new bool[numberOfColors + 1];
                foreach (var edge in graph.OutEdges(i))
                {
                    if (colors[edge.To] != -1)
                    {
                        neighborColors[colors[edge.To]] = true;
                    }
                }
                for (int j = 0; j < neighborColors.Length; j++)
                {
                    if (!neighborColors[j])
                    {
                        colors[i] = j;
                        break;
                    }
                }
                if (colors[i] == -1)
                    colors[i] = ++numberOfColors;
            }
            return numberOfColors + 1;
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
            coloredGraph = null;
            var newG = graph.LineGraph(out var names);
            var sqG = newG.SquareOfGraph();
            int colorNum = sqG.VertexColoring(out var colors);

            coloredGraph = graph.IsolatedVerticesGraph();
            for (int i = 0; i < names.Length; i++)
            {
                coloredGraph.AddEdge(names[i].x, names[i].y, colors[i]);
            }
            return colorNum;
        }
    }

}
