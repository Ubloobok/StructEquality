using System;
using System.Collections.Generic;
using System.Text;

namespace StructEquality.Domain
{
    public class Test
    {
        public static void Assert()
        {
            Assert(new KeyClassComparer().Equals(new KeyClass(1, 2, 3), new KeyClass(1, 2, 3)));
            Assert(!new KeyClassComparer().Equals(new KeyClass(3, 2, 1), new KeyClass(1, 2, 3)));

            Assert(new KeyStructComparer().Equals(new KeyStruct(1, 2, 3), new KeyStruct(1, 2, 3)));
            Assert(!new KeyStructComparer().Equals(new KeyStruct(3, 2, 1), new KeyStruct(1, 2, 3)));

            Assert(new KeyStructProperties(1, 2, 3).Equals(new KeyStructProperties(1, 2, 3)));
            Assert(!new KeyStructProperties(3, 2, 1).Equals(new KeyStructProperties(1, 2, 3)));

            Assert(new KeyStruct(1, 2, 3).Equals(new KeyStruct(1, 2, 3)));
            Assert(!new KeyStruct(3, 2, 1).Equals(new KeyStruct(1, 2, 3)));

            Assert(new KeyStructTightlyPacked(1, 2, 3).Equals(new KeyStructTightlyPacked(1, 2, 3)));
            Assert(!new KeyStructTightlyPacked(3, 2, 1).Equals(new KeyStructTightlyPacked(1, 2, 3)));

            Assert(new KeyStructNotTightlyPacked(1, 2, 3).Equals(new KeyStructNotTightlyPacked(1, 2, 3)));
            Assert(!new KeyStructNotTightlyPacked(3, 2, 1).Equals(new KeyStructNotTightlyPacked(1, 2, 3)));

            Assert(new KeyStructEquals(1, 2, 3).Equals(new KeyStructEquals(1, 2, 3)));
            Assert(!new KeyStructEquals(3, 2, 1).Equals(new KeyStructEquals(1, 2, 3)));

            Assert(new KeyStructEquatableManual(1, 2, 3).Equals(new KeyStructEquatableManual(1, 2, 3)));
            Assert(!new KeyStructEquatableManual(3, 2, 1).Equals(new KeyStructEquatableManual(1, 2, 3)));

            Assert(new KeyStructEquatableValueTuple(1, 2, 3).Equals(new KeyStructEquatableValueTuple(1, 2, 3)));
            Assert(!new KeyStructEquatableValueTuple(3, 2, 1).Equals(new KeyStructEquatableValueTuple(1, 2, 3)));

            var intDictionaryInt = new IntDictionary<int>();
            intDictionaryInt[1] = int.MaxValue;
            intDictionaryInt[2] = int.MaxValue;
            intDictionaryInt[3] = int.MaxValue;
            intDictionaryInt[1] = 100;
            intDictionaryInt[2] = 200;
            intDictionaryInt[3] = 300;
            Assert(intDictionaryInt[1] == 100);
            Assert(intDictionaryInt[2] == 200);
            Assert(intDictionaryInt[3] == 300);

            var equatableDictionaryInt = new EquatableDictionary<int, int>();
            equatableDictionaryInt[1] = int.MaxValue;
            equatableDictionaryInt[2] = int.MaxValue;
            equatableDictionaryInt[3] = int.MaxValue;
            equatableDictionaryInt[1] = 100;
            equatableDictionaryInt[2] = 200;
            equatableDictionaryInt[3] = 300;
            Assert(equatableDictionaryInt[1] == 100);
            Assert(equatableDictionaryInt[2] == 200);
            Assert(equatableDictionaryInt[3] == 300);

            var equatableDictionaryKey = new EquatableDictionary<KeyStructEquatableManual, int>();
            equatableDictionaryKey[new KeyStructEquatableManual(1, 1, 1)] = int.MaxValue;
            equatableDictionaryKey[new KeyStructEquatableManual(2, 2, 2)] = int.MaxValue;
            equatableDictionaryKey[new KeyStructEquatableManual(3, 3, 3)] = int.MaxValue;
            equatableDictionaryKey[new KeyStructEquatableManual(1, 1, 1)] = 100;
            equatableDictionaryKey[new KeyStructEquatableManual(2, 2, 2)] = 200;
            equatableDictionaryKey[new KeyStructEquatableManual(3, 3, 3)] = 300;
            Assert(equatableDictionaryKey[new KeyStructEquatableManual(1, 1, 1)] == 100);
            Assert(equatableDictionaryKey[new KeyStructEquatableManual(2, 2, 2)] == 200);
            Assert(equatableDictionaryKey[new KeyStructEquatableManual(3, 3, 3)] == 300);
        }

        public static void Assert(bool condition)
        {
            if (!condition)
                throw new ArgumentException();
        }
    }
}
