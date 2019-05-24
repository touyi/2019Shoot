using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            warp.InitClient("127.0.0.1", 7000);
            warp.ConnectServer();
            string str = "abc";
            IntPtr ptr = Marshal.StringToHGlobalAnsi(str);
            warp.SendData(1, ptr, 3);
            while (true)
            {
                DataItem item = warp.PopNextData();

                
                if (item.protocol != 0)
                {
                    Console.WriteLine(item.protocol);
                    break;
                }
            }
            
            Console.ReadLine();
        }
    }
}
