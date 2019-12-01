# StructEquality

Description: TBD.

# Benchmarks

## .NET Framework 4.8

``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
  [Host]    : .NET Framework 4.8 (4.8.4042.0), X64 RyuJIT
  MediumRun : .NET Framework 4.8 (4.8.4042.0), X64 RyuJIT

Job=MediumRun  Jit=RyuJit  Platform=AnyCpu  
IterationCount=15  LaunchCount=2  WarmupCount=10  

```

### Dictionary.Add

|                           Method | Count |      Mean |     Error |    StdDev | Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------- |------ |----------:|----------:|----------:|------:|--------:|--------:|-------:|------:|----------:|
|                      KeyInt_Init |  1000 |  14.90 us |  0.885 us |  1.270 us |  1.00 |    0.00 |  5.2795 |      - |     - |   22247 B |
|                     KeyLong_Init |  1000 |  20.10 us |  0.986 us |  1.445 us |  1.36 |    0.18 |  7.3853 |      - |     - |   31071 B |
|            KeyClassComparer_Init |  1000 |  28.92 us |  1.679 us |  2.408 us |  1.96 |    0.25 | 15.0146 | 0.0305 |     - |   63172 B |
|                   KeyString_Init |  1000 | 435.44 us | 14.360 us | 20.595 us | 29.37 |    2.28 | 70.3125 | 0.4883 |     - |  295837 B |
|           KeyStructComparer_Init |  1000 |  29.75 us |  0.695 us |  0.975 us |  2.00 |    0.14 |  7.3853 |      - |     - |   31101 B |
| KeyStructPropertiesComparer_Init |  1000 |  31.00 us |  1.192 us |  1.785 us |  2.08 |    0.20 |  7.3853 |      - |     - |   31101 B |
|       KeyStructLostComparer_Init |  1000 |  74.47 us |  5.973 us |  8.755 us |  5.01 |    0.61 | 15.0146 | 0.1221 |     - |   63408 B |
|      KeyStructTightlyPacked_Init |  1000 |  67.87 us |  3.646 us |  5.457 us |  4.59 |    0.56 | 15.0146 | 0.1221 |     - |   63408 B |
|   KeyStructNotTightlyPacked_Init |  1000 | 110.09 us |  7.305 us | 10.477 us |  7.46 |    1.14 | 21.2402 |      - |     - |   89285 B |
|             KeyStructEquals_Init |  1000 |  30.62 us |  2.009 us |  2.945 us |  2.07 |    0.24 |  7.3853 |      - |     - |   31072 B |
|    KeyStructEquatableManual_Init |  1000 |  31.15 us |  1.682 us |  2.357 us |  2.10 |    0.27 |  7.3853 |      - |     - |   31071 B |
|     KeyStructEquatableTuple_Init |  1000 |  40.32 us |  1.745 us |  2.557 us |  2.73 |    0.33 |  7.3853 |      - |     - |   31072 B |
|    KeyStructTupleDictionary_Init |  1000 |  35.79 us |  0.193 us |  0.276 us |  2.42 |    0.20 |  9.3994 |      - |     - |   39903 B |

### Dictionary.Get

|                           Method | Count |      Mean |     Error |    StdDev | Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------- |------ |----------:|----------:|----------:|------:|--------:|--------:|-------:|------:|----------:|
|                       KeyInt_Get |  1000 |  17.44 us |  0.517 us |  0.741 us |  1.00 |    0.00 |       - |      - |     - |         - |
|                      KeyLong_Get |  1000 |  22.98 us |  4.052 us |  6.065 us |  1.29 |    0.33 |       - |      - |     - |         - |
|             KeyClassComparer_Get |  1000 |  25.93 us |  0.995 us |  1.489 us |  1.49 |    0.12 |  7.6294 |      - |     - |   32094 B |
|                    KeyString_Get |  1000 | 415.32 us |  2.819 us |  4.219 us | 23.87 |    1.06 | 62.9883 |      - |     - |  264779 B |
|            KeyStructComparer_Get |  1000 |  28.02 us |  0.074 us |  0.108 us |  1.61 |    0.06 |       - |      - |     - |         - |
|  KeyStructPropertiesComparer_Get |  1000 |  24.49 us |  0.066 us |  0.096 us |  1.41 |    0.06 |       - |      - |     - |         - |
|        KeyStructLostComparer_Get |  1000 |  96.03 us |  0.664 us |  0.994 us |  5.52 |    0.22 | 22.9492 |      - |     - |   96540 B |
|       KeyStructTightlyPacked_Get |  1000 |  93.87 us |  0.449 us |  0.643 us |  5.39 |    0.22 | 22.9492 |      - |     - |   96540 B |
|    KeyStructNotTightlyPacked_Get |  1000 | 557.27 us |  4.646 us |  6.954 us | 32.03 |    1.38 | 74.2188 |      - |     - |  313596 B |
|              KeyStructEquals_Get |  1000 |  60.09 us |  0.487 us |  0.729 us |  3.46 |    0.12 | 15.2588 |      - |     - |   64189 B |
|     KeyStructEquatableManual_Get |  1000 |  29.64 us |  1.031 us |  1.511 us |  1.70 |    0.09 |       - |      - |     - |         - |
|      KeyStructEquatableTuple_Get |  1000 |  40.21 us |  1.722 us |  2.577 us |  2.32 |    0.20 |       - |      - |     - |         - |
|     KeyStructTupleDictionary_Get |  1000 |  45.26 us |  1.100 us |  1.612 us |  2.60 |    0.14 |       - |      - |     - |         - |

## .NET Core 2.0

``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]    : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT
  MediumRun : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT

Job=MediumRun  Jit=RyuJit  Platform=AnyCpu  
IterationCount=15  LaunchCount=2  WarmupCount=10  

```

### Dictionary.Add

|                           Method | Count |      Mean |     Error |    StdDev | Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------- |------ |----------:|----------:|----------:|------:|--------:|--------:|-------:|------:|----------:|
|                      KeyInt_Init |  1000 |  15.07 us |  0.257 us |  0.369 us |  1.00 |    0.00 |  5.2795 |      - |     - |   22192 B |
|                     KeyLong_Init |  1000 |  17.35 us |  0.224 us |  0.328 us |  1.15 |    0.03 |  7.3242 |      - |     - |   31016 B |
|            KeyClassComparer_Init |  1000 |  26.61 us |  0.660 us |  0.967 us |  1.77 |    0.08 | 14.9841 | 0.0305 |     - |   63040 B |
|                   KeyString_Init |  1000 | 417.75 us | 37.588 us | 55.097 us | 27.84 |    3.99 | 70.3125 | 0.4883 |     - |  295016 B |
|           KeyStructComparer_Init |  1000 |  28.18 us |  0.292 us |  0.418 us |  1.87 |    0.05 |  7.3853 |      - |     - |   31040 B |
| KeyStructPropertiesComparer_Init |  1000 |  28.32 us |  0.230 us |  0.344 us |  1.88 |    0.06 |  7.3853 |      - |     - |   31040 B |
|       KeyStructLostComparer_Init |  1000 |  61.97 us |  0.468 us |  0.671 us |  4.12 |    0.10 | 15.0146 | 0.1221 |     - |   63336 B |
|      KeyStructTightlyPacked_Init |  1000 |  62.48 us |  0.453 us |  0.649 us |  4.15 |    0.10 | 15.0146 | 0.1221 |     - |   63464 B |
|   KeyStructNotTightlyPacked_Init |  1000 | 108.84 us |  0.628 us |  0.859 us |  7.23 |    0.18 | 21.2402 |      - |     - |   89575 B |
|             KeyStructEquals_Init |  1000 |  28.55 us |  0.426 us |  0.597 us |  1.90 |    0.07 |  7.3242 |      - |     - |   31016 B |
|    KeyStructEquatableManual_Init |  1000 |  28.81 us |  0.486 us |  0.697 us |  1.91 |    0.07 |  7.3242 |      - |     - |   31016 B |
|     KeyStructEquatableTuple_Init |  1000 |  37.95 us |  0.358 us |  0.536 us |  2.52 |    0.07 |  7.3242 |      - |     - |   31016 B |
|    KeyStructTupleDictionary_Init |  1000 |  33.53 us |  0.336 us |  0.470 us |  2.23 |    0.07 |  9.3994 |      - |     - |   39840 B |

### Dictionary.Get

|                           Method | Count |      Mean |     Error |    StdDev | Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------------------- |------ |----------:|----------:|----------:|------:|--------:|--------:|-------:|------:|----------:|
|                       KeyInt_Get |  1000 |  14.94 us |  0.081 us |  0.122 us |  1.00 |    0.00 |       - |      - |     - |         - |
|                      KeyLong_Get |  1000 |  16.22 us |  0.158 us |  0.236 us |  1.09 |    0.02 |       - |      - |     - |         - |
|             KeyClassComparer_Get |  1000 |  26.58 us |  1.143 us |  1.710 us |  1.78 |    0.12 |  7.5989 |      - |     - |   32000 B |
|                    KeyString_Get |  1000 | 395.29 us |  7.407 us | 10.857 us | 26.47 |    0.83 | 62.5000 |      - |     - |  264000 B |
|            KeyStructComparer_Get |  1000 |  27.13 us |  0.125 us |  0.180 us |  1.82 |    0.02 |       - |      - |     - |         - |
|  KeyStructPropertiesComparer_Get |  1000 |  23.96 us |  0.133 us |  0.200 us |  1.60 |    0.02 |       - |      - |     - |         - |
|        KeyStructLostComparer_Get |  1000 |  93.71 us |  0.580 us |  0.868 us |  6.27 |    0.08 | 22.8271 |      - |     - |   96256 B |
|       KeyStructTightlyPacked_Get |  1000 |  91.71 us |  0.277 us |  0.397 us |  6.14 |    0.06 | 22.8271 |      - |     - |   96192 B |
|    KeyStructNotTightlyPacked_Get |  1000 | 751.95 us |  7.710 us | 11.058 us | 50.33 |    0.71 | 74.2188 |      - |     - |  312674 B |
|              KeyStructEquals_Get |  1000 |  57.11 us |  0.450 us |  0.631 us |  3.82 |    0.04 | 15.1978 |      - |     - |   64000 B |
|     KeyStructEquatableManual_Get |  1000 |  28.44 us |  0.235 us |  0.337 us |  1.90 |    0.02 |       - |      - |     - |         - |
|      KeyStructEquatableTuple_Get |  1000 |  36.67 us |  0.255 us |  0.382 us |  2.45 |    0.03 |       - |      - |     - |         - |
|     KeyStructTupleDictionary_Get |  1000 |  39.52 us |  0.343 us |  0.503 us |  2.65 |    0.03 |       - |      - |     - |         - |
