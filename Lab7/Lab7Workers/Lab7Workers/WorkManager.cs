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
            int worker1Sum = 0;
            int worker2Sum = 0;

            //bool[] usedBlocks = new bool[blocks.Length];
            int[] resultBlocks = new int[blocks.Length];

            bool result = false;
            for (int block = 0; block < blocks.Length; block++)
            {
                if (GiveBlockTo1(block, 0) == true)
                {
                    result = true;
                    break;
                }
            }

            if (result)
                return resultBlocks;
            else
                return null;


            bool GiveBlockTo1(int blockIndex, int workerSum)
            {
                resultBlocks[blockIndex] = 1;
                int currentSum = workerSum + blocks[blockIndex];
                if (currentSum > expectedBlockSum)
                {
                    resultBlocks[blockIndex] = 0;
                    return false;
                }
                if (currentSum == expectedBlockSum)
                {
                    for (int block2 = 0; block2 < blocks.Length; block2++)
                    {
                        if (resultBlocks[block2] != 0)
                            continue;
                        bool resultFrom2 = GiveBlockTo2(block2, 0);
                        if (resultFrom2 == true) return true;
                    }
                }
                if (currentSum < expectedBlockSum)
                {
                    for (int block = 0; block < blocks.Length; block++)
                    {
                        if (resultBlocks[block] != 0)
                            continue;
                        bool resultFrom1 = GiveBlockTo1(block, currentSum);
                        if (resultFrom1 == false)
                        {
                            int blockHelper = block;
                            while (blockHelper < blocks.Length - 1)
                            {
                                if (blocks[blockHelper] == blocks[blockHelper + 1])
                                    blockHelper++;
                                else
                                    break;
                            }
                            if(blockHelper!=block)
                            {
                                block = blockHelper;
                            }

                        }
                        if (resultFrom1 == true) return true;
                    }
                }
                resultBlocks[blockIndex] = 0;
                return false;

            }

            bool GiveBlockTo2(int blockIndex, int workerSum)
            {
                resultBlocks[blockIndex] = 2;
                int currentSum = workerSum + blocks[blockIndex];
                if (currentSum > expectedBlockSum)
                {
                    resultBlocks[blockIndex] = 0;
                    return false;
                }
                if (currentSum == expectedBlockSum)
                {
                    return true;
                }
                if (currentSum < expectedBlockSum)
                {
                    for (int block2Index = 0; block2Index < blocks.Length; block2Index++)
                    {
                        if (resultBlocks[block2Index] != 0)
                            continue;
                        bool resultFrom2 = GiveBlockTo2(block2Index, currentSum);
                        if (resultFrom2 == false)
                        {
                            int blockHelper = block2Index;
                            while (blockHelper < blocks.Length - 1)
                            {
                                if (blocks[blockHelper] == blocks[blockHelper + 1])
                                    blockHelper++;
                                else
                                    break;
                            }
                            if (blockHelper != block2Index)
                            {
                                block2Index = blockHelper;
                            }

                        }
                        if (resultFrom2 == true) return true;
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

            List<int[]> solutions = new List<int[]>();
            int worker1Sum = 0;
            int worker2Sum = 0;

            //bool[] usedBlocks = new bool[blocks.Length];
            int[] resultBlocks = new int[blocks.Length];

            bool result = false;
            for (int block = 0; block < blocks.Length; block++)
            {
                if (GiveBlockTo1(block, 0) == true)
                {
                    result = true;
                }
            }

            int[] bestSolution = new int[blocks.Length];
            int bestDifference = int.MaxValue;

            foreach (var solution in solutions)
            {
                int worker1 = 0;
                int worker2 = 0;

                for (int blockIndex = 0; blockIndex < blocks.Length; blockIndex++)
                {
                    if (solution[blockIndex] == 1)
                        worker1++;
                    if (solution[blockIndex] == 2)
                        worker2++;
                }
                int diff = Math.Abs(worker1 - worker2);
                if (diff < bestDifference)
                {
                    bestDifference = diff;
                    bestSolution = solution;
                }
            }



            if (solutions.Count == 0)
                return null;
            else
                return bestSolution;


            bool GiveBlockTo1(int blockIndex, int workerSum)
            {
                resultBlocks[blockIndex] = 1;
                int currentSum = workerSum + blocks[blockIndex];
                if (currentSum > expectedBlockSum)
                {
                    resultBlocks[blockIndex] = 0;
                    return false;
                }
                if (currentSum == expectedBlockSum)
                {
                    for (int block2 = 0; block2 < blocks.Length; block2++)
                    {
                        if (resultBlocks[block2] != 0)
                            continue;
                        bool resultFrom2 = GiveBlockTo2(block2, 0);
                        if (resultFrom2 == true) return true;
                    }
                }
                if (currentSum < expectedBlockSum)
                {
                    for (int block = 0; block < blocks.Length; block++)
                    {
                        if (resultBlocks[block] != 0)
                            continue;
                        bool resultFrom1 = GiveBlockTo1(block, currentSum);
                        if (resultFrom1 == true) return true;
                    }
                }
                resultBlocks[blockIndex] = 0;
                return false;

            }

            bool GiveBlockTo2(int blockIndex, int workerSum)
            {
                resultBlocks[blockIndex] = 2;
                int currentSum = workerSum + blocks[blockIndex];
                if (currentSum > expectedBlockSum)
                {
                    resultBlocks[blockIndex] = 0;
                    return false;
                }
                if (currentSum == expectedBlockSum)
                {
                    solutions.Add((int[])resultBlocks.Clone());
                    resultBlocks[blockIndex] = 0;
                    return false;
                }
                if (currentSum < expectedBlockSum)
                {
                    for (int block2Index = 0; block2Index < blocks.Length; block2Index++)
                    {
                        if (resultBlocks[block2Index] != 0)
                            continue;
                        bool resultFrom2 = GiveBlockTo2(block2Index, currentSum);
                        if (resultFrom2 == true) return true;
                    }
                }
                resultBlocks[blockIndex] = 0;
                return false;
            }
        }

        // Mo¿na dopisywaæ pola i metody pomocnicze

    }
}

