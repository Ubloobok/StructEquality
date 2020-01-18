using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

using StructEquality.Domain;

namespace StructEquality
{
    [Config(typeof(Config))]
    [HtmlExporter]
    [MemoryDiagnoser]
    public class DictionarySetBenchmark
    {
        public class Config : ManualConfig
        {
            public Config() => Add(Job
                .MediumRun
                .With(Platform.X64)
                .With(Jit.RyuJit));
        }

        public int Min = 1_000_000;
        public int Max = 2_000_000;

        [Params(100_000)]
        public int Count;

        /// <summary>Inputs for key in dictionary.</summary>
        private (int A, int B, int C)[] _inputs;

        /// <summary>Just some object as value in dictionary.</summary>
        private object _value;

        #region Setup and Cleanup

        [GlobalSetup]
        public void GlobalSetup()
        {
            var rnd = new Random();
            _inputs = Enumerable.Range(0, Count)
                .Select(_ => (rnd.Next(Min, Max), rnd.Next(Min, Max), rnd.Next(Min, Max)))
                .ToArray();
            _value = new object();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _inputs = null;
            _value = null;
        }

        #endregion // Setup and Cleanup

        [Benchmark(Baseline = true)]
        public Dictionary<int, object> NetDictionary_Int()
        {
            // Ideal case: Domain int (ID for example) may be used as the key.
            var dict = new Dictionary<int, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[B] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<long, object> NetDictionary_Long()
        {
            // Long GetHashCode implementation:
            // return (unchecked((int)((long)m_value)) ^ (int)(m_value >> 32));
            var dict = new Dictionary<long, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[KeyLong.Make(A, B)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyClass, object> NetDictionary_KeyClassComparer()
        {
            var dict = new Dictionary<KeyClass, object>(Count, new KeyClassComparer());

            foreach (var (A, B, C) in _inputs)
            {
                // Order of items is changed to avoid JIT optimization with tuple variable assignment.
                dict[new KeyClass(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<string, object> NetDictionary_KeyString()
        {
            var dict = new Dictionary<string, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[$"{C}_{B}_{A}"] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyStruct, object> NetDictionary_KeyStructComparer()
        {
            var dict = new Dictionary<KeyStruct, object>(Count, new KeyStructComparer());

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStruct(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyStructProperties, object> NetDictionary_KeyStructPropertiesComparer()
        {
            var dict = new Dictionary<KeyStructProperties, object>(Count, new KeyStructPropertiesComparer());

            foreach (var (A, B, C) in _inputs)
            {
                dict[new KeyStructProperties(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyStruct, object> NetDictionary_KeyStructLostComparer()
        {
            var dict = new Dictionary<KeyStruct, object>(Count); // Forgot :( new KeyStructComparer()

            foreach (var (A, B, C) in _inputs)
            {
                dict[new KeyStruct(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyStructTightlyPacked, object> NetDictionary_KeyStructTightlyPacked()
        {
            var dict = new Dictionary<KeyStructTightlyPacked, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[new KeyStructTightlyPacked(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyStructNotTightlyPacked, object> NetDictionary_KeyStructNotTightlyPacked()
        {
            var dict = new Dictionary<KeyStructNotTightlyPacked, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[new KeyStructNotTightlyPacked(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyStructEquals, object> NetDictionary_KeyStructEquals()
        {
            var dict = new Dictionary<KeyStructEquals, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[new KeyStructEquals(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyStructEquatableManual, object> NetDictionary_KeyStructEquatableManual()
        {
            var dict = new Dictionary<KeyStructEquatableManual, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[new KeyStructEquatableManual(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<KeyStructEquatableValueTuple, object> NetDictionary_KeyStructEquatableValueTuple()
        {
            var dict = new Dictionary<KeyStructEquatableValueTuple, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                // Order of items is changed, to avoid JIT optimization with tuple variable assignment.
                dict[new KeyStructEquatableValueTuple(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public Dictionary<(int, int, int), object> NetDictionary_KeyStructValueTupleDictionary()
        {
            var dict = new Dictionary<(int, int, int), object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                // Order of items is changed, to avoid JIT optimization with tuple variable assignment.
                dict[(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public IntDictionary<object> IntDictionary_Int()
        {
            var dict = new IntDictionary<object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[B] = _value;
            }

            return dict;
        }

        [Benchmark]
        public EquatableDictionary<int, object> EquatableDictionary_Int()
        {
            var dict = new EquatableDictionary<int, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[B] = _value;
            }

            return dict;
        }

        [Benchmark]
        public EquatableDictionary<KeyStructEquatableManual, object> EquatableDictionary_KeyStructEquatableManual()
        {
            var dict = new EquatableDictionary<KeyStructEquatableManual, object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[new KeyStructEquatableManual(C, B, A)] = _value;
            }

            return dict;
        }

        [Benchmark]
        public EquatableDictionary<(int A, int B, int C), object> EquatableDictionary_ValueTuple()
        {
            var dict = new EquatableDictionary<(int A, int B, int C), object>(Count);

            foreach (var (A, B, C) in _inputs)
            {
                dict[(C, B, A)] = _value;
            }

            return dict;
        }
    }
}
