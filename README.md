# StructEquality

Performance comparison of different Struct and Dictionary usages in .NET/C#.

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
IterationCount=10  LaunchCount=2  WarmupCount=10

```

### Dictionary.Set

`this[key] = value`

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
|  NetDictionary_ValueTuple | 100000 |  6.537 ms |  2.38 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|                            IntDictionary_Int | 100000 |  3.246 ms |  1.18 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
|                      EquatableDictionary_Int | 100000 |  3.223 ms |  1.17 |  328.1250 |  328.1250 | 328.1250 |    2.9 MB |
| EquatableDictionary_KeyStructEquatableManual | 100000 |  4.091 ms |  1.49 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |
|               EquatableDictionary_ValueTuple | 100000 |  6.391 ms |  2.33 |  328.1250 |  328.1250 | 328.1250 |   3.73 MB |

### Dictionary.TryGet

`TryGetValue(key, out value)`

|                                       Method |  Count |      Mean | Ratio |     Gen 0 | Gen 1 | Gen 2 |  Allocated |
|--------------------------------------------- |------- |----------:|------:|----------:|------:|------:|-----------:|
|                            NetDictionary_Int | 100000 |  2.337 ms |  1.00 |         - |     - |     - |          - |
|                           NetDictionary_Long | 100000 |  2.432 ms |  1.04 |         - |     - |     - |          - |
|               NetDictionary_KeyClassComparer | 100000 |  3.018 ms |  1.30 |  761.7188 |     - |     - |  3209451 B |
|                         NetDictionary_String | 100000 | 38.909 ms | 16.70 | 6307.6923 |     - |     - | 26478111 B |
|              NetDictionary_KeyStructComparer | 100000 |  4.180 ms |  1.80 |         - |     - |     - |          - |
|    NetDictionary_KeyStructPropertiesComparer | 100000 |  3.899 ms |  1.67 |         - |     - |     - |          - |
|          NetDictionary_KeyStructLostComparer | 100000 | 10.302 ms |  4.42 | 2343.7500 |     - |     - |  9869571 B |
|         NetDictionary_KeyStructTightlyPacked | 100000 | 11.317 ms |  4.85 | 2406.2500 |     - |     - | 10096519 B |
|      NetDictionary_KeyStructNotTightlyPacked | 100000 | 15.574 ms |  6.68 | 1484.3750 |     - |     - |  6271708 B |
|                NetDictionary_KeyStructEquals | 100000 |  3.922 ms |  1.68 |         - |     - |     - |      320 B |
|       NetDictionary_KeyStructEquatableManual | 100000 |  3.892 ms |  1.67 |         - |     - |     - |          - |
|   NetDictionary_KeyStructEquatableValueTuple | 100000 |  4.632 ms |  1.99 |         - |     - |     - |          - |
|                     NetDictionary_ValueTuple | 100000 |  4.316 ms |  1.85 |         - |     - |     - |          - |
|                            IntDictionary_Int | 100000 |  5.199 ms |  2.26 |         - |     - |     - |          - |
|                      EquatableDictionary_Int | 100000 |  3.586 ms |  1.56 |         - |     - |     - |          - |
| EquatableDictionary_KeyStructEquatableManual | 100000 |  2.622 ms |  1.13 |         - |     - |     - |          - |
|               EquatableDictionary_ValueTuple | 100000 |  4.679 ms |  2.01 |         - |     - |     - |          - |

## .NET Core 2.0

``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]    : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT
  MediumRun : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT

Job=MediumRun  Jit=RyuJit  Platform=X64
IterationCount=10  LaunchCount=2  WarmupCount=10

```

### Dictionary.Set

`this[key] = value`

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

### Dictionary.TryGet

`TryGetValue(key, out value)`

|                                       Method |  Count |      Mean | Ratio |     Gen 0 | Gen 1 | Gen 2 |  Allocated |
|--------------------------------------------- |------- |----------:|------:|----------:|------:|------:|-----------:|
|                            NetDictionary_Int | 100000 |  2.249 ms |  1.00 |         - |     - |     - |          - |
|                           NetDictionary_Long | 100000 |  2.358 ms |  1.05 |         - |     - |     - |          - |
|               NetDictionary_KeyClassComparer | 100000 |  3.055 ms |  1.36 |  761.7188 |     - |     - |  3200000 B |
|                         NetDictionary_String | 100000 | 36.838 ms | 16.38 | 6285.7143 |     - |     - | 26400000 B |
|              NetDictionary_KeyStructComparer | 100000 |  3.913 ms |  1.74 |         - |     - |     - |          - |
|    NetDictionary_KeyStructPropertiesComparer | 100000 |  3.883 ms |  1.73 |         - |     - |     - |          - |
|          NetDictionary_KeyStructLostComparer | 100000 | 13.596 ms |  6.82 | 2343.7500 |     - |     - |  9834240 B |
|         NetDictionary_KeyStructTightlyPacked | 100000 | 20.844 ms |  9.27 | 2375.0000 |     - |     - | 10062464 B |
|      NetDictionary_KeyStructNotTightlyPacked | 100000 | 17.077 ms |  7.60 | 1468.7500 |     - |     - |  6202700 B |
|                NetDictionary_KeyStructEquals | 100000 |  3.915 ms |  1.74 |         - |     - |     - |      192 B |
|       NetDictionary_KeyStructEquatableManual | 100000 |  3.911 ms |  1.74 |         - |     - |     - |          - |
|   NetDictionary_KeyStructEquatableValueTuple | 100000 |  4.698 ms |  2.09 |         - |     - |     - |          - |
|                     NetDictionary_ValueTuple | 100000 |  4.267 ms |  1.90 |         - |     - |     - |          - |
|                            IntDictionary_Int | 100000 |  2.328 ms |  1.04 |         - |     - |     - |          - |
|                      EquatableDictionary_Int | 100000 |  2.323 ms |  1.03 |         - |     - |     - |          - |
| EquatableDictionary_KeyStructEquatableManual | 100000 |  2.587 ms |  1.15 |         - |     - |     - |          - |
|               EquatableDictionary_ValueTuple | 100000 |  4.706 ms |  2.09 |         - |     - |     - |          - |

