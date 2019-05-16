using System;
using System.Collections.Generic;
using System.Text;

namespace Common.UnitTest
{
    public class Program
    {
        public static void Main1()
        {
            var redisTest = new RedisTest();
            redisTest.Setup();
            redisTest.Redis_Function_Test();

            Console.ReadLine();
        }
    }
}
