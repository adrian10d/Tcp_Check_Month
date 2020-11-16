using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server_Support;

namespace Tcp_Check_Month
{
    class Program
    {
        static void Main(string[] args)
        {
            TCP_Server_Asynch moj = new TCP_Server_Asynch(IPAddress.Parse("127.0.0.1"), 2048);
            moj.Start();
        }
    }
}
