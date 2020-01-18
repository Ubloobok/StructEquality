using System;
using BenchmarkDotNet.Running;
using StructEquality.Domain;

namespace StructEquality
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check that classes and structures are implemented correctly:
            Test.Assert();

            // Perform benchmarks:
            var benchmarks = new (string Name, Action Action)[]
            {
                ("DictionarySetBenchmark", () => BenchmarkRunner.Run<DictionarySetBenchmark>()),
                ("DictionaryTryGetBenchmark", () => BenchmarkRunner.Run<DictionaryTryGetBenchmark>()),
            };

            Console.WriteLine("Available benchmarks:");

            for (var i = 0; i < benchmarks.Length; i++)
            {
                Console.WriteLine($"  {i + 1}) {benchmarks[i].Name}");
            }

            Console.Write("Enter benchmark number to run: ");

            var enteredNumber = Console.ReadLine();

            var selectedBenchmark = benchmarks[int.Parse(enteredNumber) - 1];

            selectedBenchmark.Action();

            Console.Read();
        }
    }
}
