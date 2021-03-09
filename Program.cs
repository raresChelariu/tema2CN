using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace tema2CN
{
    internal static class Program
    {
        private const string InputFile = "input.txt";

        private static readonly string InputPath =
            $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName}\{InputFile}";

        private static double[,] a, t, aInit;
        private static double[] d, b, y, x;
        private static double eps;
        private static int n;

        private static void Main()
        {
            Init();
            //PrintArray(rezolvareSEL(a, b));
        }
        
        private static double[] rezolvareSEL(double[,] a, double[] b)
        {
            a = DescompunereCholesky(a);
            return SubstitutieSuperior(Transpusa(a), SubstitutieInferior(a, b));
        }
        private static double[] Diferenta(double[] a, double[] b)
        {
            var res = new double[n];
            for (var i = 0; i < n; i++)
                res[i] = a[i] - b[i];
            return res;
        }
        private static double[] ProdusMatriceVector(double[,] a, double[] x)
        {
            var res = new double[n];
            for (var i = 0; i < n; i++)
            {
                res[i] = 0;
                for (var j = 0; j < n; j++)
                    res[i] += a[i, j] * x[j];
            }

            return res;
        }
        private static double VerificareSolutie()
        {
            return normaEuclidiana(Diferenta(ProdusMatriceVector(aInit, x), b));
        }

        private static double normaEuclidiana(double[] x)
        {
            double sum = 0;
            for (var i = 0; i < n; i++)
            {
                sum += x[i] * x[i];
            }

            return Math.Sqrt(sum);
        }
        private static void PrintArray(double[] v)
        {
            for (var i = 0; i < n; i++)
                Console.Write(v[i].ToString(CultureInfo.CurrentCulture) + ' ');
            Console.WriteLine();
        }

        private static double[,] Transpusa(double[,] X)
        {
            var res = new double[n,n];
            for (var i = 0; i < n; i++)
                for (var j = 0; j < n; j++)
                {
                    res[i, j] = X[j, i];
                }

            return res;
        }

        private static void PrintMatrix(double[,] m)
        {
            for (var i = 0; i < n; i++)
            {
                var line = "";
                for (var j = 0; j < n; j++)
                {
                    line += m[i, j].ToString() + ' ';
                }

                Console.WriteLine(line);
            }
        }

        private static double[] SubstitutieInferior(double[,] a, double[] b)
        {
            var x = new double[n];
            var sum = 0d;
            for (var i = 0; i < n; i++)
            {
                sum = 0d;
                for (var j = 0; j < i; j++)
                {
                    sum += a[i, j] * x[j];
                }
                x[i] = (b[i] - sum)/a[i, i];
            }
            return x;
        }

        private static double[] SubstitutieSuperior(double[,] a, double[] b)
        {
            var x = new double[n];
            for (var i = n-1; i >= 0; i--)
            {
                double sum = 0;
                for (var j = i + 1; j < n; j++)
                    sum += a[i, j] * x[j];
                x[i] = (b[i] - sum) / a[i, i];
            }

            return x;
        }
        private static double[,] DescompunereCholesky(double[,] a)
        {
            int n = (int)Math.Sqrt(a.Length);
 
            double[,] ret = new double[n, n];
            for (int r = 0; r < n; r++)
            for (int c = 0; c <= r; c++)
            {
                if (c == r)
                {
                    double sum = 0;
                    for (int j = 0; j < c; j++)
                    {
                        sum += ret[c, j] * ret[c, j];
                    }
                    ret[c, c] = Math.Sqrt(a[c, c] - sum);
                }
                else
                {
                    double sum = 0;
                    for (int j = 0; j < c; j++)
                        sum += ret[r, j] * ret[c, j];
                    ret[r, c] = 1.0 / ret[c, c] * (a[r, c] - sum);
                }
            }
            return ret;
        }

        private static bool MatriceValida()
        {
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (a[i, j] < 0 || a[i, j] != a[j, i])
                        return false;
                }
            }

            return true;
        }

        private static void Init()
        {
            var lines = File.ReadAllLines(InputPath);
            var tokens = lines[0].Split(' ');
            n = int.Parse(tokens[0]);
            eps = double.Parse(tokens[1]);

            a = new double[n, n];
            aInit = new double[n,n];
            d = new double[n];
            b = new double[n];

            for (var i = 0; i < n; i++)
            {
                tokens = lines[1 + i].Split(' ');
                for (var j = 0; j < n; j++)
                {
                    a[i, j] = double.Parse(tokens[j]);
                }
            }

            tokens = lines[n + 1].Split(' ');
            for (var i = 0; i < n; i++)
            {
                b[i] = double.Parse(tokens[i]);
            }

            for (var i = 0; i < n; i++)
            {
                d[i] = a[i, i];
            }

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    aInit[i, j] = a[i, j];
                }                
            }
            if (!MatriceValida())
            {
                Console.WriteLine(!MatriceValida() ? "Matricea nu este valida!" : "Input valid!");
            }
        }
    }
}