using System;
using System.Linq;
using ASD.Graphs;

namespace ASD
{
    public class ProductionPlanner : MarshalByRefObject
    {
        /// <summary>
        /// Flaga pozwalająca na włączenie wypisywania szczegółów skonstruowanego planu na konsolę.
        /// Wartość <code>true</code> spoeoduje wypisanie planu.
        /// </summary>
        public bool ShowDebug { get; } = false;

        /// <summary>
        /// Część 1. zadania - zaplanowanie produkcji telewizorów dla pojedynczego kontrahenta.
        /// </summary>
        /// <remarks>
        /// Do przeprowadzenia testów wyznaczających maksymalną produkcję i zysk wymagane jest jedynie zwrócenie obiektu <see cref="PlanData"/>.
        /// Testy weryfikujące plan wymagają przypisania tablicy z planem do parametru wyjściowego <see cref="weeklyPlan"/>.
        /// </remarks>
        /// <param name="production">
        /// Tablica obiektów zawierających informacje o produkcji fabryki w kolejnych tygodniach.
        /// Wartości pola <see cref="PlanData.Quantity"/> oznaczają limit produkcji w danym tygodniu,
        /// a pola <see cref="PlanData.Value"/> - koszt produkcji jednej sztuki.
        /// </param>
        /// <param name="sales">
        /// Tablica obiektów zawierających informacje o sprzedaży w kolejnych tygodniach.
        /// Wartości pola <see cref="PlanData.Quantity"/> oznaczają maksymalną sprzedaż w danym tygodniu,
        /// a pola <see cref="PlanData.Value"/> - cenę sprzedaży jednej sztuki.
        /// </param>
        /// <param name="storageInfo">
        /// Obiekt zawierający informacje o magazynie.
        /// Wartość pola <see cref="PlanData.Quantity"/> oznacza pojemność magazynu,
        /// a pola <see cref="PlanData.Value"/> - koszt przechowania jednego telewizora w magazynie przez jeden tydzień.
        /// </param>
        /// <param name="weeklyPlan">
        /// Parametr wyjściowy, przez który powinien zostać zwrócony szczegółowy plan sprzedaży.
        /// </param>
        /// <returns>
        /// Obiekt <see cref="PlanData"/> opisujący wyznaczony plan.
        /// W polu <see cref="PlanData.Quantity"/> powinna znaleźć się maksymalna liczba wyprodukowanych telewizorów,
        /// a w polu <see cref="PlanData.Value"/> - wyznaczony maksymalny zysk fabryki.
        /// </returns>
        public PlanData CreateSimplePlan(PlanData[] production, PlanData[] sales, PlanData storageInfo,
            out SimpleWeeklyPlan[] weeklyPlan)
        {
            int verticesPerWeek = 3;
            int verticesCount = verticesPerWeek * production.Length + 2;
            Graph initialGraph = new AdjacencyListsGraph<HashTableAdjacencyList>(true, verticesCount);
            Graph flowsGraph = initialGraph.IsolatedVerticesGraph();
            Graph costGraph = initialGraph.IsolatedVerticesGraph();

            for (int week = 0; week < production.Length; week++)
            {
                flowsGraph.AddEdge(0, week * verticesPerWeek + 1, double.PositiveInfinity);
                costGraph.AddEdge(0, week * verticesPerWeek + 1, 0);

                flowsGraph.AddEdge(week * verticesPerWeek + 1, week * verticesPerWeek + 2, production[week].Quantity);
                costGraph.AddEdge(week * verticesPerWeek + 1, week * verticesPerWeek + 2, production[week].Value);

                flowsGraph.AddEdge(week * verticesPerWeek + 2, week * verticesPerWeek + 3, sales[week].Quantity);
                costGraph.AddEdge(week * verticesPerWeek + 2, week * verticesPerWeek + 3, -sales[week].Value);

                if (production.Length - week >= 2)
                {
                    flowsGraph.AddEdge(week * verticesPerWeek + 2, week * verticesPerWeek + verticesPerWeek + 2, storageInfo.Quantity);
                    costGraph.AddEdge(week * verticesPerWeek + 2, (week + 1) * verticesPerWeek + 2, storageInfo.Value);
                }

                flowsGraph.AddEdge(week * verticesPerWeek + 3, verticesCount - 1, double.PositiveInfinity);
                costGraph.AddEdge(week * verticesPerWeek + 3, verticesCount - 1, 0);
            }


            (double tvQuantity, double cost, Graph resultFlow) = flowsGraph.MinCostFlow(costGraph, 0, verticesCount - 1, true);


            weeklyPlan = new SimpleWeeklyPlan[production.Length];
            for (int week = 0; week < production.Length; week++)
            {
                weeklyPlan[week].UnitsProduced = (int)resultFlow.GetEdgeWeight(week * verticesPerWeek + 1, week * verticesPerWeek + 2);
                weeklyPlan[week].UnitsSold = (int)resultFlow.GetEdgeWeight(week * verticesPerWeek + 2, week * verticesPerWeek + 3);
                if (production.Length - week >= 2)
                    weeklyPlan[week].UnitsStored = (int)resultFlow.GetEdgeWeight(week * verticesPerWeek + 2, (week + 1) * verticesPerWeek + 2);
                else
                    weeklyPlan[week].UnitsStored = 0;
            }
            return new PlanData { Quantity = (int)tvQuantity, Value = -cost };
        }

        /// <summary>
        /// Część 2. zadania - zaplanowanie produkcji telewizorów dla wielu kontrahentów.
        /// </summary>
        /// <remarks>
        /// Do przeprowadzenia testów wyznaczających produkcję dającą maksymalny zysk wymagane jest jedynie zwrócenie obiektu <see cref="PlanData"/>.
        /// Testy weryfikujące plan wymagają przypisania tablicy z planem do parametru wyjściowego <see cref="weeklyPlan"/>.
        /// </remarks>
        /// <param name="production">
        /// Tablica obiektów zawierających informacje o produkcji fabryki w kolejnych tygodniach.
        /// Wartość pola <see cref="PlanData.Quantity"/> oznacza limit produkcji w danym tygodniu,
        /// a pola <see cref="PlanData.Value"/> - koszt produkcji jednej sztuki.
        /// </param>
        /// <param name="sales">
        /// Dwuwymiarowa tablica obiektów zawierających informacje o sprzedaży w kolejnych tygodniach.
        /// Pierwszy wymiar tablicy jest równy liczbie kontrahentów, zaś drugi - liczbie tygodni w planie.
        /// Wartości pola <see cref="PlanData.Quantity"/> oznaczają maksymalną sprzedaż w danym tygodniu,
        /// a pola <see cref="PlanData.Value"/> - cenę sprzedaży jednej sztuki.
        /// Każdy wiersz tablicy odpowiada jednemu kontrachentowi.
        /// </param>
        /// <param name="storageInfo">
        /// Obiekt zawierający informacje o magazynie.
        /// Wartość pola <see cref="PlanData.Quantity"/> oznacza pojemność magazynu,
        /// a pola <see cref="PlanData.Value"/> - koszt przechowania jednego telewizora w magazynie przez jeden tydzień.
        /// </param>
        /// <param name="weeklyPlan">
        /// Parametr wyjściowy, przez który powinien zostać zwrócony szczegółowy plan sprzedaży.
        /// </param>
        /// <returns>
        /// Obiekt <see cref="PlanData"/> opisujący wyznaczony plan.
        /// W polu <see cref="PlanData.Quantity"/> powinna znaleźć się optymalna liczba wyprodukowanych telewizorów,
        /// a w polu <see cref="PlanData.Value"/> - wyznaczony maksymalny zysk fabryki.
        /// </returns>
        public PlanData CreateComplexPlan(PlanData[] production, PlanData[,] sales, PlanData storageInfo,
            out WeeklyPlan[] weeklyPlan)
        {
            int verticesPerWeek = 2 + sales.GetLength(0);
            int verticesCount = verticesPerWeek * production.Length + 2;
            Graph initialGraph = new AdjacencyListsGraph<HashTableAdjacencyList>(true, verticesCount);
            Graph flowsGraph = initialGraph.IsolatedVerticesGraph();
            Graph costGraph = initialGraph.IsolatedVerticesGraph();

            for (int week = 0; week < production.Length; week++)
            {
                flowsGraph.AddEdge(0, week * verticesPerWeek + 1, double.PositiveInfinity);
                costGraph.AddEdge(0, week * verticesPerWeek + 1, 0);

                flowsGraph.AddEdge(week * verticesPerWeek + 1, week * verticesPerWeek + 2, production[week].Quantity);
                costGraph.AddEdge(week * verticesPerWeek + 1, week * verticesPerWeek + 2, production[week].Value);


                if (production.Length - week >= 2)
                {
                    flowsGraph.AddEdge(week * verticesPerWeek + 2, week * verticesPerWeek + verticesPerWeek + 2, storageInfo.Quantity);
                    costGraph.AddEdge(week * verticesPerWeek + 2, (week + 1) * verticesPerWeek + 2, storageInfo.Value);
                }
                //for (int i = 0; i < length; i++)
                //{

                //}
                //flowsGraph.AddEdge(week * verticesPerWeek + 2, week * verticesPerWeek + 3, sales[week].Quantity);
                //costGraph.AddEdge(week * verticesPerWeek + 2, week * verticesPerWeek + 3, -sales[week].Value);

                flowsGraph.AddEdge(week * verticesPerWeek + 3, verticesCount - 1, double.PositiveInfinity);
                costGraph.AddEdge(week * verticesPerWeek + 3, verticesCount - 1, 0);
            }


            (double tvQuantity, double cost, Graph resultFlow) = flowsGraph.MinCostFlow(costGraph, 0, verticesCount - 1, true);


            weeklyPlan = new WeeklyPlan[production.Length];
            for (int week = 0; week < production.Length; week++)
            {
                weeklyPlan[week].UnitsProduced = (int)resultFlow.GetEdgeWeight(week * verticesPerWeek + 1, week * verticesPerWeek + 2);
                //weeklyPlan[week].UnitsSold = (int)resultFlow.GetEdgeWeight(week * verticesPerWeek + 2, week * verticesPerWeek + 3);
                if (production.Length - week >= 2)
                    weeklyPlan[week].UnitsStored = (int)resultFlow.GetEdgeWeight(week * verticesPerWeek + 2, (week + 1) * verticesPerWeek + 2);
                else
                    weeklyPlan[week].UnitsStored = 0;
            }
            return new PlanData { Quantity = (int)tvQuantity, Value = -cost };
        }

    }
}