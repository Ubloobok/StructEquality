# StructEquality

Performance comparison of different Dictionary usages in .NET/C#.

In addition to standard `Dictionary<TKey, TValue>`, also includes experimental:

- `EquatableDictionary<TKey, TValue>`, optimized for `TKey : IEquatable<TKey>`.
- `IntDictionary<TValue>`, optimized for `int` key.

TBD:

- HashSet's comparison.

# Benchmarks

## .NET Framework 4.8 (.NET Standard 2.0)

``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
  [Host]    : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  MediumRun : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT

Job=MediumRun  Jit=RyuJit  Platform=X64
IterationCount=15  LaunchCount=2  WarmupCount=10

```

### Dictionary, Set (this[key] = value)

|                                       Method |  Count |      Mean | Ratio |     Gen 0 |     Gen 1 |    Gen 2 | Allocated |
|--------------------------------------------- |------- |----------:|------:|----------:|----------:|---------:|----------:|
|                            NetDictionary_Int | 100000 |  2.745 ms |  1.00 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
|                           NetDictionary_Long | 100000 |  2.898 ms |  1.06 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
|               NetDictionary_KeyClassComparer | 100000 | 11.705 ms |  4.26 |  718.7500 |  390.6250 | 234.3750 |   5.96 MB |
|                      NetDictionary_KeyString | 100000 | 73.557 ms | 26.81 | 4714.2857 | 1428.5714 | 571.4286 |  28.27 MB |
|              NetDictionary_KeyStructComparer | 100000 |  4.595 ms |  1.67 |  320.3125 |  320.3125 | 320.3125 |   3.73 MB |
|    NetDictionary_KeyStructPropertiesComparer | 100000 |  4.654 ms |  1.69 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|          NetDictionary_KeyStructLostComparer | 100000 |  8.360 ms |  3.04 | 1062.5000 |  250.0000 | 250.0000 |   7.01 MB |
|         NetDictionary_KeyStructTightlyPacked | 100000 |  8.261 ms |  3.01 | 1062.5000 |  250.0000 | 250.0000 |   7.01 MB |
|      NetDictionary_KeyStructNotTightlyPacked | 100000 | 14.456 ms |  5.27 | 1687.5000 |  468.7500 | 468.7500 |   9.44 MB |
|                NetDictionary_KeyStructEquals | 100000 |  4.604 ms |  1.68 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|       NetDictionary_KeyStructEquatableManual | 100000 |  4.820 ms |  1.76 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|   NetDictionary_KeyStructEquatableValueTuple | 100000 |  6.672 ms |  2.43 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|  NetDictionary_KeyStructValueTupleDictionary | 100000 |  6.537 ms |  2.38 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|                            IntDictionary_Int | 100000 |  3.246 ms |  1.18 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
|                      EquatableDictionary_Int | 100000 |  3.223 ms |  1.17 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
| EquatableDictionary_KeyStructEquatableManual | 100000 |  4.091 ms |  1.49 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|               EquatableDictionary_ValueTuple | 100000 |  6.391 ms |  2.33 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |

### Dictionary, TryGet (TryGetValue(key))

|                                       Method |  Count |      Mean | Ratio |     Gen 0 | Gen 1 | Gen 2 |  Allocated |
|--------------------------------------------- |------- |----------:|------:|----------:|------:|------:|-----------:|
|                            NetDictionary_Int | 100000 |  2.253 ms |  1.00 |         - |     - |     - |          - |
|                           NetDictionary_Long | 100000 |  2.435 ms |  1.08 |         - |     - |     - |          - |
|               NetDictionary_KeyClassComparer | 100000 |  3.052 ms |  1.36 |  761.7188 |     - |     - |  3209451 B |
|                         NetDictionary_String | 100000 | 38.326 ms | 17.02 | 6285.7143 |     - |     - | 26478019 B |
|              NetDictionary_KeyStructComparer | 100000 |  3.916 ms |  1.74 |         - |     - |     - |          - |
|    NetDictionary_KeyStructPropertiesComparer | 100000 |  3.916 ms |  1.74 |         - |     - |     - |          - |
|          NetDictionary_KeyStructLostComparer | 100000 | 10.137 ms |  4.50 | 2343.7500 |     - |     - |  9862787 B |
|         NetDictionary_KeyStructTightlyPacked | 100000 | 12.098 ms |  5.39 | 2390.6250 |     - |     - | 10092038 B |
|      NetDictionary_KeyStructNotTightlyPacked | 100000 | 15.680 ms |  6.97 | 1484.3750 |     - |     - |  6232924 B |
|                NetDictionary_KeyStructEquals | 100000 |  4.316 ms |  1.94 |         - |     - |     - |      352 B |
|       NetDictionary_KeyStructEquatableManual | 100000 |  3.887 ms |  1.72 |         - |     - |     - |          - |
|   NetDictionary_KeyStructEquatableValueTuple | 100000 |  4.687 ms |  2.08 |         - |     - |     - |          - |
|                     NetDictionary_ValueTuple | 100000 |  4.303 ms |  1.91 |         - |     - |     - |          - |
|                            IntDictionary_Int | 100000 |  2.358 ms |  1.05 |         - |     - |     - |          - |
|                      EquatableDictionary_Int | 100000 |  2.360 ms |  1.05 |         - |     - |     - |          - |
| EquatableDictionary_KeyStructEquatableManual | 100000 |  2.648 ms |  1.17 |         - |     - |     - |          - |
|               EquatableDictionary_ValueTuple | 100000 |  4.695 ms |  2.08 |         - |     - |     - |          - |

## .NET Core 2.0

``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]    : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT
  MediumRun : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT

Job=MediumRun  Jit=RyuJit  Platform=X64
IterationCount=15  LaunchCount=2  WarmupCount=10

```

### Dictionary, Set (this[key] = value)

|                                       Method |  Count |      Mean | Ratio |     Gen 0 |     Gen 1 |    Gen 2 | Allocated |
|--------------------------------------------- |------- |----------:|------:|----------:|----------:|---------:|----------:|
|                            NetDictionary_Int | 100000 |  3.633 ms |  1.00 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
|                           NetDictionary_Long | 100000 |  3.687 ms |  1.01 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
|               NetDictionary_KeyClassComparer | 100000 | 12.359 ms |  3.41 |  671.8750 |  406.2500 | 203.1250 |   5.95 MB |
|                      NetDictionary_KeyString | 100000 | 75.986 ms | 20.91 | 4714.2857 | 1428.5714 | 571.4286 |  28.08 MB |
|              NetDictionary_KeyStructComparer | 100000 |  5.593 ms |  1.54 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|    NetDictionary_KeyStructPropertiesComparer | 100000 |  5.642 ms |  1.55 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|          NetDictionary_KeyStructLostComparer | 100000 | 10.057 ms |  2.77 | 1093.7500 |  281.2500 | 281.2500 |   7.02 MB |
|         NetDictionary_KeyStructTightlyPacked | 100000 |  9.919 ms |  2.73 | 1109.3750 |  296.8750 | 296.8750 |   7.01 MB |
|      NetDictionary_KeyStructNotTightlyPacked | 100000 | 16.895 ms |  4.65 | 1656.2500 |  437.5000 | 437.5000 |   9.45 MB |
|                NetDictionary_KeyStructEquals | 100000 |  5.565 ms |  1.53 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|       NetDictionary_KeyStructEquatableManual | 100000 |  5.626 ms |  1.55 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|   NetDictionary_KeyStructEquatableValueTuple | 100000 |  6.282 ms |  1.73 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|  NetDictionary_KeyStructValueTupleDictionary | 100000 |  5.925 ms |  1.63 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|                            IntDictionary_Int | 100000 |  3.430 ms |  0.94 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
|                      EquatableDictionary_Int | 100000 |  3.591 ms |  0.99 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
| EquatableDictionary_KeyStructEquatableManual | 100000 |  4.121 ms |  1.13 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|               EquatableDictionary_ValueTuple | 100000 |  5.751 ms |  1.58 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |

### Dictionary, TryGet (TryGetValue(key, out value))

|                                       Method |  Count |      Mean | Ratio |     Gen 0 | Gen 1 | Gen 2 |  Allocated |
|--------------------------------------------- |------- |----------:|------:|----------:|------:|------:|-----------:|
|                            NetDictionary_Int | 100000 |  2.255 ms |  1.00 |         - |     - |     - |          - |
|                           NetDictionary_Long | 100000 |  2.373 ms |  1.05 |         - |     - |     - |          - |
|               NetDictionary_KeyClassComparer | 100000 |  2.985 ms |  1.32 |  761.7188 |     - |     - |  3200000 B |
|                         NetDictionary_String | 100000 | 37.760 ms | 16.75 | 6250.0000 |     - |     - | 26400000 B |
|              NetDictionary_KeyStructComparer | 100000 |  3.913 ms |  1.74 |         - |     - |     - |          - |
|    NetDictionary_KeyStructPropertiesComparer | 100000 |  4.503 ms |  1.99 |         - |     - |     - |          - |
|          NetDictionary_KeyStructLostComparer | 100000 | 13.965 ms |  6.19 | 2343.7500 |     - |     - |  9842432 B |
|         NetDictionary_KeyStructTightlyPacked | 100000 | 11.059 ms |  4.90 | 2390.6250 |     - |     - | 10076288 B |
|      NetDictionary_KeyStructNotTightlyPacked | 100000 | 17.400 ms |  7.72 | 1468.7500 |     - |     - |  6254624 B |
|                NetDictionary_KeyStructEquals | 100000 |  3.907 ms |  1.73 |         - |     - |     - |      256 B |
|       NetDictionary_KeyStructEquatableManual | 100000 |  3.897 ms |  1.73 |         - |     - |     - |          - |
|   NetDictionary_KeyStructEquatableValueTuple | 100000 |  4.663 ms |  2.07 |         - |     - |     - |          - |
|                     NetDictionary_ValueTuple | 100000 |  4.279 ms |  1.90 |         - |     - |     - |          - |
|                            IntDictionary_Int | 100000 |  2.316 ms |  1.03 |         - |     - |     - |          - |
|                      EquatableDictionary_Int | 100000 |  2.327 ms |  1.03 |         - |     - |     - |          - |
| EquatableDictionary_KeyStructEquatableManual | 100000 |  2.587 ms |  1.15 |         - |     - |     - |          - |
|               EquatableDictionary_ValueTuple | 100000 |  4.595 ms |  2.04 |         - |     - |     - |          - |

