using System;
using System.Collections.Generic;
using System.Linq;
using ASD.Graphs;

namespace asd2
{
    public class City : MarshalByRefObject
    {
        /// <summary>
        /// Sprawdza przecięcie zadanych ulic-odcinków. Zwraca liczbę punktów wspólnych.
        /// </summary>
        /// <returns>0 - odcinki rozłączne, 
        /// 1 - dokładnie jeden punkt wspólny, 
        /// int.MaxValue - odcinki częściowo pokrywają się (więcej niż 1 punkt wspólny)</returns>
        public int CheckIntersection(Street s1, Street s2)
        {
            Point p1 = s1.p1;
            Point p2 = s1.p2;
            Point p3 = s2.p1;
            Point p4 = s2.p2;

            double d1 = Point.CrossProduct((p4 - p3), (p1 - p3));
            double d2 = Point.CrossProduct((p4 - p3), (p2 - p3));
            double d3 = Point.CrossProduct((p2 - p1), (p3 - p1));
            double d4 = Point.CrossProduct((p2 - p1), (p4 - p1));

            double d12 = d1 * d2;
            double d34 = d3 * d4;

            if (d12 > 0 || d34 > 0)
                return 0;
            if (d12 < 0 || d34 < 0)
                return 1;

            if ((p1 == p3 || p1 == p4) && !OnRectangle(p2, p3, p4))
                return 1;

            if ((p2 == p3 || p2 == p4) && !OnRectangle(p1, p3, p4))
                return 1;

            if ((p3 == p1 || p3 == p2) && !OnRectangle(p4, p1, p2))
                return 1;

            if ((p4 == p1 || p4 == p2) && !OnRectangle(p3, p1, p2))
                return 1;



            if (OnRectangle(p1, p3, p4) || OnRectangle(p2, p3, p4) ||
                OnRectangle(p3, p1, p2) || OnRectangle(p4, p1, p2))
                return int.MaxValue;


            if (!OnRectangle(p1, p3, p4) && !OnRectangle(p2, p3, p4))
                return 0;

            if (!OnRectangle(p3, p1, p2) && !OnRectangle(p4, p1, p2))
                return 0;

            return 1;


        }

        private bool OnRectangle(Point q, Point p1, Point p2)
        {
            double x1 = p1.x, y1 = p1.y, x2 = p2.x, y2 = p2.y, x = q.x, y = q.y;
            return Math.Min(x1, x2) <= x && x <= Math.Max(x1, x2) &&
                    Math.Min(y1, y2) <= y && y <= Math.Max(y1, y2);
        }


        /// <summary>
        /// Sprawdza czy dla podanych par ulic możliwy jest przejazd między nimi (z użyciem być może innych ulic). 
        /// </summary>
        /// <returns>Lista, w której na i-tym miejscu jest informacja czy przejazd między ulicami w i-tej parze z wejścia jest możliwy</returns>
        public bool[] CheckStreetsPairs(Street[] streets, int[] streetsToCheck1, int[] streetsToCheck2)
        {
            UnionFind union = new UnionFind(streets.Length);

            for (int i = 0; i < streets.Length; i++)
            {
                for (int j = 0; j < streets.Length; j++)
                {
                    if (j == i)
                        continue;
                    //int firstStreetIndex = streetsToCheck1[i];
                    //int secondStreetIndex = streetsToCheck2[j];
                    int intersectionResult = CheckIntersection(streets[i], streets[j]);
                    if (intersectionResult == 1)
                        union.Union(i, j);
                    if (intersectionResult == int.MaxValue)
                        throw new ArgumentException();
                }
            }

            bool[] results = new bool[streetsToCheck1.Length];

            for (int i = 0; i < streetsToCheck1.Length; i++)
            {

                int firstStreetIndex = streetsToCheck1[i];
                int secondStreetIndex = streetsToCheck2[i];
                if (union.Find(firstStreetIndex) == union.Find(secondStreetIndex))
                    results[i] = true;
                else
                    results[i] = false;
            }

            return results;
        }


        /// <summary>
        /// Zwraca punkt przecięcia odcinków s1 i s2.
        /// W przypadku gdy nie ma jednoznacznego takiego punktu rzuć wyjątek ArgumentException
        /// </summary>
        public Point GetIntersectionPoint(Street s1, Street s2)
        {
            //znajdź współczynniki a i b prostych y=ax+b zawierających odcinki s1 i s2
            //uwaga na proste równoległe do osi y
            //uwaga na odcinki równoległe o wspólnych końcu
            //porównaj równania prostych, aby znaleźć ich punkt wspólny
            int intersectionResult = CheckIntersection(s1, s2);
            if (intersectionResult != 1)
                throw new ArgumentException();

            if (s1.p1 == s2.p1 || s1.p1 == s2.p2)
                return s1.p1;

            if (s1.p2 == s2.p1 || s1.p2 == s2.p2)
                return s1.p2;

            bool is1Vertical = !TryGetLinearFunction(s1.p1, s1.p2, out double a1, out double b1);
            bool is2Vertical = !TryGetLinearFunction(s2.p1, s2.p2, out double a2, out double b2);

            if (is1Vertical)
            {
                double xFromS1 = s1.p1.x;
                double y = a2 * xFromS1 + b2;
                return new Point(xFromS1, y);
            }
            else if (is2Vertical)
            {
                double xFromS2 = s2.p1.x;
                double y = a1 * xFromS2 + b1;
                return new Point(xFromS2, y);
            }
            else
            {
                double xIntersection = (b2 - b1) / (a1 - a2);
                double yIntersection = a1 * xIntersection + b1;
                return new Point(xIntersection, yIntersection);
            }

        }

        private bool TryGetLinearFunction(Point p1, Point p2, out double slope, out double valueIn0)
        {
            slope = int.MaxValue;
            if (p2.x - p1.x == 0)
            {
                valueIn0 = p1.x;
                return false;
            }
            else
            {
                slope = (p2.y - p1.y) / (p2.x - p1.x);
                valueIn0 = p1.y - slope * p1.x;
                return true;
            }
        }


        /// <summary>
        /// Sprawdza możliwość przejazdu między dzielnicami-wielokątami district1 i district2,
        /// tzn. istnieją para ulic, pomiędzy którymi jest przejazd 
        /// oraz fragment jednej ulicy należy do obszaru jednej z dzielnic i fragment drugiej należy do obszaru drugiej dzielnicy
        /// </summary>
        /// <returns>Informacja czy istnieje przejazd między dzielnicami</returns>
        public bool CheckDistricts(Street[] streets, Point[] district1, Point[] district2, out List<int> path, out List<Point> intersections)
        {
            path = new List<int>();
            intersections = new List<Point>();
            Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(false, streets.Length);

            for (int i = 0; i < streets.Length; i++)
            {
                for (int j = 0; j < streets.Length; j++)
                {
                    if (j == i)
                        continue;
                    //int firstStreetIndex = streetsToCheck1[i];
                    //int secondStreetIndex = streetsToCheck2[j];
                    int intersectionResult = CheckIntersection(streets[i], streets[j]);
                    if (intersectionResult == 1)
                        g.AddEdge(i, j);
                    if (intersectionResult == int.MaxValue)
                        throw new ArgumentException();
                }
            }

            var streetsConnectedToDiscrict1 = new List<int>();
            var streetsConnectedToDiscrict2 = new List<int>();

            for (int i = 0; i < streets.Length; i++)
            {
                if (IsStreetInsidePolygon(streets[i], district1))
                    streetsConnectedToDiscrict1.Add(i);

                if (IsStreetInsidePolygon(streets[i], district2))
                    streetsConnectedToDiscrict2.Add(i);
            }

            bool connectionBetweenDistrictsExists = false;
            Edge[] edgePath = new Edge[0];
            bool shouldContinueOuterLoop = true;
            foreach (var sourceIndex in streetsConnectedToDiscrict1)
            {
                if (shouldContinueOuterLoop)
                    foreach (int destinationIndex in streetsConnectedToDiscrict2)
                    {
                        bool pathExists = CheckIfPathExists(g, sourceIndex, destinationIndex, out edgePath);
                        if (pathExists)
                        {
                            shouldContinueOuterLoop = false;
                            connectionBetweenDistrictsExists = true;
                            if (sourceIndex == destinationIndex)
                            {
                                path.Add(sourceIndex);
                            }
                            break;
                        }
                    }
                else
                    break;
            }

            if (connectionBetweenDistrictsExists)
            {
                if (edgePath.Length != 0)
                {
                    //if (edgePath.Length < 400)
                    path.Add(edgePath.First().From);
                    foreach (var edge in edgePath)
                    {
                        path.Add(edge.To);
                    }
                    if (path.Count > 1)
                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            var intersectionPoint = GetIntersectionPoint(streets[path[i]], streets[path[i + 1]]);
                            intersections.Add(intersectionPoint);
                        }
                }
            }

            return connectionBetweenDistrictsExists;
        }

        //Consider intersections with end points
        private bool IsPointInsidePolygon(Point point, Point[] polygon)
        {
            var oxHalfLine = new Street(point, new Point(int.MaxValue, point.y));
            int intersectionsCount = 0;
            var edges = new List<Street>();
            for (int i = 0; i < polygon.Length - 1; i++)
            {
                edges.Add(new Street(polygon[i], polygon[i + 1]));
            }
            edges.Add(new Street(polygon.Last(), polygon[0]));

            foreach (var edge in edges)
            {
                if (IsPointOnLine(point, edge))
                    return true;
                int intersectionResult = CheckIntersection(oxHalfLine, edge);
                if (intersectionResult == 1)
                    intersectionsCount++;
                //else if (intersectionResult == int.MaxValue)
                //    intersectionsCount++;
            }



            if (intersectionsCount % 2 == 1)
                return true;
            else
                return false;
        }

        private bool IsStreetInsidePolygon(Street street, Point[] polygon)
        {
            //var oxHalfLine = new Street(point, new Point(int.MaxValue, point.y));
            //int intersectionsCount = 0;
            var edges = new List<Street>();
            for (int i = 0; i < polygon.Length - 1; i++)
            {
                edges.Add(new Street(polygon[i], polygon[i + 1]));
            }
            edges.Add(new Street(polygon.Last(), polygon[0]));

            foreach (var edge in edges)
            {
                int intersectionResult = CheckIntersection(edge, street);
                if (intersectionResult == 1)
                    return true;
            }
            return false;
        }

        private bool IsPointOnLine(Point p, Street street)
        {
            var p1 = street.p1;
            var p2 = street.p2;
            bool isVertical = TryGetLinearFunction(p1, p2, out double slope, out double valueIn0);
            if (isVertical)
            {
                double x = p2.x;
                if (p.x != x)
                    return false;
                if (p.y < Math.Min(p1.y, p2.y))
                    return false;
                if (p.y > Math.Max(p1.y, p2.y))
                    return false;
                else
                    return true;
            }
            double resolution = 1;
            if (Math.Abs(p.y - (slope * p.x + valueIn0)) < resolution)
                return true;
            else
                return false;
        }

        private bool CheckIfPathExists(Graph g, int source, int target, out Edge[] path)
        {
            bool pathFound = false;
            //Predicate<int> beforeVisit = (int n) =>
            //{
            //    if (n == target)
            //    {
            //        pathFound = true;
            //        return false;
            //    }
            //    else
            //        return true;
            //};

            //g.DFSearchFrom(source, beforeVisit, null);

            pathFound = g.AStar(source, target, out path);

            return pathFound;
        }

    }

    [Serializable]
    public struct Point
    {
        public double x;
        public double y;

        public Point(double px, double py) { x = px; y = py; }

        public static Point operator +(Point p1, Point p2) { return new Point(p1.x + p2.x, p1.y + p2.y); }

        public static Point operator -(Point p1, Point p2) { return new Point(p1.x - p2.x, p1.y - p2.y); }

        public static bool operator ==(Point p1, Point p2) { return p1.x == p2.x && p1.y == p2.y; }

        public static bool operator !=(Point p1, Point p2) { return !(p1 == p2); }

        public override bool Equals(object obj) { return base.Equals(obj); }

        public override int GetHashCode() { return base.GetHashCode(); }

        public static double CrossProduct(Point p1, Point p2) { return p1.x * p2.y - p2.x * p1.y; }

        public override string ToString() { return String.Format("({0},{1})", x, y); }
    }

    [Serializable]
    public struct Street
    {
        public Point p1;
        public Point p2;

        public Street(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}