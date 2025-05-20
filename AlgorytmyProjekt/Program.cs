
namespace AlgorytmyProjekt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Wybierz algorytm do uruchomienia:");
                Console.WriteLine("1. Algorytm Euklidesa");
                Console.WriteLine("2. Bisekcja");
                Console.WriteLine("3. Sito Erastotenesa");
                Console.WriteLine("4. Wyznacznik macierzy");
                Console.WriteLine("5. Zakończ program");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Euclides();
                        break;
                    case "2":
                        Bisection();
                        break;
                    case "3":
                        KruskalAlgorithm.Run();
                        break;
                    case "4":
                        PrimAlgorithm.Run();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                        break;
                }
            }
        }
        #region Euclides
        public static void Euclides()
        {
            Console.WriteLine("Podaj pierwszą liczbę:");
            int a = int.Parse(Console.ReadLine());
            Console.WriteLine("Podaj drugą liczbę:");
            int b = int.Parse(Console.ReadLine());
            int result = GCD(a, b);
            Console.WriteLine($"Największy wspólny dzielnik {a} i {b} to: {result}");
        }

        private static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        #endregion
        #region Bisection
        public static void Bisection()
        {
            Console.WriteLine("Podaj lewy koniec przedziału:");
            double left = double.Parse(Console.ReadLine());
            Console.WriteLine("Podaj prawy koniec przedziału:");
            double right = double.Parse(Console.ReadLine());
            Console.WriteLine("Podaj dokładność:");
            double accuracy = double.Parse(Console.ReadLine());
            double root = BisectionMethod(left, right, accuracy);
            Console.WriteLine($"Pierwiastek równania w przedziale [{left}, {right}] to: {root}");
        }
        private static double BisectionMethod(double left, double right, double accuracy)
        {
            if (Function(left) * Function(right) >= 0)
            {
                Console.WriteLine("Funkcja nie zmienia znaku w podanym przedziale.");
                return double.NaN;
            }
            double middle = (left + right) / 2;

            while (Math.Abs(right - left) >= accuracy)
            {
                if (Function(middle) == 0)
                {
                    return middle;
                }
                else if (Function(left) * Function(middle) < 0)
                {
                    right = middle;
                }
                else
                {
                    left = middle;
                }
            }
            return middle;
        }
        private static double Function(double x)
        {
            return x * x - 4; // Przykładowa funkcja: f(x) = x^2 - 4
        }
    }
}
