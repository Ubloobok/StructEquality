using System;
using BenchmarkDotNet.Running;

namespace StructEquality
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check that classes and structures are implemented correctly:
            Test.Assert();

            // Perform benchmarks:
            var summary = BenchmarkRunner.Run<DictionaryBenchmark>();
            Console.Read();
        }
    }
}
