using Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ripper
{
    public class Program : Metrics
    {
        static bool isRunning = true;
        static bool isLoaded = false;

        static readonly Metrics metrics = new Metrics();
        static readonly List<Task> tl = new List<Task>();

        static void Main(string[] args)
        {
            // Clear console.
            ClearConsole();

            // Setup console.
            SetupConsole(args.FirstOrDefault() == "clean");

            do
            {
                try
                {
                    if (isLoaded)
                    {
                        UpdateConsole();
                        RunBatch();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"There was an exception: {ex.ToString()}");
                }
            }
            while (isRunning == true);
        }

        private static List<string> Addresses = new List<string>();

        private static void SetupConsole(bool dedupe)
        {
            ClearConsole();

            var success = false;

            var path = $@"data\batch.ini";
            if (System.IO.File.Exists(path))
            {
                success = long.TryParse(System.IO.File.ReadAllText(path), out metrics.contenderCount);
            }

            if (success == false)
            {
                metrics.contenderCount = 0;

                // (Re)Create it.
                System.IO.Directory.CreateDirectory("data");
                System.IO.File.WriteAllText(path, metrics.contenderCount.ToString());
            }

            var a = System.IO.File.ReadAllLines("data/addresses.dat").ToList();
            if (dedupe)
            {
                a.ForEach(addr =>
                {
                    if (!Addresses.Contains(addr))
                    {
                        Addresses.Add(addr);
                    }
                });

                System.IO.File.WriteAllLines("data/addresses.cleaned.dat", Addresses.ToArray());
            }
            else
            {
                Addresses = a;
            }

            for (var i = 0; i < 10; i++)
            {
                Console.Beep(320, 100);
            }

            isLoaded = true;
        }

        static private void ClearConsole()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
        }

        static private void RunBatch()
        {
            tl.RemoveAll(t => t.IsCompleted == true);

            while (tl.Count < 10000)
            {
                if (metrics.contenderCount % 10000 == 0)
                {
                    var path = $@"data\batch.ini";
                    System.IO.File.WriteAllText(path, metrics.contenderCount.ToString());

                    ClearConsole();
                }

                if (metrics.contenderCount > 0)
                {
                    tl.Add(Task.Run(() => NewTry()));
                }

                metrics.contenderCount++;
            }

            var currentUpdate = DateTime.Now;
            if (currentUpdate > metrics.lastUpdate.AddSeconds(1))
            {
                metrics.keysPerSecond = metrics.previousCount;
                if (metrics.keysPerSecond > metrics.hightestRate)
                {
                    metrics.hightestRate = metrics.keysPerSecond;
                }
                metrics.lastUpdate = currentUpdate;
                metrics.previousCount = 0;
            }
        }

        static private async Task NewTry()
        {
            var privateKey = await PrivateKey.GeneratePrivateKey(PrivateKey.PrivateKeyType.Random);

            var publicCompressed = await PublicKey.GeneratePublicKey(privateKey, true);
            var btcCompressed = await PublicKey.GenerateBTCAddress(publicCompressed);

            var publicUncompressed = await PublicKey.GeneratePublicKey(privateKey, false);
            var btcUncompressed = await PublicKey.GenerateBTCAddress(publicUncompressed);

            metrics.previousCount++;

            var matchCompressed = Addresses.Contains(btcCompressed);
            var matchUncompressed = Addresses.Contains(btcUncompressed);
            if (matchCompressed || matchUncompressed)
            {
                for (var i = 0; i < 100; i++)
                {
                    Console.Beep(3200, 100);
                }

                var path = $@"wallets\{ btcCompressed }.txt";
                System.IO.Directory.CreateDirectory("wallets");
                System.IO.File.WriteAllText(path, $"{ btcCompressed }\r\n{ btcUncompressed }\r\n{ privateKey }");
            }
        }

        static private void UpdateConsole()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"");
            Console.WriteLine($"  ╔══════════════════════════════════════════════════════════════════════════╗ ");
            Console.WriteLine($"  ║  Intersect Testing Against Known Address List                            ║ ");
            Console.WriteLine($"  ║  Contenders: { metrics.contenderCount.ToString().PadRight(60)           }║ ");
            Console.WriteLine($"  ║  KPS:        { metrics.keysPerSecond.ToString().PadRight(60)            }║ ");
            Console.WriteLine($"  ║  Max KPS:    { metrics.hightestRate.ToString().PadRight(60)             }║ ");
            Console.WriteLine($"  ╚══════════════════════════════════════════════════════════════════════════╝ ");
            Console.WriteLine($"");
        }
    }
}
