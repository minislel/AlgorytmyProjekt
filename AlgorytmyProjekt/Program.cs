
using System.Globalization;
using System.Text;

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
                        EuclidesMenu();
                        break;
                    case "2":
                        BisectionMenu();
                        break;
                    case "3":
                        ErasthotenesSieveMenu();
                        break;
                    case "4":
                        MonteCarloMenu();
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
        public static void EuclidesMenu()
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
        public static void BisectionMenu()
        {
            Console.WriteLine("Podaj wzór funkcji (np. x^2 - 4):");
            string formula = Console.ReadLine();
            Console.WriteLine("Podaj lewy koniec przedziału:");
            double left = double.Parse(Console.ReadLine());
            Console.WriteLine("Podaj prawy koniec przedziału:");
            double right = double.Parse(Console.ReadLine());
            Console.WriteLine("Podaj dokładność:");
            double accuracy = double.Parse(Console.ReadLine());
            double root = BisectionMethod(left, right, accuracy, formula);
            Console.WriteLine($"Pierwiastek równania w przedziale [{left}, {right}] to: {root}");
        }
        private static double BisectionMethod(double left, double right, double accuracy, string formula)
        {
            if (Function(left, formula) * Function(right, formula) >= 0)
            {
                Console.WriteLine("Funkcja nie zmienia znaku w podanym przedziale.");
                return double.NaN;
            }
            double middle = (left + right) / 2; 
            while (Math.Abs(right - left) >= accuracy)
            {
                middle = (left + right) / 2;
                if (Function(middle, formula) == 0)
                {
                    return middle;
                }
                else if (Function(left, formula) * Function(middle, formula) < 0)
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
        private static double Function(double x, string formula)
        {
            formula = formula.Replace("x", x.ToString(CultureInfo.InvariantCulture));
            return StringToFormula.Eval(formula);
        }

        #endregion
        #region Erastotenes Sieve
        public static void ErasthotenesSieveMenu()
        {
            Console.WriteLine("Podaj gorny zakres do znalezienia liczb pierwszych:");
            int n = int.Parse(Console.ReadLine());
            List<int> primes = EratosthenesSieve(n);
            Console.WriteLine($"Liczby pierwsze do {n}: {string.Join(", ", primes)}");
        }
        public static List<int> EratosthenesSieve(int n)
        {
            bool[] isPrime = new bool[n + 1];
            for (int i = 2; i <= n; i++)
            {
                isPrime[i] = true;
            }

            for (int i = 2; i * i <= n; i++)
            {
                if (isPrime[i])
                {
                    for (int j = i * i; j <= n; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            List<int> primes = new List<int>();
            for (int i = 2; i <= n; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }

            return primes;
        }
        #endregion
        #region Monte Carlo
        public static void MonteCarloMenu()
        {
            Console.WriteLine("Podaj macierz (np. 1,2,3;0,1,4;5,6,0):");
            string input = Console.ReadLine();
            double[,] matrix = ParseMatrix(input);
            Console.WriteLine("Podaj liczbę prób Monte Carlo:");
            int trials = int.Parse(Console.ReadLine());
            double estimate = MonteCarloDeterminant.EstimateDeterminantMonteCarlo(matrix, trials);
            Console.WriteLine($"Przybliżony wyznacznik (Monte Carlo): {estimate}");
            double exact = MonteCarloDeterminant.Determinant(matrix);
            Console.WriteLine($"Dokładny wyznacznik (rekurencyjnie): {exact}");
        }

        private static double[,] ParseMatrix(string input)
        {
            string[] rows = input.Split(';');
            int n = rows.Length;
            double[,] matrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                string[] values = rows[i].Split(',');
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = double.Parse(values[j], CultureInfo.InvariantCulture);
                }
            }
            return matrix;
        }
        #endregion

    }
    #region math formula parser
    public static class StringToFormula
    {
        private static readonly string[] operators = { "+", "-", "/", "%", "*", "^" };
        private static readonly Func<double, double, double>[] operations = {
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 % a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };

        public static bool TryEval(string expression, out double value)
        {
            try
            {
                value = Eval(expression);
                return true;
            }
            catch
            {
                value = 0.0;
                return false;
            }
        }

        public static double Eval(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return 0.0;

            if (double.TryParse(expression, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                return value;

            List<string> tokens = GetTokens(expression);
            tokens.Add("$"); // Append end of expression token
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count - 1)
            {
                string token = tokens[tokenIndex];
                string nextToken = tokens[tokenIndex + 1];

                switch (token)
                {
                    case "(":
                        {
                            string subExpr = GetSubExpression(tokens, ref tokenIndex);
                            operandStack.Push(Eval(subExpr));
                            continue;
                        }
                    case ")":
                        throw new ArgumentException("Mis-matched parentheses in expression");

                    // Handle unary ops
                    case "-":
                    case "+":
                        {
                            if (!IsOperator(nextToken) && operatorStack.Count == operandStack.Count)
                            {
                                operandStack.Push(double.Parse($"{token}{nextToken}", CultureInfo.InvariantCulture));
                                tokenIndex += 2;
                                continue;
                            }
                        }
                        break;
                }

                if (IsOperator(token))
                {
                    while (operatorStack.Count > 0 && OperatorPrecedence(token) <= OperatorPrecedence(operatorStack.Peek()))
                    {
                        if (!ResolveOperation())
                        {
                            throw new ArgumentException(BuildOpError());
                        }
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(double.Parse(token, CultureInfo.InvariantCulture));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                if (!ResolveOperation())
                    throw new ArgumentException(BuildOpError());
            }

            return operandStack.Pop();

            bool IsOperator(string token)
            {
                return Array.IndexOf(operators, token) >= 0;
            }
            int OperatorPrecedence(string op)
            {
                switch (op)
                {
                    case "^":
                        return 3;
                    case "*":
                    case "/":
                    case "%":
                        return 2;

                    case "+":
                    case "-":
                        return 1;
                    default:
                        return 0;
                }
            }

            string BuildOpError()
            {
                string op = operatorStack.Pop();
                string rhs = operandStack.Any() ? operandStack.Pop().ToString() : "null";
                string lhs = operandStack.Any() ? operandStack.Pop().ToString() : "null";
                return $"Operation not supported: {lhs} {op} {rhs}";
            }

            bool ResolveOperation()
            {
                if (operandStack.Count < 2)
                {
                    return false;
                }

                string op = operatorStack.Pop();
                double rhs = operandStack.Pop();
                double lhs = operandStack.Pop();
                operandStack.Push(operations[Array.IndexOf(operators, op)](lhs, rhs));
                Console.WriteLine($"Resolve {lhs} {op} {rhs} = {operandStack.Peek()}");
                return true;
            }
        }

        private static string GetSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                switch (token)
                {
                    case "(": parenlevels += 1; break;
                    case ")": parenlevels -= 1; break;
                }

                if (parenlevels > 0)
                    subExpr.Append(token);

                index += 1;
            }

            if (parenlevels > 0)
                throw new ArgumentException("Mis-matched parentheses in expression");

            return subExpr.ToString();
        }

        private static List<string> GetTokens(string expression)
        {
            string operators = "()^*/%+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
    }
    #endregion
    #region Monte Carlo Determinant
    public static class MonteCarloDeterminant
    {
        public static double EstimateDeterminantMonteCarlo(double[,] matrix, int trials)
        {
            int n = matrix.GetLength(0);
            Random rand = new Random();
            double sum = 0;

            for (int t = 0; t < trials; t++)
            {
                int[] perm = Enumerable.Range(0, n).OrderBy(_ => rand.Next()).ToArray();
                double product = 1;
                for (int i = 0; i < n; i++)
                {
                    product *= matrix[i, perm[i]];
                }

                int sign = PermutationSign(perm);
                sum += sign * product;
            }

            return sum / trials * Factorial(n);
        }

        public static int PermutationSign(int[] perm)
        {
            int sign = 1;
            bool[] visited = new bool[perm.Length];

            for (int i = 0; i < perm.Length; i++)
            {
                if (!visited[i])
                {
                    int cycleLength = 0;
                    int j = i;
                    while (!visited[j])
                    {
                        visited[j] = true;
                        j = perm[j];
                        cycleLength++;
                    }

                    if (cycleLength % 2 == 0)
                        sign *= -1;
                }
            }

            return sign;
        }

        public static double Determinant(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n == 1) return matrix[0, 0];

            double det = 0;
            for (int col = 0; col < n; col++)
            {
                double[,] minor = GetMinor(matrix, 0, col);
                det += Math.Pow(-1, col) * matrix[0, col] * Determinant(minor);
            }
            return det;
        }

        private static double[,] GetMinor(double[,] matrix, int rowToRemove, int colToRemove)
        {
            int n = matrix.GetLength(0);
            double[,] minor = new double[n - 1, n - 1];
            int mi = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == rowToRemove) continue;
                int mj = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == colToRemove) continue;
                    minor[mi, mj] = matrix[i, j];
                    mj++;
                }
                mi++;
            }
            return minor;
        }

        private static double Factorial(int n)
        {
            double result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }
    }

    #endregion
}
