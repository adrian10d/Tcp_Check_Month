using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_Support
{
    public class TCP_Server_Asynch : TCP_Server
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);
        public TCP_Server_Asynch(IPAddress IP, int port) : base(IP, port)
        {

        }
        protected override void AcceptClient()

        {

            while (true)

            {

                TcpClient tcpClient = TcpListener.AcceptTcpClient();

                Stream = tcpClient.GetStream();

                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);

                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);

            }

        }
        private void TransmissionCallback(IAsyncResult ar)

        {

            // sprzątanie

        }
        protected override void BeginDataTransmission(NetworkStream stream)

        {
            /*string powitanie = "Podaj numer miesiaca a otrzymasz jego nazwe:";
            byte[] bytes = Encoding.ASCII.GetBytes(powitanie);
            stream.Write(bytes, 0, bytes.Length);
            while (true)

            {

                try

                {
                    string odp = "";
                    byte[] buffer = new byte[Buffer_size];
                    stream.Read(buffer, 0, Buffer_size);
                    string otrzymane = Encoding.ASCII.GetString(buffer);
                    if (Int32.TryParse(otrzymane, out int miesiac))
                    {
                        switch (miesiac)
                        {
                            case 1:
                                odp = "Styczen\n";
                                break;
                            case 2:
                                odp = "Luty\n";
                                break;
                            case 3:
                                odp = "Marzec\n";
                                break;
                            case 4:
                                odp = "Kwiecien\n";
                                break;
                            case 5:
                                odp = "Maj\n";
                                break;
                            case 6:
                                odp = "Czerwiec\n";
                                break;
                            case 7:
                                odp = "Lipiec\n";
                                break;
                            case 8:
                                odp = "Sierpien\n";
                                break;
                            case 9:
                                odp = "Wrzesien\n";
                                break;
                            case 10:
                                odp = "Pazdziernik\n";
                                break;
                            case 11:
                                odp = "Listopad\n";
                                break;
                            case 12:
                                odp = "Grudzien\n";
                                break;
                            default:
                                odp = "Nie ma takiego miesiaca.\n";
                                break;
                        }
                    }
                    else if (odp.Length > 1)
                    {
                        odp = "Podano zly format.\n";
                    }
                    if (odp.Length >= 1)
                    {
                        byte[] odp_bytes = Encoding.ASCII.GetBytes(odp);
                        //wysylanie informacji zwrotnej do klienta
                        stream.Write(odp_bytes, 0, odp_bytes.Length);
                    }
                }

                catch (IOException e)

                {

                    break;

                }

            }*/

        }
        public override void Start()

        {

            StartListening();

            //transmission starts within the accept function

            AcceptClient();

        }
    }
}
}
