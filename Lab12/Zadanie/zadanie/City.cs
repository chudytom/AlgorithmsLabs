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
            return 0;
        }


        /// <summary>
        /// Sprawdza czy dla podanych par ulic możliwy jest przejazd między nimi (z użyciem być może innych ulic). 
        /// </summary>
        /// <returns>Lista, w której na i-tym miejscu jest informacja czy przejazd między ulicami w i-tej parze z wejścia jest możliwy</returns>
        public bool[] CheckStreetsPairs(Street[] streets, int[] streetsToCheck1, int[] streetsToCheck2)
        {
            return null;
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

            return new Point(0, 0);
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