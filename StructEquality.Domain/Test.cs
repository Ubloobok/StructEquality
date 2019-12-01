using System;
using System.Collections.Generic;
using System.Text;

namespace StructEquality
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

            Assert(new KeyStructEquatableTuple(1, 2, 3).Equals(new KeyStructEquatableTuple(1, 2, 3)));
            Assert(!new KeyStructEquatableTuple(3, 2, 1).Equals(new KeyStructEquatableTuple(1, 2, 3)));
        }

        public static void Assert(bool condition)
        {
            if (!condition)
                throw new ArgumentException();
        }
    }
}
