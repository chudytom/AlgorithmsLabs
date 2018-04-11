using ASD.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASD
{
    public class Maze : MarshalByRefObject
    {

        /// <summary>
        /// Wersje zadania I oraz II
        /// Zwraca najkrótszy możliwy czas przejścia przez labirynt bez dynamitów lub z dowolną ich liczbą
        /// </summary>
        /// <param name="maze">labirynt</param>
        /// <param name="withDynamite">informacja, czy dostępne są dynamity 
        /// Wersja I zadania -> withDynamites = false, Wersja II zadania -> withDynamites = true</param>
        /// <param name="path">zwracana ścieżka</param>
        /// <param name="t">czas zburzenia ściany (dotyczy tylko wersji II)</param> 
        public int FindShortestPath(char[,] maze, bool withDynamite, out string path, int t = 0)
        {
            //path = null; // tej linii na laboratorium nie zmieniamy!

            int totalRows = maze.GetLength(0);
            int totalColumns = maze.GetLength(1);
            int totalFields = totalRows * totalColumns;
            int startVertex = -1;
            int endVertex = -1;
            HashSet<char> goodChars = new HashSet<char>() { 'O', 'S', 'E' };
            Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(true, totalFields);
            for (int row = 0; row < totalRows; row++)
            {
                for (int column = 0; column < totalColumns; column++)
                {

                    //if (maze[row, column] == 'X')
                    //    continue;
                    if (maze[row, column] == 'S')
                        startVertex = GetVertexNumber(row, column);
                    if (maze[row, column] == 'E')
                        endVertex = GetVertexNumber(row, column);
                    int currentVertexNumber = GetVertexNumber(row, column);
                    if (row > 0)
                    {
                        int otherVertexNumber = GetVertexNumber(row - 1, column);
                        if (goodChars.Contains(maze[row - 1, column]))
                            g.AddEdge(currentVertexNumber, otherVertexNumber);
                        if (maze[row - 1, column] == 'X' && t != 0)
                        {
                            g.AddEdge(currentVertexNumber, otherVertexNumber, t);
                        }
                    }
                    if (row < totalRows - 1)
                    {
                        int otherVertexNumber = GetVertexNumber(row + 1, column);
                        if (goodChars.Contains(maze[row + 1, column]))
                            g.AddEdge(currentVertexNumber, otherVertexNumber);
                        if (maze[row + 1, column] == 'X' && t != 0)
                        {
                            g.AddEdge(currentVertexNumber, otherVertexNumber, t);
                        }
                    }
                    if (column > 0)
                    {
                        int otherVertexNumber = GetVertexNumber(row, column - 1);
                        if (goodChars.Contains(maze[row, column - 1]))
                            g.AddEdge(currentVertexNumber, otherVertexNumber);
                        if (maze[row, column - 1] == 'X' && t != 0)
                        {
                            g.AddEdge(currentVertexNumber, otherVertexNumber, t);
                        }
                    }
                    if (column < totalColumns - 1)
                    {
                        int otherVertexNumber = GetVertexNumber(row, column + 1);
                        if (goodChars.Contains(maze[row, column + 1]))
                            g.AddEdge(currentVertexNumber, otherVertexNumber);
                        if (maze[row, column + 1] == 'X' && t != 0)
                        {
                            g.AddEdge(currentVertexNumber, otherVertexNumber, t);
                        }
                    }
                }
            }

            bool result = g.DijkstraShortestPaths(startVertex, out PathsInfo[] pathsInfo);

            double length = pathsInfo[endVertex].Dist;
            if (double.IsNaN(length))
            {
                path = "";
                return -1;
            }
            else
            {
                Edge[] pathEdges = PathsInfo.ConstructPath(startVertex, endVertex, pathsInfo);

                StringBuilder pathStringBuilder = new StringBuilder();
                foreach (var edge in pathEdges)
                {
                    pathStringBuilder.Append(ConvertEdgeToChar(edge));
                }

                path = pathStringBuilder.ToString();
                return (int)length;
            }

            //--------------------------------------------------------
            int GetVertexNumber(int row, int column)
            {
                return row * totalColumns + column;
            }

            char ConvertEdgeToChar(Edge edge)
            {
                var startField = GetRowAndColumn(edge.From);
                var endField = GetRowAndColumn(edge.To);
                if (startField.row == endField.row)
                {
                    if (endField.column > startField.column)
                        return 'E';
                    else if (endField.column < startField.column)
                        return 'W';
                }
                else if (startField.column == endField.column)
                {
                    if (endField.row > startField.row)
                        return 'S';
                    else if (endField.row < startField.row)
                        return 'N';
                }
                throw new ArgumentException("Incorrect edge");
            }

            Field GetRowAndColumn(int vertexNumber)
            {
                int row = vertexNumber / totalColumns;
                int column = vertexNumber - row * totalColumns;
                return new Field(row, column);
            }
        }


        /// <summary>
        /// Wersja III i IV zadania
        /// Zwraca najkrótszy możliwy czas przejścia przez labirynt z użyciem co najwyżej k lasek dynamitu
        /// </summary>
        /// <param name="maze">labirynt</param>
        /// <param name="k">liczba dostępnych lasek dynamitu, dla wersji III k=1</param>
        /// <param name="path">zwracana ścieżka</param>
        /// <param name="t">czas zburzenia ściany</param>
        public int FindShortestPathWithKDynamites(char[,] maze, int k, out string path, int t)
        {
            path = ""; // tej linii na laboratorium nie zmieniamy!

            int totalRows = maze.GetLength(0);
            int totalColumns = maze.GetLength(1);
            int totalFields = totalRows * totalColumns;
            int[] startVerices = new int[k + 1];
            int[] endVertices = new int[k + 1];
            for (int i = 0; i < startVerices.Length; i++)
            {
                startVerices[i] = endVertices[i] = -1;
            }

            HashSet<char> goodChars = new HashSet<char>() { 'O', 'S', 'E' };
            Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(true, totalFields * (k + 1));
            for (int row = 0; row < totalRows; row++)
            {
                for (int column = 0; column < totalColumns; column++)
                {

                    if (maze[row, column] == 'S')
                    {
                        for (int layerNumber = 0; layerNumber < startVerices.Length; layerNumber++)
                        {
                            startVerices[layerNumber] = GetVertexNumber(row, column, layerNumber);
                        }
                    }

                    if (maze[row, column] == 'E')
                    {
                        for (int layerNumber = 0; layerNumber < endVertices.Length; layerNumber++)
                        {
                            endVertices[layerNumber] = GetVertexNumber(row, column, layerNumber);
                        }
                    }

                    int[] currentVerticesNumbers = new int[k + 1];
                    int[] otherVerticesNumbers = new int[k + 1];
                    for (int layerNumber = 0; layerNumber < currentVerticesNumbers.Length; layerNumber++)
                    {
                        currentVerticesNumbers[layerNumber] = GetVertexNumber(row, column, layerNumber);
                    }
                    if (row > 0)
                    {
                        for (int layerNumber = 0; layerNumber < otherVerticesNumbers.Length; layerNumber++)
                        {
                            otherVerticesNumbers[layerNumber] = GetVertexNumber(row - 1, column, layerNumber);
                        }
                        if (goodChars.Contains(maze[row - 1, column]))
                        {
                            for (int layerNumber = 0; layerNumber < currentVerticesNumbers.Length; layerNumber++)
                            {
                                g.AddEdge(currentVerticesNumbers[layerNumber], otherVerticesNumbers[layerNumber]);
                            }
                        }

                        if (maze[row - 1, column] == 'X' && t != 0)
                        {
                            for (int layerNumber = 0; layerNumber < k; layerNumber++)
                            {
                                g.AddEdge(currentVerticesNumbers[layerNumber], otherVerticesNumbers[layerNumber + 1], t);
                            }
                        }
                    }
                    if (row < totalRows - 1)
                    {
                        for (int layerNumber = 0; layerNumber < otherVerticesNumbers.Length; layerNumber++)
                        {
                            otherVerticesNumbers[layerNumber] = GetVertexNumber(row + 1, column, layerNumber);
                        }
                        if (goodChars.Contains(maze[row + 1, column]))
                        {
                            for (int layerNumber = 0; layerNumber < currentVerticesNumbers.Length; layerNumber++)
                            {
                                g.AddEdge(currentVerticesNumbers[layerNumber], otherVerticesNumbers[layerNumber]);
                            }
                        }

                        if (maze[row + 1, column] == 'X' && t != 0)
                        {
                            for (int layerNumber = 0; layerNumber < k; layerNumber++)
                            {
                                g.AddEdge(currentVerticesNumbers[layerNumber], otherVerticesNumbers[layerNumber + 1], t);
                            }
                        }
                    }
                    if (column > 0)
                    {
                        for (int layerNumber = 0; layerNumber < otherVerticesNumbers.Length; layerNumber++)
                        {
                            otherVerticesNumbers[layerNumber] = GetVertexNumber(row, column - 1, layerNumber);
                        }
                        if (goodChars.Contains(maze[row, column - 1]))
                        {
                            for (int layerNumber = 0; layerNumber < currentVerticesNumbers.Length; layerNumber++)
                            {
                                g.AddEdge(currentVerticesNumbers[layerNumber], otherVerticesNumbers[layerNumber]);
                            }
                        }

                        if (maze[row, column - 1] == 'X' && t != 0)
                        {
                            for (int layerNumber = 0; layerNumber < k; layerNumber++)
                            {
                                g.AddEdge(currentVerticesNumbers[layerNumber], otherVerticesNumbers[layerNumber + 1], t);
                            }
                        }
                    }
                    if (column < totalColumns - 1)
                    {
                        for (int layerNumber = 0; layerNumber < otherVerticesNumbers.Length; layerNumber++)
                        {
                            otherVerticesNumbers[layerNumber] = GetVertexNumber(row, column + 1, layerNumber);
                        }
                        if (goodChars.Contains(maze[row, column + 1]))
                        {
                            for (int layerNumber = 0; layerNumber < currentVerticesNumbers.Length; layerNumber++)
                            {
                                g.AddEdge(currentVerticesNumbers[layerNumber], otherVerticesNumbers[layerNumber]);
                            }
                        }

                        if (maze[row, column + 1] == 'X' && t != 0)
                        {
                            for (int layerNumber = 0; layerNumber < k; layerNumber++)
                            {
                                g.AddEdge(currentVerticesNumbers[layerNumber], otherVerticesNumbers[layerNumber + 1], t);
                            }
                        }
                    }
                }
            }

            for (int layerNumber = 0; layerNumber < k; layerNumber++)
            {
                g.AddEdge(endVertices[layerNumber], endVertices[layerNumber + 1], 0);
                g.AddEdge(endVertices[layerNumber + 1], endVertices[layerNumber], 0);
            }
            bool result = g.DijkstraShortestPaths(startVerices[0], out PathsInfo[] pathsInfo);

            double length = pathsInfo[endVertices[k]].Dist;

            if (double.IsNaN(length))
            {
                path = "";
                return -1;
            }
            else
            {
                Edge[] pathEdges = PathsInfo.ConstructPath(startVerices[0], endVertices[k], pathsInfo);

                StringBuilder pathStringBuilder = new StringBuilder();
                foreach (var edge in pathEdges)
                {
                    char direction = ConvertEdgeToChar(edge);
                    if (direction != '*')
                        pathStringBuilder.Append(direction);
                }

                path = pathStringBuilder.ToString();
                return (int)length;
            }

            //Edge[] pathEdges = PathsInfo.ConstructPath(startVertex, endVertex, pathsInfo);

            //StringBuilder pathStringBuilder = new StringBuilder();
            //foreach (var edge in pathEdges)
            //{
            //    pathStringBuilder.Append(ConvertEdgeToChar(edge));
            //}

            //path = pathStringBuilder.ToString();
            //if (true)
            //{

            //}
            //return double.IsNaN(length) ? -1 : (int)length;

            int GetVertexNumber(int row, int column, int layerNumber)
            {
                return row * totalColumns + column + layerNumber * totalFields;
            }

            char ConvertEdgeToChar(Edge edge)
            {
                var startField = GetRowAndColumn(edge.From);
                var endField = GetRowAndColumn(edge.To);
                if (startField.row == endField.row)
                {
                    if (endField.column > startField.column)
                        return 'E';
                    else if (endField.column < startField.column)
                        return 'W';
                    else if (endField.column == startField.column)
                        return '*';
                }
                else if (startField.column == endField.column)
                {
                    if (endField.row > startField.row)
                        return 'S';
                    else if (endField.row < startField.row)
                        return 'N';
                }
                throw new ArgumentException("Incorrect edge");
            }

            Field GetRowAndColumn(int vertexNumber)
            {
                int layerNumber = vertexNumber / totalFields;
                int row = (vertexNumber - layerNumber * totalFields) / totalColumns;
                int column = (vertexNumber - layerNumber * totalFields) - row * totalColumns;
                return new Field(row, column);
            }
        }


        private struct Field
        {
            public Field(int row, int column)
            {
                this.row = row;
                this.column = column;
            }

            public int row;
            public int column;
        }
    }
}