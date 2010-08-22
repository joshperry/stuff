using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SportyGeek.Tests
{
    static class EnumerableAssert
    {
        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            IEnumerator<T> expectenum = expected.GetEnumerator();
            IEnumerator<T> actualenum = actual.GetEnumerator();

            bool expectmoved, actualmoved = false;
            while ((expectmoved = expectenum.MoveNext()) & (actualmoved = actualenum.MoveNext()))
            {
                Assert.AreEqual(expectenum.Current, actualenum.Current, "An item in the enumerables was different");
            }

            Assert.IsFalse(expectmoved || actualmoved, "The enumerables have a different number of items");
        }
    }
}
