
using System;

namespace ASD
{

    class ChangeMaking
    {
        //Predicate
        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// bez ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to metochange = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości amount ( czyli rzędu o(amount) )
        /// </remarks>
        public static int? NoLimitsDynamic(int amount, int[] coins, out int[] change)
        {
            change = null;
            int[] values = new int[amount + 1];
            int[] usedCoinsIndices = new int[amount + 1];
            for (int i = 1; i < values.Length; i++)
            {
                values[i] = -1;
                usedCoinsIndices[i] = -1;
            }

            for (int currentAmount = 0; currentAmount < amount + 1; currentAmount++)
            {
                int minCoinsUsed = int.MaxValue;
                int usedCoinIndex = -1;
                for (int coinIndex = 0; coinIndex < coins.Length; coinIndex++)
                {
                    int diff = currentAmount - coins[coinIndex];
                    if (diff < 0 || values[diff] < 0)
                        continue;
                    int currentCoinsCount = values[diff] + 1;
                    if (currentCoinsCount < minCoinsUsed)
                    {
                        minCoinsUsed = currentCoinsCount;
                        usedCoinIndex = coinIndex;
                    }
                }
                if (minCoinsUsed < int.MaxValue)
                {
                    values[currentAmount] = minCoinsUsed;
                    usedCoinsIndices[currentAmount] = usedCoinIndex;
                }
            }

            if (values[amount] < 0)
                return null;
            else
            {
                change = new int[coins.Length];
                int currentAmount = amount;
                while (currentAmount > 0)
                {
                    int usedCoinIndex = usedCoinsIndices[currentAmount];
                    change[usedCoinIndex]++;
                    currentAmount -= coins[usedCoinIndex];
                }
                return values[amount];
            }
        }


        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// z uwzględnieniem ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="limits">Liczba dostępnych monet danego nomimału</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// limits[i] - dostepna liczba monet i-tego rodzaju (nominału)
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to change = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości iloczynu amount*(liczba rodzajów monet)
        /// ( czyli rzędu o(amount*(liczba rodzajów monet)) )
        /// </remarks>
        public static int? Dynamic(int amount, int[] coins, int[] limits, out int[] change)
        {
            change = null;
            int[] values = new int[amount + 1];
            int[] usedCoinIndices = new int[amount + 1];
            for (int i = 1; i < values.Length; i++)
            {
                values[i] = -1;
            }

            int[,] usedLimits = new int[coins.Length, amount + 1];

            for (int currentAmount = 0; currentAmount < amount + 1; currentAmount++)
            {
                int minCoinsUsed = int.MaxValue;
                int usedCoinIndex = -1;
                for (int coinIndex = 0; coinIndex < coins.Length; coinIndex++)
                {
                    int diff = currentAmount - coins[coinIndex];
                    if (diff < 0 || values[diff] < 0 || usedLimits[coinIndex, currentAmount] >= limits[coinIndex])
                        continue;
                    int currentCoinsCount = values[diff] + 1;
                    if (currentCoinsCount < minCoinsUsed)
                    {
                        minCoinsUsed = currentCoinsCount;
                        usedCoinIndex = coinIndex;
                    }
                }
                if (minCoinsUsed < int.MaxValue)
                {
                    values[currentAmount] = minCoinsUsed;
                    usedCoinIndices[currentAmount] = usedCoinIndex;
                    usedLimits[usedCoinIndex, currentAmount]++;
                }
            }


            if (values[amount] < 0)
                return null;
            else
            {
                change = new int[coins.Length];
                int currentAmount = amount;
                while (currentAmount > 0)
                {
                    int currentIndex = usedCoinIndices[currentAmount];
                    change[currentIndex]++;
                    currentAmount -= coins[currentIndex];
                }
                return values[amount];
            }
        }

    }

}
