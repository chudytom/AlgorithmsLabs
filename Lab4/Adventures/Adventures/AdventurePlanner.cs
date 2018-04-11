using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD
{
    /// <summary>
    /// struktura przechowująca punkt
    /// </summary>
    [Serializable]
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }

    public class AdventurePlanner : MarshalByRefObject
    {
        /// <summary>
        /// największy rozmiar tablicy, którą wyświetlamy
        /// ustaw na 0, żeby nic nie wyświetlać
        /// </summary>
        public int MaxToShow = 0;


        /// <summary>
        /// Znajduje optymalną pod względem liczby znalezionych skarbów ścieżkę,
        /// zaczynającą się w lewym górnym rogu mapy (0,0), a kończącą się w prawym
        /// dolnym rogu (X-Y-1).
        /// Za każdym razem możemy wykonać albo krok w prawo albo krok w dół.
        /// Pierwszym polem ścieżki powinno być (0,0), a ostatnim polem (X-1,Y-1).        
        /// </summary>
        /// <param name="treasure">liczba znalezionych skarbów</param>
        /// <param name="path">znaleziona ścieżka</param>
        /// <remarks>
        /// Złożoność rozwiązania to O(X * Y).
        /// </remarks>
        /// <returns></returns>
        // 1 from left
        // 2 from top
        public int FindPathThere(int[,] treasure, out List<Point> path)
        {
            int totalRows = treasure.GetLength(0);
            int totalColumns = treasure.GetLength(1);
            int[,] boardValues = new int[totalRows, totalColumns];
            int[,] prevMoves = new int[totalRows, totalColumns];
            if (totalColumns > 0 && totalRows > 0)
                boardValues[0, 0] = treasure[0, 0];
            for (int pathLength = 0; pathLength < totalRows + totalColumns - 1; pathLength++)
            {
                for (int column = 0; column < totalColumns && column <= pathLength; column++)
                {
                    int row = pathLength - column;
                    if (row < 0 || row >= totalRows)
                        continue;
                    int valueFromLeft = -1;
                    int valueFromTop = -1;
                    if (row > 0)
                        valueFromTop = boardValues[row - 1, column];
                    if (column > 0)
                        valueFromLeft = boardValues[row, column - 1];
                    if (row == 0 && column == 0)
                        continue;
                    if (valueFromLeft > valueFromTop)
                    {
                        boardValues[row, column] = valueFromLeft + treasure[row, column];
                        prevMoves[row, column] = 1;
                    }
                    else
                    {
                        boardValues[row, column] = valueFromTop + treasure[row, column];
                        prevMoves[row, column] = 2;
                    }
                }

            }

            int currentRow = totalRows - 1;
            int currentColum = totalColumns - 1;
            Stack<Point> stack = new Stack<Point>();
            while (currentRow > 0 || currentColum > 0)
            {
                stack.Push(new Point(currentRow, currentColum));
                int change = prevMoves[currentRow, currentColum];
                if (change == 1)
                {
                    currentColum--;
                }
                else if (change == 2)
                {
                    currentRow--;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            stack.Push(new Point(0, 0));
            path = new List<Point>();
            while (stack.Count > 0)
            {
                path.Add(stack.Pop());
            }

            return boardValues[totalRows - 1, totalColumns - 1];
        }

        /// <summary>
        /// Znajduje optymalną pod względem liczby znalezionych skarbów ścieżkę,
        /// zaczynającą się w lewym górnym rogu mapy (0,0), dochodzącą do prawego dolnego rogu (X-1,Y-1), a 
        /// następnie wracającą do lewego górnego rogu (0,0).
        /// W pierwszy etapie możemy wykonać albo krok w prawo albo krok w dół. Po osiągnięciu pola (x-1,Y-1)
        /// zacynamy wracać - teraz możemy wykonywać algo krok w prawo albo krok w górę.
        /// Pierwszym i ostatnim polem ścieżki powinno być (0,0).
        /// Możemy założyć, że X,Y >= 2.
        /// </summary>
        /// <param name="treasure">liczba znalezionych skarbów</param>
        /// <param name="path">znaleziona ścieżka</param>
        /// <remarks>
        /// Złożoność rozwiązania to O(X^2 * Y) lub O(X * Y^2).
        /// </remarks>
        /// <returns></returns>
        /// 
        // 1 from left
        // 2 from top
        public int FindPathThereAndBack(int[,] treasure, out List<Point> path)
        {

            int totalRows = treasure.GetLength(0);
            int totalColumns = treasure.GetLength(1);
            int[,,] boardValues = new int[totalRows, totalColumns, totalColumns];
            if (totalColumns > 0 && totalRows > 0)
                boardValues[0, 0, 0] = treasure[0, 0];
            for (int pathLength = 0; pathLength < totalRows + totalColumns - 1; pathLength++)
            {
                for (int column1 = 0; column1 < totalColumns && column1 <= pathLength; column1++)
                {
                    int row1 = pathLength - column1;
                    if (row1 < 0 || row1 >= totalRows)
                        continue;
                    for (int column2 = 0; column2 < totalColumns && column2 <= pathLength; column2++)
                    {
                        int row2 = pathLength - column2;
                        if (row2 < 0 || row2 >= totalRows)
                            continue;
                        if (row1 == 0 && column1 == 0)
                            continue;
                        int[] neighbourValues = new int[4];
                        // 0 TopTop
                        // 1 TopLeft
                        // 2 LeftTop
                        // 3 LeftLeft
                        for (int i = 0; i < neighbourValues.Length; i++)
                        {
                            neighbourValues[i] = -1;
                        }

                        //Here we count values for all the top combinations
                        //0 TopTop
                        if (row1 > 0 && row2 > 0)
                        {
                            neighbourValues[0] = boardValues[row1 - 1, column1, column2];
                        }
                        //1 TopLeft
                        if (row1 > 0 && column2 > 0)
                        {
                            neighbourValues[1] = boardValues[row1 - 1, column1, column2 - 1];
                        }
                        //2 LeftTop
                        if (column1 > 0 && row2 > 0)
                        {
                            neighbourValues[2] = boardValues[row1, column1 - 1, column2];
                        }
                        //3 LeftLeft
                        if (column1 > 0 && column2 > 0)
                        {
                            neighbourValues[3] = boardValues[row1, column1 - 1, column2 - 1];
                        }
                        //Now we need to add top value with the current treasure points and check if its the same


                        int totalExtraTreasure = treasure[row1, column1];
                        if (row1 != row2 || column1 != column2)
                        {
                            totalExtraTreasure += treasure[row2, column2];
                        }

                        int maxValue = -1;
                        for (int i = 0; i < neighbourValues.Length; i++)
                        {
                            if (neighbourValues[i] > maxValue)
                            {
                                maxValue = neighbourValues[i];
                            }
                        }

                        boardValues[row1, column1, column2] = maxValue + totalExtraTreasure;
                    }
                }
            }

            int currentRow1 = totalRows - 1;
            int currentColum1 = totalColumns - 1;
            int currentRow2 = totalRows - 1;
            int currentColum2 = totalColumns - 1;
            Stack<Point> stack1 = new Stack<Point>();
            Queue<Point> queue2 = new Queue<Point>();
            while (currentRow1 > 0 || currentColum1 > 0)
            {
                int[] neighbourValues = new int[4];
                //0 TopTop
                //1 TopLeft
                //2LeftTop
                //3LeftLeft
                for (int i = 0; i < neighbourValues.Length; i++)
                {
                    neighbourValues[i] = -1;
                }

                //Here we count values for all the top combinations
                //0 TopTop
                if (currentRow1 > 0 && currentRow2 > 0)
                {
                    neighbourValues[0] = boardValues[currentRow1 - 1, currentColum1, currentColum2];
                }
                //1 TopLeft
                if (currentRow1 > 0 && currentColum2 > 0)
                {
                    neighbourValues[1] = boardValues[currentRow1 - 1, currentColum1, currentColum2 - 1];
                }
                //2 LeftTop
                if (currentColum1 > 0 && currentRow2 > 0)
                {
                    neighbourValues[2] = boardValues[currentRow1, currentColum1 - 1, currentColum2];
                }
                //3 LeftLeft
                if (currentColum1 > 0 && currentColum2 > 0)
                {
                    neighbourValues[3] = boardValues[currentRow1, currentColum1 - 1, currentColum2 - 1];
                }

                int maxValue = -1, maxIndex = -1;
                for (int i = 0; i < neighbourValues.Length; i++)
                {
                    if (neighbourValues[i] > maxValue)
                    {
                        maxValue = neighbourValues[i];
                        maxIndex = i;
                    }
                }
                int change1 = -1;
                int change2 = -1;
                switch (maxIndex)
                {
                    case 0:
                        change1 = 2;
                        change2 = 2;
                        break;
                    case 1:
                        change1 = 2;
                        change2 = 1;
                        break;
                    case 2:
                        change1 = 1;
                        change2 = 2;
                        break;
                    case 3:
                        change1 = 1;
                        change2 = 1;
                        break;
                    default:
                        break;
                }

                stack1.Push(new Point(currentRow1, currentColum1));
                if (currentRow2 != totalRows - 1 || currentColum2 != totalColumns - 1)
                {
                    queue2.Enqueue(new Point(currentRow2, currentColum2));
                }
                if (change1 == 1)
                {
                    currentColum1--;
                }
                else if (change1 == 2)
                {
                    currentRow1--;
                }
                else
                {
                    throw new ArgumentException();
                }
                if (change2 == 1)
                {
                    currentColum2--;
                }
                else if (change2 == 2)
                {
                    currentRow2--;
                }
                else if (currentRow2 != 0 && currentColum2 != 0)
                {
                    throw new ArgumentException();
                }
            }
            stack1.Push(new Point(0, 0));
            //In case there is only one point in the top-left corner and there is actually not path back
            if (queue2.Count > 0)
            {
                queue2.Enqueue(new Point(0, 0));
            }
            path = new List<Point>();
            while (stack1.Count > 0)
            {
                path.Add(stack1.Pop());
            }
            while (queue2.Count > 0)
            {
                path.Add(queue2.Dequeue());
            }

            return boardValues[totalRows - 1, totalColumns - 1, totalColumns - 1];
        }
    }
}
