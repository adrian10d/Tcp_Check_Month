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
            int wybor = 0;
            while (true)
            {
                string welcome1 = "1. Chce sie zalogowac.\t2. Chce sie zarejestrowac.";
                string welcome2 = "Podaj wybor: ";
                byte[] bytes1_1 = Encoding.ASCII.GetBytes(welcome1);
                stream.Write(bytes1_1, 0, bytes1_1.Length);
                byte[] bytes1_2 = Encoding.ASCII.GetBytes(welcome2);
                stream.Write(bytes1_2, 0, bytes1_2.Length);
                byte[] buffer = new byte[Buffer_size];
                int wielkosc = stream.Read(buffer, 0, Buffer_size);
                string otrzymane = System.Text.Encoding.ASCII.GetString(buffer, 0, wielkosc);
                byte[] pusty = new byte[Buffer_size];
                stream.Read(pusty, 0, Buffer_size);
                if (Int32.TryParse(otrzymane, out wybor) && (wybor == 1 || wybor == 2))
                {
                    if (wybor == 1)
                        logowanie(stream);
                    else if (wybor == 2)
                        rejestrowanie(stream);
                }
                else
                {
                    string wyb = "Niepoprawny wybor.";
                    byte[] bytes1_3 = Encoding.ASCII.GetBytes(wyb);
                    stream.Write(bytes1_3, 0, bytes1_3.Length);
                }
            }
        }



        private void logowanie(NetworkStream stream)
        {
            bool czy_zalogowano = false;
            while (!czy_zalogowano)
            {
                string powitanie1 = "Podaj login: ";
                byte[] bytes1 = Encoding.ASCII.GetBytes(powitanie1);
                stream.Write(bytes1, 0, bytes1.Length);
                byte[] buffer1 = new byte[Buffer_size];
                int wielkosc1 = stream.Read(buffer1, 0, Buffer_size);
                string otrzymane1 = System.Text.Encoding.ASCII.GetString(buffer1, 0, wielkosc1);
                if (!User.UserExists(otrzymane1))
                {
                    string brak_loginu = "Nie istnieje taki login, zarejestruj sie \n";
                    byte[] zajetosc = Encoding.ASCII.GetBytes(brak_loginu);
                    stream.Write(zajetosc, 0, zajetosc.Length);
                }
                else
                {
                    string powitanie2 = "Podaj haslo: ";
                    byte[] bytes2 = Encoding.ASCII.GetBytes(powitanie2);
                    stream.Write(bytes2, 0, bytes2.Length);
                    byte[] pusty1 = new byte[Buffer_size];
                    stream.Read(pusty1, 0, Buffer_size);
                    byte[] buffer2 = new byte[Buffer_size];
                    int wielkosc2 = stream.Read(buffer2, 0, Buffer_size);
                    string otrzymane2 = System.Text.Encoding.ASCII.GetString(buffer2, 0, wielkosc2);
                    byte[] pusty2 = new byte[Buffer_size];
                    stream.Read(pusty2, 0, Buffer_size);
                    if (User.Login(otrzymane1, otrzymane2))
                    {
                        czy_zalogowano = true;
                        string zalogowano = "Udalo sie zalogowac.";
                        byte[] bytes_zal = Encoding.ASCII.GetBytes(zalogowano);
                        stream.Write(bytes_zal, 0, bytes_zal.Length);
                        gra(stream);
                    }


                    // else
                    // {
                    //     string niezalogowano = "Niepoprawny login lub haslo.\n";
                    //     byte[] bytes_zal = Encoding.ASCII.GetBytes(niezalogowano);
                    //     stream.Write(bytes_zal, 0, bytes_zal.Length);
                    //  }
                }
            }
        }
        private void rejestrowanie(NetworkStream stream)
        {
            string powitanie1 = "Podaj login: ";
            byte[] bytes1 = Encoding.ASCII.GetBytes(powitanie1);
            stream.Write(bytes1, 0, bytes1.Length);
            byte[] buffer1 = new byte[Buffer_size];
            int wielkosc1 = stream.Read(buffer1, 0, Buffer_size);
            string login = System.Text.Encoding.ASCII.GetString(buffer1, 0, wielkosc1);
            if (User.UserExists(login))
            {
                string nazwa_zajeta = "Ten login jest juz zajety \n";
                byte[] zajetosc = Encoding.ASCII.GetBytes(nazwa_zajeta);
                stream.Write(zajetosc, 0, zajetosc.Length);
            }
            else
            {
                string powitanie2 = "Podaj haslo: ";
                byte[] bytes2 = Encoding.ASCII.GetBytes(powitanie2);
                stream.Write(bytes2, 0, bytes2.Length);
                byte[] pusty1 = new byte[Buffer_size];
                stream.Read(pusty1, 0, Buffer_size);
                byte[] buffer2 = new byte[Buffer_size];
                int wielkosc2 = stream.Read(buffer2, 0, Buffer_size);
                string haslo = System.Text.Encoding.ASCII.GetString(buffer2, 0, wielkosc2);
                User.AddUser(login, haslo);
                byte[] pusty2 = new byte[Buffer_size];
                stream.Read(pusty2, 0, Buffer_size);
            }
        }
        private void gra(NetworkStream stream)
        {
            while (true)
            {
                try
                {
                    string powitanie = "Podaj numer miesiaca a otrzymasz jego nazwe(jesli chcesz zakonczyc gre to napisz \"koniec\"):";
                    byte[] bytes = Encoding.ASCII.GetBytes(powitanie);
                    stream.Write(bytes, 0, bytes.Length);
                    string odp;
                    byte[] buffer1 = new byte[Buffer_size];
                    int wielkosc1 = stream.Read(buffer1, 0, Buffer_size);
                    string otrzymane = System.Text.Encoding.ASCII.GetString(buffer1, 0, wielkosc1);
                    byte[] pusty1 = new byte[Buffer_size];
                    stream.Read(pusty1, 0, Buffer_size);
                    if (otrzymane == "koniec")
                        break;
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
                    else
                    {
                        odp = "Podano zly format.\n";
                    }
                    byte[] odp_bytes = Encoding.ASCII.GetBytes(odp);
                    stream.Write(odp_bytes, 0, odp_bytes.Length);
                }
                catch (IOException e)
                {

                    break;

                }
            }
        }
        public override void Start()
        {

            StartListening();

            //transmission starts within the accept function

            AcceptClient();

        }
    }
}