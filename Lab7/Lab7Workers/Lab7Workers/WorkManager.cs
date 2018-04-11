using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{
    public class WorkManager : MarshalByRefObject
    {
        /// <summary>
        /// Implementacja wersji 1
        /// W tablicy blocks zapisane s¹ wagi wszystkich bloków do przypisania robotnikom.
        /// Ka¿dy z nich powinien mieæ przypisane bloki sumie wag równej expectedBlockSum.
        /// Metoda zwraca tablicê przypisuj¹c¹ ka¿demu z bloków jedn¹ z wartoœci:
        /// 1 - jeœli blok zosta³ przydzielony 1. robotnikowi
        /// 2 - jeœli blok zosta³ przydzielony 2. robotnikowi
        /// 0 - jeœli blok nie zosta³ przydzielony do ¿adnego robotnika
        /// Jeœli wymaganego podzia³u nie da siê zrealizowaæ metoda zwraca null.
        /// </summary>
        public int[] DivideWorkersWork(int[] blocks, int expectedBlockSum)
        {
            if (blocks == null || blocks.Length == 0)
                return null;
            int[] resultBlocks = new int[blocks.Length];
            bool divisionPossible = false;

            for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
            {
                divisionPossible = giveBlock(blockIndex: blockIndex, workerNumber: 1, workerSum: 0);
                if (divisionPossible)
                    break;
            }

            if (divisionPossible)
                return resultBlocks;
            else
                return null;


            bool giveBlock(int blockIndex, int workerNumber, int workerSum)
            {
                if (blockIndex >= blocks.Length || resultBlocks[blockIndex] != 0)
                {
                    return false;
                }
                resultBlocks[blockIndex] = workerNumber;
                int newWorkerSum = workerSum + blocks[blockIndex];
                if (newWorkerSum > expectedBlockSum)
                {
                    resultBlocks[blockIndex] = 0;
                    return false;
                }

                if(newWorkerSum < expectedBlockSum)
                {
                    int currentIndex = blockIndex + 1;
                    int previousIndex = -1;
                    while (currentIndex < blocks.Length)
                    {
                        bool isDivision1Possible = giveBlock(blockIndex: currentIndex, workerNumber: workerNumber, workerSum: newWorkerSum);
                        if (isDivision1Possible)
                            return true;
                        previousIndex = currentIndex;
                        currentIndex++;
                    }
                }

                else
                {
                    if (workerNumber == 2)
                    {
                        return true;
                    }
                    else
                    {
                        int currentIndex = 0;
                        int previousIndex = -1;
                        while (currentIndex < blocks.Length)
                        {
                            bool isDivision2Possible = giveBlock(blockIndex: currentIndex, workerNumber: 2, workerSum: 0);
                            if (isDivision2Possible)
                                return true;
                            previousIndex = currentIndex;
                            currentIndex++;
                        }
                    }
                }

                resultBlocks[blockIndex] = 0;
                return false;
            }
        }

        /// <summary>
        /// Implementacja wersji 2
        /// Parametry i wynik s¹ analogiczne do wersji 1.
        /// </summary>
        public int[] DivideWorkWithClosestBlocksCount(int[] blocks, int expectedBlockSum)
        {
            if (blocks == null || blocks.Length == 0)
                return null;

            int currentBestDiff = int.MaxValue;
            int[] bestSolution = new int[blocks.Length];
            //List<int[]> solutions = new List<int[]>();
            int[] resultBlocks = new int[blocks.Length];
            bool divisionPossible = false;

            for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
            {
                divisionPossible = giveBlock(blockIndex: blockIndex, workerNumber: 1, workerSum: 0);
                if (divisionPossible)
                    break;
            }

            //int[] bestSolution = new int[blocks.Length];
            //int bestDifference = int.MaxValue;

            //foreach (var solution in solutions)
            //{
            //    int worker1 = 0;
            //    int worker2 = 0;

            //    for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
            //    {
            //        if (solution[blockIndex] == 1)
            //            worker1++;
            //        if (solution[blockIndex] == 2)
            //            worker2++;
            //    }
            //    int diff = Math.Abs(worker1 - worker2);
            //    if (diff < bestDifference)
            //    {
            //        bestDifference = diff;
            //        bestSolution = solution;
            //    }
            //}

            if (currentBestDiff < int.MaxValue)
                return bestSolution;
            else
                return null;

            bool giveBlock(int blockIndex, int workerNumber, int workerSum)
            {
                if (blockIndex >= blocks.Length || resultBlocks[blockIndex] != 0)
                {
                    return false;
                }
                resultBlocks[blockIndex] = workerNumber;
                int newWorkerSum = workerSum + blocks[blockIndex];
                if (newWorkerSum > expectedBlockSum)
                {
                    resultBlocks[blockIndex] = 0;
                    return false;
                }

                if (newWorkerSum < expectedBlockSum)
                {
                    int currentIndex = blockIndex + 1;
                    while (currentIndex < blocks.Length)
                    {
                        bool isDivision1Possible = giveBlock(blockIndex: currentIndex, workerNumber: workerNumber, workerSum: newWorkerSum);
                        if (isDivision1Possible)
                            return true;
                        currentIndex++;
                    }
                }

                else
                {
                    if (workerNumber == 2)
                    {
                        

                        int worker1 = 0;
                        int worker2 = 0;

                        for (int index = 0; index < blocks.Length; index++)
                        {
                            if (resultBlocks[index] == 1)
                                worker1++;
                            if (resultBlocks[index] == 2)
                                worker2++;
                        }
                        int diff = Math.Abs(worker1 - worker2);
                        if (diff == 0)
                        {
                            bestSolution = (int[])resultBlocks.Clone();
                            currentBestDiff = diff;
                            return true;
                        }
                        if (diff < currentBestDiff)
                        {
                            currentBestDiff = diff;
                            bestSolution = (int[])resultBlocks.Clone();
                        }

                        resultBlocks[blockIndex] = 0;
                        return false;
                    }
                    else
                    {
                        int currentIndex = 0;
                        while (currentIndex < blocks.Length)
                        {
                            bool isDivision2Possible = giveBlock(blockIndex: currentIndex, workerNumber: 2, workerSum: 0);
                            if (isDivision2Possible)
                                return true;
                            currentIndex++;
                        }
                    }
                }

                resultBlocks[blockIndex] = 0;
                return false;
            }
        }

        // Mo¿na dopisywaæ pola i metody pomocnicze

    }
}

