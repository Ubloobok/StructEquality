using System;
using System.Collections.Generic;
using System.Linq;
using KeyStructTupleDictionary = System.Collections.Generic.Dictionary<(int A, int B, int C), int>;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace StructEquality
{
    [Config(typeof(Config))]
    [HtmlExporter]
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory, BenchmarkLogicalGroupRule.ByParams)]
    public class DictionaryBenchmark
    {
        public class Config : ManualConfig
        {
            public Config() => Add(Job
                .MediumRun
                .With(Platform.AnyCpu)
                .With(Jit.RyuJit));
        }

        public int Min = 1_000_000;
        public int Max = 1_100_000;

        [Params(1_000)]
        public int Count;

        private (int A, int B, int C)[] _inputs;

        private Dictionary<int, int> KeyIntDict;
        private Dictionary<long, int> KeyLongDict;
        private Dictionary<KeyClass, int> KeyClassComparerDict;
        private Dictionary<string, int> KeyStringDict;
        private Dictionary<KeyStruct, int> KeyStructComparerDict;
        private Dictionary<KeyStructProperties, int> KeyStructPropertiesComparerDict;
        private Dictionary<KeyStruct, int> KeyStructLostComparerDict;
        private Dictionary<KeyStructTightlyPacked, int> KeyStructTightlyPackedDict;
        private Dictionary<KeyStructNotTightlyPacked, int> KeyStructNotTightlyPackedDict;
        private Dictionary<KeyStructEquals, int> KeyStructEqualsDict;
        private Dictionary<KeyStructEquatableManual, int> KeyStructEquatableManualDict;
        private Dictionary<KeyStructEquatableTuple, int> KeyStructEquatableTupleDict;
        private KeyStructTupleDictionary KeyStructTupleDictionaryDict;

        #region Setup and Cleanup

        [GlobalSetup(Targets = new[]
        {
            nameof(KeyInt_Init), nameof(KeyLong_Init), nameof(KeyClassComparer_Init), nameof(KeyString_Init),
            nameof(KeyStructComparer_Init), nameof(KeyStructPropertiesComparer_Init), nameof(KeyStructLostComparer_Init),
            nameof(KeyStructTightlyPacked_Init), nameof(KeyStructNotTightlyPacked_Init), nameof(KeyStructEquals_Init),
            nameof(KeyStructEquatableManual_Init), nameof(KeyStructEquatableTuple_Init), nameof(KeyStructTupleDictionary_Init)
        })]
        public void GlobalSetup_Init()
        {
            var rnd = new Random();
            _inputs = Enumerable.Range(0, Count)
                .Select(_ => (rnd.Next(Min, Max), rnd.Next(Min, Max), rnd.Next(Min, Max)))
                .ToArray();
        }

        [GlobalSetup]
        public void GlobalSetup_Get()
        {
            var rnd = new Random();
            _inputs = Enumerable.Range(0, Count)
                .Select(_ => (rnd.Next(Min, Max), rnd.Next(Min, Max), rnd.Next(Min, Max)))
                .ToArray();

            KeyIntDict = KeyInt_Init();
            KeyLongDict = KeyLong_Init();
            KeyClassComparerDict = KeyClassComparer_Init();
            KeyStringDict = KeyString_Init();
            KeyStructComparerDict = KeyStructComparer_Init();
            KeyStructPropertiesComparerDict = KeyStructPropertiesComparer_Init();
            KeyStructLostComparerDict = KeyStructLostComparer_Init();
            KeyStructTightlyPackedDict = KeyStructTightlyPacked_Init();
            KeyStructNotTightlyPackedDict = KeyStructNotTightlyPacked_Init();
            KeyStructEqualsDict = KeyStructEquals_Init();
            KeyStructEquatableManualDict = KeyStructEquatableManual_Init();
            KeyStructEquatableTupleDict = KeyStructEquatableTuple_Init();
            KeyStructTupleDictionaryDict = KeyStructTupleDictionary_Init();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _inputs = null;
            KeyIntDict = null;
            KeyLongDict = null;
            KeyClassComparerDict = null;
            KeyStringDict = null;
            KeyStructComparerDict = null;
            KeyStructPropertiesComparerDict = null;
            KeyStructLostComparerDict = null;
            KeyStructTightlyPackedDict = null;
            KeyStructNotTightlyPackedDict = null;
            KeyStructEqualsDict = null;
            KeyStructEquatableManualDict = null;
            KeyStructEquatableTupleDict = null;
            KeyStructTupleDictionaryDict = null;
        }

        #endregion // Setup and Cleanup

        #region Benchmark Category : Init

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Init")]
        public Dictionary<int, int> KeyInt_Init()
        {
            var dict = new Dictionary<int, int>(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[B] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<long, int> KeyLong_Init()
        {
            // Long GetHashCode implementation:
            // return (unchecked((int)((long)m_value)) ^ (int)(m_value >> 32));
            var dict = new Dictionary<long, int>(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[A << 32 | B] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyClass, int> KeyClassComparer_Init()
        {
            var dict = new Dictionary<KeyClass, int>(Count, new KeyClassComparer());

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                // Order of items is changed to avoid JIT optimization with tuple variable assignment.
                dict[new KeyClass(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<string, int> KeyString_Init()
        {
            var dict = new Dictionary<string, int>(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[$"{C}_{B}_{A}"] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyStruct, int> KeyStructComparer_Init()
        {
            var dict = new Dictionary<KeyStruct, int>(Count, new KeyStructComparer());

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStruct(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyStructProperties, int> KeyStructPropertiesComparer_Init()
        {
            var dict = new Dictionary<KeyStructProperties, int>(Count, new KeyStructPropertiesComparer());

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStructProperties(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyStruct, int> KeyStructLostComparer_Init()
        {
            var dict = new Dictionary<KeyStruct, int>(Count); // Forgot :( new KeyStructComparer()

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStruct(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyStructTightlyPacked, int> KeyStructTightlyPacked_Init()
        {
            var dict = new Dictionary<KeyStructTightlyPacked, int>(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStructTightlyPacked(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyStructNotTightlyPacked, int> KeyStructNotTightlyPacked_Init()
        {
            var dict = new Dictionary<KeyStructNotTightlyPacked, int>(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStructNotTightlyPacked(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyStructEquals, int> KeyStructEquals_Init()
        {
            var dict = new Dictionary<KeyStructEquals, int>(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStructEquals(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyStructEquatableManual, int> KeyStructEquatableManual_Init()
        {
            var dict = new Dictionary<KeyStructEquatableManual, int>(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStructEquatableManual(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<KeyStructEquatableTuple, int> KeyStructEquatableTuple_Init()
        {
            var dict = new Dictionary<KeyStructEquatableTuple, int>(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict[new KeyStructEquatableTuple(C, B, A)] = i;
            }

            return dict;
        }

        [Benchmark]
        [BenchmarkCategory("Init")]
        public Dictionary<(int, int, int), int> KeyStructTupleDictionary_Init()
        {
            var dict = new KeyStructTupleDictionary(Count);

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                // Order of items is changed, to avoit JIT optimization with tuple variable assignment.
                dict[(C, B, A)] = i;
            }

            return dict;
        }

        #endregion // Benchmark Category : Init

        #region Benchmark Category : Get

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Get")]
        public int KeyInt_Get()
        {
            var dict = KeyIntDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(B, out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyLong_Get()
        {
            var dict = KeyLongDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(A << 32 | B, out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyClassComparer_Get()
        {
            var dict = KeyClassComparerDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyClass(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyString_Get()
        {
            var dict = KeyStringDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue($"{C}_{B}_{A}", out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructComparer_Get()
        {
            var dict = KeyStructComparerDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyStruct(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructPropertiesComparer_Get()
        {
            var dict = KeyStructPropertiesComparerDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyStructProperties(A, B, C), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructLostComparer_Get()
        {
            var dict = KeyStructLostComparerDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyStruct(A, B, C), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructTightlyPacked_Get()
        {
            var dict = KeyStructTightlyPackedDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyStructTightlyPacked(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructNotTightlyPacked_Get()
        {
            var dict = KeyStructNotTightlyPackedDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyStructNotTightlyPacked(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructEquals_Get()
        {
            var dict = KeyStructEqualsDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyStructEquals(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructEquatableManual_Get()
        {
            var dict = KeyStructEquatableManualDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyStructEquatableManual(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructEquatableTuple_Get()
        {
            var dict = KeyStructEquatableTupleDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue(new KeyStructEquatableTuple(C, B, A), out result);
            }

            return result;
        }

        [Benchmark]
        [BenchmarkCategory("Get")]
        public int KeyStructTupleDictionary_Get()
        {
            var dict = KeyStructTupleDictionaryDict;
            int result = default;

            for (var i = 0; i < Count; i++)
            {
                var (A, B, C) = _inputs[i];
                dict.TryGetValue((C, B, A), out result);
            }

            return result;
        }

        #endregion // Benchmark Category : Get
    }
}
