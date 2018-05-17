using System;
using System.Collections.Generic;
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
            Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(true, streets.Length);

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
            return false;
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