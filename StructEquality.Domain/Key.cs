using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace StructEquality.Domain
{
    public static class KeyLong
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Make(int left, int right)
        {
            //implicit conversion of left to a long
            long res = left;

            //shift the bits creating an empty space on the right
            // ex: 0x0000CFFF becomes 0xCFFF0000
            res = (res << 32);

            //combine the bits on the right with the previous value
            // ex: 0xCFFF0000 | 0x0000ABCD becomes 0xCFFFABCD
            res = res | (long)(uint)right; //uint first to prevent loss of signed bit

            //return the combined result
            return res;
        }
    }

    public class KeyClass
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public KeyClass(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }

    public class KeyClassComparer : IEqualityComparer<KeyClass>
    {
        public bool Equals(KeyClass x, KeyClass y) =>
            (x == null && y == null) ? true :
            (x == null || y == null) ? false :
            (x.A == y.A && x.B == y.B && x.C == y.C);

        public int GetHashCode(KeyClass x)
        {
            //unchecked
            {
                var hashCode = -1872639489;
                hashCode = hashCode * -1521134295 + x.A.GetHashCode();
                hashCode = hashCode * -1521134295 + x.B.GetHashCode();
                hashCode = hashCode * -1521134295 + x.C.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    /// Here and below all structs must be marked as 'readonly' by default.
    /// </summary>
    public readonly struct KeyStruct
    {
        public readonly int A;
        public readonly int B;
        public readonly int C;

        public KeyStruct(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }

    public class KeyStructComparer : IEqualityComparer<KeyStruct>
    {
        public bool Equals(KeyStruct x, KeyStruct y) =>
            (x.A == y.A && x.B == y.B && x.C == y.C);

        public int GetHashCode(KeyStruct x)
        {
            //unchecked
            {
                var hashCode = -1872639489;
                hashCode = hashCode * -1521134295 + x.A.GetHashCode();
                hashCode = hashCode * -1521134295 + x.B.GetHashCode();
                hashCode = hashCode * -1521134295 + x.C.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    /// Example of the structure with mutable properties instead of readonly fields.
    /// </summary>
    public struct KeyStructProperties
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public KeyStructProperties(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }

    public class KeyStructPropertiesComparer : IEqualityComparer<KeyStructProperties>
    {
        public bool Equals(KeyStructProperties x, KeyStructProperties y) =>
            (x.A == y.A && x.B == y.B && x.C == y.C);

        public int GetHashCode(KeyStructProperties x)
        {
            //unchecked
            {
                var hashCode = -1872639489;
                hashCode = hashCode * -1521134295 + x.A.GetHashCode();
                hashCode = hashCode * -1521134295 + x.B.GetHashCode();
                hashCode = hashCode * -1521134295 + x.C.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    /// Tightly packed - will be compared byte-to-byte.
    /// In accordance with: Pro .NET Performance: Optimize Your C# Applications, page 86-87.
    /// Internal CanCompareBits is true if:
    /// 1) The value type contains only primitive types and does not override Equals.
    /// 2) The value type contains only value types for which (1) holds and does not override Equals.
    /// 3) The value type contains only value types for which (2) holds and does not override Equals.
    /// 
    /// Optimization is "on", because the struct is properly "packed".
    /// 
    /// Additionaly attributes StructLayout and FieldOffset can be used.
    /// Reference: https://www.developerfusion.com/article/84519/mastering-structs-in-c/
    /// </summary>
    public readonly struct KeyStructTightlyPacked
    {
        public readonly int A;
        public readonly int B;
        public readonly int C;

        public KeyStructTightlyPacked(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        // If override GetHashCode - It'll be already another implementation case.
        //public override int GetHashCode();
    }

    /// <summary>
    /// Not Tightly packed - will be compared field-to-field by reflection.
    /// </summary>
    public readonly struct KeyStructNotTightlyPacked
    {
        public readonly int A;
        public readonly double B;
        public readonly int C;

        public KeyStructNotTightlyPacked(int a, double b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        // If override GetHashCode - It'll be already another implementation case.
        //public override int GetHashCode();
    }

    public readonly struct KeyStructEquals
    {
        public readonly int A;
        public readonly int B;
        public readonly int C;

        public KeyStructEquals(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        // If override Equals(object obj) - it will produce boxing/unboxing.
        //public override bool Equals(object obj)

        public bool Equals(KeyStructEquals other)
          => other.A == A && other.B == B && other.C == C;

        public override int GetHashCode()
        {
            //unchecked
            {
                var hashCode = -1872639489;
                hashCode = hashCode * -1521134295 + A.GetHashCode();
                hashCode = hashCode * -1521134295 + B.GetHashCode();
                hashCode = hashCode * -1521134295 + C.GetHashCode();
                return hashCode;
            }
        }
    }

    public readonly struct KeyStructEquatableManual : IEquatable<KeyStructEquatableManual>
    {
        public readonly int A;
        public readonly int B;
        public readonly int C;

        public KeyStructEquatableManual(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        public bool Equals(KeyStructEquatableManual other) =>
            other.A == A && other.B == B && other.C == C;

        public override int GetHashCode()
        {
            //unchecked
            {
                var hashCode = -1872639489;
                hashCode = hashCode * -1521134295 + A.GetHashCode();
                hashCode = hashCode * -1521134295 + B.GetHashCode();
                hashCode = hashCode * -1521134295 + C.GetHashCode();
                return hashCode;
            }
        }
    }

    public struct KeyStructEquatableValueTuple : IEquatable<KeyStructEquatableValueTuple>
    {
        public readonly int A;
        public readonly int B;
        public readonly int C;

        public KeyStructEquatableValueTuple(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        public bool Equals(KeyStructEquatableValueTuple other) =>
            (A, B, C) == (other.A, other.B, other.C);

        public override int GetHashCode() =>
            (A, B, C).GetHashCode();
    }
}