﻿using System;
using System.IO;
using System.Linq;
using LevelDBMCPE;

namespace LibraryTest
{
    class Program
    {
        class NullLogger : ILogger
        {
            public void Destructor()
            {
            }

            public void Logv(string format, Stream ap)
            {
            }
        }

        static void Main(string[] args)
        {
            if (File.Exists(Path.Combine(Path.GetTempPath(), "LevelDB-MCPE.dll")))
                File.Delete(Path.Combine(Path.GetTempPath(), "LevelDB-MCPE.dll"));

            Options options = Options.Create();
            options.ErrorIfExists = false;
            options.ParanoidChecks = false;
            options.CreateIfMissing = false;
            options.Compression = Compression.RawZlibCompression;
            options.SetCompression(Compression.ZlibCompression, 1);
            options.InfoLog = Logger.Create(new NullLogger());

            DB db = DB.Open(".\\db\\", options);

            ReadOptions readOptions = ReadOptions.Create();
            readOptions.VerifyChecksums = false;
            Snapshot snapshot = db.CreateSnapshot();
            readOptions.Snapshot = snapshot;
            readOptions.UseDecompressAllocator = true;

            Iterator iterator = db.CreateIterator(readOptions);

            for (iterator.SeekToFirst(); iterator.IsValid; iterator.Next())
            {
                byte[] key = iterator.Key;
                Console.Write(LevelDB.BytesToString(key));
                Console.WriteLine($" ({string.Join(" ", from byte val in key select val.ToString("X2"))})");
            }

            ulong[] sizes = db.ApproximateSizes(
                new string[]
                {
                    "Overworld",
                    "~local_player"
                },
                new string[]
                {
                    "mobevents",
                    "\xFF"
                });

            Console.WriteLine($"sizes = [{string.Join(", ", sizes ?? new ulong[0])}]");
            Console.WriteLine($"Overworld: {db.ApproximateSize("Overworld")} bytes");
            Console.WriteLine($"~local_player: {db.ApproximateSize("~local_player")} bytes");

            byte[] local_player = db.Get(LevelDB.StringToBytes("~local_player"), readOptions);
            HexDump(local_player);

            Console.ReadKey();

            db.Close();
        }

        public static void HexDump(byte[] array)
        {
            for (int i = 0; i < array.Length / 16; i++)
            {
                byte[] arr0 = array.Skip(i * 16).Take(8).ToArray();
                Console.Write(string.Join(" ", from byte val in arr0 select val.ToString("X2")));
                byte[] arr1 = array.Skip(i * 16 + 8).Take(8).ToArray();
                Console.Write("  " + string.Join(" ", from byte val in arr1 select val.ToString("X2")));
                Console.Write("  " + string.Join(" ", PrintableStringFromBytes(arr0)));
                Console.WriteLine("" + string.Join(" ", PrintableStringFromBytes(arr1)));
            }

            {
                int rest = array.Length % 16;
                byte[] arr0 = null;
                byte[] arr1 = null;
                if (rest != 0)
                {
                    arr0 = array.Skip(array.Length - array.Length % 16).Take(Math.Min(array.Length % 16, 8)).ToArray();
                    Console.Write(string.Join(" ", from byte val in arr0 select val.ToString("X2")));
                    Console.Write(new string(' ', (8 - Math.Min(array.Length % 16, 8)) * 3));

                    if (rest > 8)
                    {
                        arr1 = array.Skip(array.Length - array.Length % 16 + 8).Take(array.Length % 16 - 8).ToArray();
                        Console.Write("  " + string.Join(" ", from byte val in arr1 select val.ToString("X2")));
                        Console.Write(new string(' ', (16 - array.Length % 16) * 3));
                    }
                    else
                    {
                        Console.Write(new string(' ', 25));
                    }

                    Console.Write("  " + string.Join(" ", PrintableStringFromBytes(arr0)));

                    if (rest > 8)
                    {
                        Console.WriteLine("" + string.Join(" ", PrintableStringFromBytes(arr1)));
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
            }
        }

        public static string PrintableStringFromBytes(byte[] bytes)
        {
            string s = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] >= 32 && bytes[i] <= 126)
                    s += (char)bytes[i];
                else
                    s += '.';
            }
            return s;
        }
    }
}
