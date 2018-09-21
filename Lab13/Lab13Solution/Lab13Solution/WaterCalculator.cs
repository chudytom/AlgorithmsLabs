using System;
using System.Collections.Generic;

namespace ASD
{
    public class WaterCalculator : MarshalByRefObject
    {

        /*
         * Metoda sprawdza, czy przechodząc p1->p2->p3 skręcamy w lewo 
         * (jeżeli idziemy prosto, zwracany jest fałsz).
         */
        private bool leftTurn(Point p1, Point p2, Point p3)
        {
            Point w1 = new Point(p2.x - p1.x, p2.y - p1.y);
            Point w2 = new Point(p3.x - p2.x, p3.y - p2.y);
            double vectProduct = w1.x * w2.y - w2.x * w1.y;
            return vectProduct > 0;
        }


        /*
         * Metoda wyznacza punkt na odcinku p1-p2 o zadanej współrzędnej y.
         * Jeżeli taki punkt nie istnieje (bo cały odcinek jest wyżej lub niżej), zgłaszany jest wyjątek ArgumentException.
         */
        private Point getPointAtY(Point p1, Point p2, double y)
        {
            if (p1.y != p2.y)
            {
                double newX = p1.x + (p2.x - p1.x) * (y - p1.y) / (p2.y - p1.y);
                if ((newX - p1.x) * (newX - p2.x) > 0)
                    throw new ArgumentException("Odcinek p1-p2 nie zawiera punktu o zadanej współrzędnej y!");
                return new Point(p1.x + (p2.x - p1.x) * (y - p1.y) / (p2.y - p1.y), y);
            }
            else
            {
                if (p1.y != y)
                    throw new ArgumentException("Odcinek p1-p2 nie zawiera punktu o zadanej współrzędnej y!");
                return new Point((p1.x + p2.x) / 2, y);
            }
        }


        /// <summary>
        /// Funkcja zwraca tablice t taką, że t[i] jest głębokością, na jakiej znajduje się punkt points[i].
        /// 
        /// Przyjmujemy, że pierwszy punkt z tablicy points jest lewym krańcem, a ostatni - prawym krańcem łańcucha górskiego.
        /// </summary>
        public double[] PointDepths(Point[] points)
        {
            int highest = -1;
            int secondHighest = -1;
            double[] depths = new double[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                if (highest == -1)
                    highest = i;
                else if (points[i].y >= points[highest].y)
                {
                    secondHighest = highest;
                    highest = i;
                    double mirrolLevel1 = points[secondHighest].y;

                    for (int j = Math.Min(highest, secondHighest); j < Math.Max(secondHighest, highest); j++)
                    {
                        depths[j] = Math.Max(0, mirrolLevel1 - points[j].y);
                    }
                    secondHighest = -1;

                }
                else if (secondHighest == -1)
                {
                    secondHighest = i;
                }
                else if (points[i].y > points[secondHighest].y)
                {
                    secondHighest = i;
                }

            }

            if (secondHighest != -1)
            {
                double mirrolLevel2 = points[secondHighest].y;
                for (int j = Math.Min(highest, secondHighest); j < Math.Max(secondHighest, highest); j++)
                {
                    depths[j] = Math.Max(0, mirrolLevel2 - points[j].y);
                }
            }

            return depths;
        }

        /// <summary>
        /// Funkcja zwraca objętość wody, jaka zatrzyma się w górach.
        /// 
        /// Przyjmujemy, że pierwszy punkt z tablicy points jest lewym krańcem, a ostatni - prawym krańcem łańcucha górskiego.
        /// </summary>
        public double WaterVolume(Point[] points)
        {
            int highest = -1;
            int secondHighest = -1;
            double volume = 0;

            for (int i = 0; i < points.Length; i++)
            {
                if (highest == -1)
                    highest = i;
                else if (points[i].y >= points[highest].y)
                {
                    secondHighest = highest;
                    highest = i;
                    double mirrolLevel1 = points[secondHighest].y;
                    var groupOfPoints = new List<Point>();

                    var lowerPoint = new Point();

                    if (secondHighest > highest)
                    {
                        lowerPoint = points[secondHighest - 1];
                    }
                    else
                    {
                        lowerPoint = points[secondHighest + 1];
                    }

                    var extraPoint = getPointAtY(lowerPoint, points[secondHighest], mirrolLevel1);
                    if (secondHighest < highest)
                    {
                        groupOfPoints.Add(extraPoint);
                    }
                    for (int j = Math.Min(highest, secondHighest); j < Math.Max(secondHighest, highest); j++)
                    {
                        if (j == highest)
                            continue;
                        groupOfPoints.Add(points[j]);
                    }
                    if (secondHighest > highest)
                    {
                        groupOfPoints.Add(extraPoint);
                    }
                    var someVolume = GetVolumeForPoints(groupOfPoints);
                    volume += someVolume;
                    secondHighest = -1;

                }
                else if (secondHighest == -1)
                {
                    secondHighest = i;
                }
                else if (points[i].y > points[secondHighest].y)
                {
                    secondHighest = i;
                }

            }
            if (secondHighest != -1)
            {
                double mirrolLevel1 = points[secondHighest].y;
                var groupOfPoints = new List<Point>();

                var lowerPoint = new Point();

                if (secondHighest > highest)
                {
                    lowerPoint = points[secondHighest - 1];
                }
                else
                {
                    lowerPoint = points[secondHighest + 1];
                }


                var extraPoint = getPointAtY(lowerPoint, points[secondHighest], mirrolLevel1);
                if (secondHighest < highest)
                {
                    groupOfPoints.Add(extraPoint);
                }
                for (int j = Math.Min(highest, secondHighest); j < Math.Max(secondHighest, highest); j++)
                {
                    if (j == highest)
                        continue;
                    groupOfPoints.Add(points[j]);
                }

                if (secondHighest > highest)
                {
                    groupOfPoints.Add(extraPoint);
                }


                var someVolume = GetVolumeForPoints(groupOfPoints);
                volume += someVolume;
                secondHighest = -1;
            }

            return volume;
        }
        private double GetVolumeForPoints(List<Point> points)
        {
            double volume = 0;
            int n = points.Count - 1;
            for (int i = 1; i < points.Count - 1; i++)
            {
                volume += points[i].x * points[i + 1].y - points[i].x * points[i - 1].y;
            }
            //volume += points[0].x * points[1].y - points[0].x * points[n - 1].y;
            //volume += points[n].x * points[0].y - points[n].x * points[n - 1].y;

            return 0.5 * Math.Abs(volume);
        }
    }


    [Serializable]
    public struct Point
    {
        public double x, y;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
