﻿using ASD.Graphs;
using System;
using System.Collections.Generic;

namespace ASD
{

    // Klasy Lab03Helper NIE WOLNO ZMIENIAĆ !!!
    public class Lab03Helper : System.MarshalByRefObject
    {
        public Graph SquareOfGraph(Graph graph) => graph.SquareOfGraph();
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
            List<Edge> edges = new List<Edge>();
            Graph g2 = graph.Clone();
            Predicate<int> before = delegate (int n)
            {
                foreach (var edge1 in graph.OutEdges(n))
                {
                    foreach (var edge2 in graph.OutEdges(edge1.To))
                    {
                        if (edge2.To == edge1.From)
                        {
                            continue;
                        }
                        g2.AddEdge(n, edge2.To);
                    }
                }
                return true;
            };


            g2.DFSearchAll(before, null, out int cc);
            return g2;
        }

        // 2 pkt
        // Funkcja zwracająca Graf krawedziowy grafu graph
        // Wierzcholki grafu krawedziwego odpowiadaja krawedziom grafu bazowego,
        // 2 wierzcholki grafu krawedziwego polaczone sa krawedzia
        // jesli w grafie bazowym z krawędzi odpowiadającej pierwszemu z nic można przejść 
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
            var g1 = SquareOfGraph(graph);
            for (int vertex = 0; vertex < graph.VerticesCount; vertex++)
            {
                foreach (var edge in graph.OutEdges(vertex))
                {
                    g1.DelEdge(edge.From, edge.To);
                }
            }
            // Moze warto stworzyc...
            // graf pomocniczy o takiej samej strukturze krawedzi co pierwotny, 
            // waga krawedzi jest numer krawedzi w grafie (taka sztuczka - to beda numery wierzcholkow w grafie krawedzowym)
            names = null;
            return g1 ;
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
            coloredGraph = null;
            return 0;
        }
    }
}
