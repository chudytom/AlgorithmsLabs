using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{
    public class WorkManager : MarshalByRefObject
    {
        /// <summary>
        /// Implementacja wersji 1
        /// W tablicy blocks zapisane s� wagi wszystkich blok�w do przypisania robotnikom.
        /// Ka�dy z nich powinien mie� przypisane bloki sumie wag r�wnej expectedBlockSum.
        /// Metoda zwraca tablic� przypisuj�c� ka�demu z blok�w jedn� z warto�ci:
        /// 1 - je�li blok zosta� przydzielony 1. robotnikowi
        /// 2 - je�li blok zosta� przydzielony 2. robotnikowi
        /// 0 - je�li blok nie zosta� przydzielony do �adnego robotnika
        /// Je�li wymaganego podzia�u nie da si� zrealizowa� metoda zwraca null.
        /// </summary>
        public int[] DivideWorkersWork(int[] blocks, int expectedBlockSum)
        {
            int[] resultBlocks = new int[blocks.Length];
            if (blocks == null)
                return null;

            int totalPossible = 0;
            for (int i = 0; i < blocks.Length; i++)
            {
                totalPossible += blocks[i];
            }

            if (blocks.Length == 0 || expectedBlockSum == 0)
                return resultBlocks;

            if (totalPossible == 0 )
            {
                resultBlocks[0] = 1;
                resultBlocks[1] = 2;
                return resultBlocks;
            }

            if (totalPossible < expectedBlockSum * 2)
                return null;

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

                if (newWorkerSum < expectedBlockSum)
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
        /// Parametry i wynik s� analogiczne do wersji 1.
        /// </summary>
        public int[] DivideWorkWithClosestBlocksCount(int[] blocks, int expectedBlockSum)
        {
            if (blocks == null)
                return null;

            int totalPossible = 0;
            for (int i = 0; i < blocks.Length; i++)
            {
                totalPossible += blocks[i];
            }

            if (blocks.Length == 0 || totalPossible == 0)
                return new int[0];

            if (totalPossible < expectedBlockSum * 2)
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
                    HashSet<int> failedValues = new HashSet<int>();
                    while (currentIndex < blocks.Length)
                    {
                        if (failedValues.Contains(blocks[currentIndex]))
                        {
                            currentIndex++;
                            continue;
                        }
                        bool isDivision1Possible = giveBlock(blockIndex: currentIndex, workerNumber: workerNumber, workerSum: newWorkerSum);
                        if (isDivision1Possible)
                            return true;
                        failedValues.Add(blocks[currentIndex]);
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
                        HashSet<int> failedValues = new HashSet<int>();
                        while (currentIndex < blocks.Length)
                        {
                            if (failedValues.Contains(blocks[currentIndex]))
                            {
                                currentIndex++;
                                continue;
                            }
                            bool isDivision2Possible = giveBlock(blockIndex: currentIndex, workerNumber: 2, workerSum: 0);
                            if (isDivision2Possible)
                                return true;
                            failedValues.Add(blocks[currentIndex]);
                            currentIndex++;
                        }
                    }
                }

                resultBlocks[blockIndex] = 0;
                return false;
            }
        }

        // Mo�na dopisywa� pola i metody pomocnicze

    }
}

