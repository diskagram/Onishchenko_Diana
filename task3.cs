using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Linq;



namespace task__1
{
    [TestFixture]
    class task3
    {
        static int DigitalRoot(long num)
        {
           var list = num.ToString().Select(ch => int.Parse(ch.ToString()));
            int sum = 0;
            foreach (var number in list) sum += number;
            if (sum > 9) {
                list1 = sum.ToString().Select(ch => int.Parse(ch.ToString()));
            int sum1 = 0;
            foreach (var number in list) sum1 += digit;
                return sum1;}
            return sum;
        }

        
        [Test]
        public void Test1()
        {
            int list = 13;
            var expected = 4;
            var result = DigitalRoot(list);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void Test2()
        {
            int list = 177;
            var expected = 6;
            var result = DigitalRoot(list);
            Assert.AreEqual(expected, result);


        }
        [Test]
        public void Test3()
        {
            int list = 40988;
            var expected = 2;
            var result = DigitalRoot(list);
            Assert.AreEqual(expected, result);

        }
    }
}
