
using System;

namespace ASD
{

    class ChangeMaking
    {

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
            change = null;  // zmienić

            int?[,] tab = new int?[2, amount + 1];
            tab[0, 0] = 0;
            for (int current = 1; current < amount + 1; current++)
            {
                int min = int.MaxValue;
                int iMin = -1;
                for (int i = 0; i < coins.Length; i++)
                {
                    if (current - coins[i] < 0) continue;
                    int? temp = tab[0, current - coins[i]];
                    if (temp == null) continue;
                    if(temp<min)
                    {
                        iMin = i;
                        min = (int)temp; 
                    }
                }
                if (min != int.MaxValue)
                {
                    tab[0, current] = min +1;
                    tab[1, current] = iMin;
                }
            }
            if(tab[0,amount]!=null)
            {
                change = new int[coins.Length];
                int current = amount;
                for (int i = 0; i < tab[0,amount]; i++)
                {
                    change[(int)tab[1, current]]++;
                    current -= coins[(int)tab[1, current]];
                }
            }
            return tab[0,amount];      // zmienić
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
            change = null;  // zmienić
            return -1;      // zmienić
        }

    }

}
