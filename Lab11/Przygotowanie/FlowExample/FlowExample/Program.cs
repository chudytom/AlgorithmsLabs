
using System;
using ASD.Graphs;

static class Test
{

    public static void Main()
    {
        int n;
        double m;
        ulong c1, c2;
        Graph f;
        Graph[] g = new Graph[5];
        int[] target = new int[5];
        string[] desc = { "Graf 1", "Graf 2a", "Graf 2b", "Graf 2c", "Graf 3" };

        // Graf, dla którego metoda Forda-Fulkersona ze znajdowaniem ścieżki maksymalnie powiększającej
        // jest znacznie lepsza niż wersja korzystające z najkrótszej ścieżki powiększającej
        n = 400;
        g[0] = new AdjacencyMatrixGraph(true, n + 1);
        for (int v = 0; v < n; ++v)
            g[0].AddEdge(v, v + 1, n);
        for (int v = 1; v < n - 1; ++v)
            g[0].AddEdge(v, n - 1, 1);
        target[0] = n;

        // Graf, dla którego metoda "przepychania wstępnego przepływu"
        // jest znacznie lepsza niż dowolna wersja metody Forda-Fulkersona i Dinica
        n = 200;
        g[1] = new AdjacencyMatrixGraph(true, 2 * n + 1);
        for (int v = 1; v <= n; ++v)
        {
            g[1].AddEdge(0, v, 1);
            g[1].AddEdge(v, n + 1, 1);
        }
        for (int v = n + 1; v < 2 * n; ++v)
            g[1].AddEdge(v, v + 1, n);
        target[1] = 2 * n;

        // Graf rózniący się jedynie wagą jednej krawędzi od poprzedniego
        // tym razem metoda "przepychania wstępnego przepływu" jest wyrazie gorsza niż metoda Dinica
        g[2] = g[1].Clone();
        g[2].DelEdge(2 * n - 1, 2 * n);
        g[2].AddEdge(2 * n - 1, 2 * n, n / 2);
        target[2] = 2 * n;

        // Graf rózniący się jedynie wagą jednej (tej samej) krawędzi od dwóch poprzednich
        // tym razem najlepsza jest metoda Forda-Fulkersona ze znajdowaniem najkrótszej ścieżki powiększającej
        // (a metoda  "przepychania wstępnego przepływu" jest zdecydowanie najgorsza)
        g[3] = g[1].Clone();
        g[3].DelEdge(2 * n - 1, 2 * n);
        g[3].AddEdge(2 * n - 1, 2 * n, 1);
        target[3] = 2 * n;

        // Inny graf, dla którego metoda Dinica
        // jest lepsza niż metoda "przepychania wstępnego przepływu"
        // oraz dowolna wersja metody Forda-Fulkersona
        n = 200;
        g[4] = new AdjacencyMatrixGraph(true, 3 * n);
        for (int v = 0; v < n - 1; ++v)
            g[4].AddEdge(v, v + 1, 2 * n);
        for (int v = n; v < 2 * n; ++v)
        {
            g[4].AddEdge(n - 1, v, 2);
            g[4].AddEdge(v, 2 * n, 1);
        }
        for (int v = 2 * n; v < 3 * n - 1; ++v)
            g[4].AddEdge(v, v + 1, n);
        target[4] = 3 * n - 1;

        for (int k = 0; k < g.Length; ++k)
        {
            Console.WriteLine($"  {desc[k]}");

            c1 = Graph.Counter;
            (m, f) = g[k].FordFulkersonDinicMaxFlow(0, target[k], MaxFlowGraphExtender.BFPath);
            c2 = Graph.Counter;
            Console.WriteLine("Metoda FF z najkr. sciezka powieksz.   -  wynik: {0,4},  zlozonosc: {1,10}", m, c2 - c1);

            c1 = Graph.Counter;
            (m, f) = g[k].FordFulkersonDinicMaxFlow(0, target[k], MaxFlowGraphExtender.MaxFlowPath);
            c2 = Graph.Counter;
            Console.WriteLine("Metoda FF ze sciezka maks. powieksz.   -  wynik: {0,4},  zlozonosc: {1,10}", m, c2 - c1);

            c1 = Graph.Counter;
            (m, f) = g[k].FordFulkersonDinicMaxFlow(0, target[k], MaxFlowGraphExtender.BFMaxPath);
            c2 = Graph.Counter;
            Console.WriteLine("Metoda FF z najkr. maks. powieksz.     -  wynik: {0,4},  zlozonosc: {1,10}", m, c2 - c1);

            c1 = Graph.Counter;
            (m, f) = g[k].FordFulkersonDinicMaxFlow(0, target[k], MaxFlowGraphExtender.OriginalDinicBlockingFlow);
            c2 = Graph.Counter;
            Console.WriteLine("Metoda Dinica - oryginalna             -  wynik: {0,4},  zlozonosc: {1,10}", m, c2 - c1);

            c1 = Graph.Counter;
            (m, f) = g[k].FordFulkersonDinicMaxFlow(0, target[k], MaxFlowGraphExtender.MKMBlockingFlow);
            c2 = Graph.Counter;
            Console.WriteLine("Metoda Dinica - trzech Hindusow        -  wynik: {0,4},  zlozonosc: {1,10}", m, c2 - c1);

            c1 = Graph.Counter;
            (m, f) = g[k].FordFulkersonDinicMaxFlow(0, target[k], MaxFlowGraphExtender.DFSBlockingFlow);
            c2 = Graph.Counter;
            Console.WriteLine("Metoda Dinica - DFS                    -  wynik: {0,4},  zlozonosc: {1,10}", m, c2 - c1);

            c1 = Graph.Counter;
            (m, f) = g[k].PushRelabelMaxFlow(0, target[k]);
            c2 = Graph.Counter;
            Console.WriteLine("Metoda przepychnia wstepnego przeplywu -  wynik: {0,4},  zlozonosc: {1,10}", m, c2 - c1);

            Console.WriteLine();
        }

    }

}
