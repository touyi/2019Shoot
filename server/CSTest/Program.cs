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
            ClientWarp warp = new ClientWarp();
            warp.InitClient("127.0.0.1", 6666);
            Console.WriteLine(warp.ConnectServer());
            string con = Console.ReadLine();
            warp.SendData(123, con);
            Console.ReadLine();
        }
    }
}
