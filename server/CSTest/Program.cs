using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DLLTest test = new DLLTest();
            Console.WriteLine(test.Add(5, 7));
        }
    }
}
