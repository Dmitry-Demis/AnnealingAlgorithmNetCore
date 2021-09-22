using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnealingAlgorithm
{
    class Program
    {
        //КОНСТАНТЫ
        const int N = 10; //размер доски и число ферзей
        const double Tn = 30.0; //начальная температура
        const double Tk = 0.5; //конечная температура
        const double Alpha = 0.98; //скорость охлаждения
        const int ST = 100; //число итераций при смене T

        public struct TMember // решение
        {
            public int[] Plan;
            public int Energy;
            public TMember(int N)
            {
                Plan = new int[N];
                Energy = default;
            }
        }


        static void Main(string[] args)
        {
            TMember Current = new TMember(N); //текущее решение
            TMember Working = new TMember(N); //рабочее решение
            TMember Best = new TMember(N); //лучшее решение
            double T; //температура
            double Delta; //разница энергий
            double P; //вероятность допуска
            bool fNew; //флаг нового решения
            bool fBest; //флаг лучшего решения
            long Time; //этап поиска
            long Step; //шаг на этапе поиска
            long Accepted; //число новых решений
            T = Tn;
            fBest = false;
            Time = 0;
            Best.Energy = 100;
            New(ref Current);
            CalcEnergy(ref Current);
            Copy(ref Working, ref Current);
            Random random = new Random();
            while (T > Tk)
            {
                Accepted = 0;
                for (Step = 0; Step <= ST; Step++)
                {
                    fNew = false;
                    Swap(ref Working);
                    CalcEnergy(ref Working);
                    if (Working.Energy <= Current.Energy)
                    {
                        fNew = true;
                    }
                    else
                    {
                        Delta = Working.Energy - Current.Energy;
                        P = Math.Exp(-Delta / T);
                        var k = random.Next();
                        if (P > k % 2)
                        {
                            Accepted = Accepted + 1;
                            fNew = true;
                        }
                    }
                    if (fNew)
                    {
                        fNew = false;
                        Copy(ref Current, ref Working);
                        if (Current.Energy < Best.Energy)
                        {
                            Copy(ref Best, ref Current);
                            fBest = true;
                        }
                    }
                    else
                    {
                        Copy(ref Working, ref Current);
                    }
                    //  Console.WriteLine($"Best {Best.Energy}");
                }
                Console.WriteLine($"T = {T:f6}, Energy = {Best.Energy} ");
                T *= Alpha;
                Time++;
            }

            if (fBest)
            {
                Show(ref Best);
            }
            Console.ReadLine();
        }
        static void Swap(ref TMember M) //модификация решения
        {
            int x, y, v;
            Random random = new Random();
            x = random.Next(0, N);
            do
            {
                y = random.Next(0, N);

            } while (x == y);
            v = M.Plan[x];
            M.Plan[x] = M.Plan[y];
            M.Plan[y] = v;

        }
        static void New(ref TMember M)
        {
            for (int i = 0; i < N; i++)
            {
                M.Plan[i] = i;
            }
            for (int i = 0; i < N; i++)
            {
                Swap(ref M);
            }
        }

        static readonly int[] dx = new int[] { -1, 1, -1, 1 };
        static readonly int[] dy = new int[] { -1, 1, 1, -1 };
        static void CalcEnergy(ref TMember M)
        {


            int j, x, tx = 0, ty = 0, error = 0;
            error = 0;
            for (x = 0; x < N; x++)
            {
                for (j = 0; j < 4; j++)
                {
                    tx = x + dx[j];
                    ty = M.Plan[x] + dy[j];
                    while ((tx >= 0) && (tx < N) && (ty >= 0) && (ty < N))
                    {
                        if (M.Plan[tx] == ty)
                        {
                            error++;
                        }
                        tx = tx + dx[j];
                        ty = ty + dy[j];
                    }
                }
            }
            M.Energy = error;
        }
        static void Copy(ref TMember MD, ref TMember MS)
        {
            for (int i = 0; i < N; i++)
            {
                MD.Plan[i] = MS.Plan[i];
            }
            MD.Energy = MS.Energy;
        }

        static void Show(ref TMember M)
        {
            Console.WriteLine("Solve");
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (M.Plan[x] == y)
                    {
                        Console.Write("Q\t");
                    }
                    else
                    {
                        Console.Write("*\t");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
