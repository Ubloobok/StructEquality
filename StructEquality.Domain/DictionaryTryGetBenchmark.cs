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
    public class DictionaryTryGetBenchmark
    {
        public class Config : ManualConfig
        {
            public Config() => Add(Job
                .MediumRun
                .WithIterationCount(10) // Default: 15
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

        private Dictionary<int, object> _NetDictionary_Int;
        private Dictionary<long, object> _NetDictionary_Long;
        private Dictionary<KeyClass, object> _NetDictionary_KeyClassComparer;
        private Dictionary<string, object> _NetDictionary_String;
        private Dictionary<KeyStruct, object> _NetDictionary_KeyStructComparer;
        private Dictionary<KeyStructProperties, object> _NetDictionary_KeyStructPropertiesComparer;
        private Dictionary<KeyStruct, object> _NetDictionary_KeyStructLostComparer;
        private Dictionary<KeyStructTightlyPacked, object> _NetDictionary_KeyStructTightlyPacked;
        private Dictionary<KeyStructNotTightlyPacked, object> _NetDictionary_KeyStructNotTightlyPacked;
        private Dictionary<KeyStructEquals, object> _NetDictionary_KeyStructEquals;
        private Dictionary<KeyStructEquatableManual, object> _NetDictionary_KeyStructEquatableManual;
        private Dictionary<KeyStructEquatableValueTuple, object> _NetDictionary_KeyStructEquatableValueTuple;
        private Dictionary<(int A, int B, int C), object> _NetDictionary_ValueTuple;
        private IntDictionary<object> _IntDictionary_Int;
        private EquatableDictionary<int, object> _EquatableDictionary_Int;
        private EquatableDictionary<KeyStructEquatableManual, object> _EquatableDictionary_KeyStructEquatableManual;
        private EquatableDictionary<(int A, int B, int C), object> _EquatableDictionary_ValueTuple;

        #region Setup and Cleanup

        [GlobalSetup]
        public void GlobalSetup()
        {
            var rnd = new Random();
            _inputs = Enumerable.Range(0, Count)
                .Select(_ => (rnd.Next(Min, Max), rnd.Next(Min, Max), rnd.Next(Min, Max)))
                .ToArray();
            _value = new object();

            Dictionary<TKey, object> CreateDictionary<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer = null)
            {
                var dict = new Dictionary<TKey, object>(comparer);

                foreach (var item in source)
                {
                    dict[keySelector(item)] = _value;
                }

                return dict;
            }

            _NetDictionary_Int = CreateDictionary(_inputs, _ => _.A);
            _NetDictionary_Long = CreateDictionary(_inputs, _ => KeyLong.Make(_.A, _.B));
            _NetDictionary_KeyClassComparer = CreateDictionary(_inputs, _ => new KeyClass(_.A, _.B, _.C), new KeyClassComparer());
            _NetDictionary_String = CreateDictionary(_inputs, _ => $"{_.A}_{_.B}_{_.C}");
            _NetDictionary_KeyStructComparer = CreateDictionary(_inputs, _ => new KeyStruct(_.A, _.B, _.C), new KeyStructComparer());
            _NetDictionary_KeyStructPropertiesComparer = CreateDictionary(_inputs, _ => new KeyStructProperties(_.A, _.B, _.C), new KeyStructPropertiesComparer());
            _NetDictionary_KeyStructLostComparer = CreateDictionary(_inputs, _ => new KeyStruct(_.A, _.B, _.C)); // Forgot KeyStructComparer.
            _NetDictionary_KeyStructTightlyPacked = CreateDictionary(_inputs, _ => new KeyStructTightlyPacked(_.A, _.B, _.C));
            _NetDictionary_KeyStructNotTightlyPacked = CreateDictionary(_inputs, _ => new KeyStructNotTightlyPacked(_.A, _.B, _.C));
            _NetDictionary_KeyStructEquals = CreateDictionary(_inputs, _ => new KeyStructEquals(_.A, _.B, _.C));
            _NetDictionary_KeyStructEquatableManual = CreateDictionary(_inputs, _ => new KeyStructEquatableManual(_.A, _.B, _.C));
            _NetDictionary_KeyStructEquatableValueTuple = CreateDictionary(_inputs, _ => new KeyStructEquatableValueTuple(_.A, _.B, _.C));
            _NetDictionary_ValueTuple = CreateDictionary(_inputs, _ => (_.A, _.B, _.C));
            _IntDictionary_Int = new IntDictionary<object>(CreateDictionary(_inputs, _ => _.A));
            _EquatableDictionary_Int = new EquatableDictionary<int, object>(CreateDictionary(_inputs, _ => _.A));
            _EquatableDictionary_KeyStructEquatableManual = new EquatableDictionary<KeyStructEquatableManual, object>(CreateDictionary(_inputs, _ => new KeyStructEquatableManual(_.A, _.B, _.C)));
            _EquatableDictionary_ValueTuple = new EquatableDictionary<(int A, int B, int C), object>(CreateDictionary(_inputs, _ => (_.A, _.B, _.C)));
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _inputs = null;
            _NetDictionary_Int = null;
            _NetDictionary_Long = null;
            _NetDictionary_KeyClassComparer = null;
            _NetDictionary_String = null;
            _NetDictionary_KeyStructComparer = null;
            _NetDictionary_KeyStructPropertiesComparer = null;
            _NetDictionary_KeyStructLostComparer = null;
            _NetDictionary_KeyStructTightlyPacked = null;
            _NetDictionary_KeyStructNotTightlyPacked = null;
            _NetDictionary_KeyStructEquals = null;
            _NetDictionary_KeyStructEquatableManual = null;
            _NetDictionary_KeyStructEquatableValueTuple = null;
            _NetDictionary_ValueTuple = null;
            _EquatableDictionary_Int = null;
            _EquatableDictionary_KeyStructEquatableManual = null;
            _EquatableDictionary_ValueTuple = null;
        }

        #endregion // Setup and Cleanup

        [Benchmark(Baseline = true)]
        public object NetDictionary_Int()
        {
            // Ideal case: Domain int (ID for example) may be used as the key.
            var dict = _NetDictionary_Int;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(B, out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_Long()
        {
            var dict = _NetDictionary_Long;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(KeyLong.Make(A, B), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyClassComparer()
        {
            var dict = _NetDictionary_KeyClassComparer;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyClass(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_String()
        {
            var dict = _NetDictionary_String;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue($"{C}_{B}_{A}", out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyStructComparer()
        {
            var dict = _NetDictionary_KeyStructComparer;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStruct(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyStructPropertiesComparer()
        {
            var dict = _NetDictionary_KeyStructPropertiesComparer;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStructProperties(A, B, C), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyStructLostComparer()
        {
            var dict = _NetDictionary_KeyStructLostComparer;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStruct(A, B, C), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyStructTightlyPacked()
        {
            var dict = _NetDictionary_KeyStructTightlyPacked;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStructTightlyPacked(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyStructNotTightlyPacked()
        {
            var dict = _NetDictionary_KeyStructNotTightlyPacked;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStructNotTightlyPacked(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyStructEquals()
        {
            var dict = _NetDictionary_KeyStructEquals;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStructEquals(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyStructEquatableManual()
        {
            var dict = _NetDictionary_KeyStructEquatableManual;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStructEquatableManual(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_KeyStructEquatableValueTuple()
        {
            var dict = _NetDictionary_KeyStructEquatableValueTuple;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStructEquatableValueTuple(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object NetDictionary_ValueTuple()
        {
            var dict = _NetDictionary_ValueTuple;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue((C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object IntDictionary_Int()
        {
            // Ideal case: Domain int (ID for example) may be used as the key.
            var dict = _IntDictionary_Int;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(B, out result);
            }

            return result;
        }

        [Benchmark]
        public object EquatableDictionary_Int()
        {
            // Ideal case: Domain int (ID for example) may be used as the key.
            var dict = _EquatableDictionary_Int;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(B, out result);
            }

            return result;
        }

        [Benchmark]
        public object EquatableDictionary_KeyStructEquatableManual()
        {
            var dict = _EquatableDictionary_KeyStructEquatableManual;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue(new KeyStructEquatableManual(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        public object EquatableDictionary_ValueTuple()
        {
            var dict = _EquatableDictionary_ValueTuple;
            object result = null;

            foreach (var (A, B, C) in _inputs)
            {
                dict.TryGetValue((C, B, A), out result);
            }

            return result;
        }
    }
}
