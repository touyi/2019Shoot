using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientWarp warp = new ClientWarp();
            warp.InitClient("127.0.0.1", 6666);
            Console.WriteLine(warp.ConnectServer());
            string con = Console.ReadLine();
            while (true)
            {
                DataItem item = warp.PopNextData();
                
                Console.WriteLine(item.protocol + "+" + item.buffer);
                Thread.Sleep(1000);
            }
        }
    }
}
