﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server_Support
{
    public class TCP_Server_Asynch : TCP_Server
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);
        public int zmienna = 0;
        public string login;
        public List<string> lista_zalogowanych = new List<string>();
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
                byte[] buffer = new byte[Buffer_size];
                int wielkosc = stream.Read(buffer, 0, Buffer_size);
                string otrzymane = System.Text.Encoding.ASCII.GetString(buffer, 0, wielkosc);
                zmienna++;
                if (otrzymane[0] == '1')
                {
                    logowanie(stream);
                }
                else if (otrzymane == "2")
                {
                    rejestrowanie(stream);
                }
                else if (otrzymane == "klientkonczypolaczenie")
                    break;
            }
        }



        private void logowanie(NetworkStream stream)
        {
            byte[] buffer1 = new byte[Buffer_size];
            int wielkosc1 = stream.Read(buffer1, 0, Buffer_size);
            string otrzymane1 = System.Text.Encoding.ASCII.GetString(buffer1, 0, wielkosc1);
            byte[] buffer2 = new byte[Buffer_size];
            int wielkosc2 = stream.Read(buffer2, 0, Buffer_size);
            string otrzymane2 = System.Text.Encoding.ASCII.GetString(buffer2, 0, wielkosc2);
            if (!User.UserExists(otrzymane1))
            {
                string niezalogowano = "0";
                byte[] bytes_zal = Encoding.ASCII.GetBytes(niezalogowano);
                stream.Write(bytes_zal, 0, bytes_zal.Length);
            }
            else
            {
                if (User.Login(otrzymane1, otrzymane2) && (!lista_zalogowanych.Contains(otrzymane1)))
                {
                    lista_zalogowanych.Add(otrzymane1);
                    login = otrzymane1;
                    string zalogowano = "1";
                    byte[] bytes_zal = Encoding.ASCII.GetBytes(zalogowano);
                    stream.Write(bytes_zal, 0, bytes_zal.Length);
                    //gra(stream);
                    baza_filmow(stream);
                    lista_zalogowanych.Remove(otrzymane1);
                }
                else
                {
                    string niezalogowano = "0";
                    byte[] bytes_zal = Encoding.ASCII.GetBytes(niezalogowano);
                    stream.Write(bytes_zal, 0, bytes_zal.Length);
                }
            }
        }            
        private void rejestrowanie(NetworkStream stream)
        {
            byte[] buffer1 = new byte[Buffer_size];
            int wielkosc1 = stream.Read(buffer1, 0, Buffer_size);
            string login = System.Text.Encoding.ASCII.GetString(buffer1, 0, wielkosc1);
            byte[] buffer2 = new byte[Buffer_size];
            int wielkosc2 = stream.Read(buffer2, 0, Buffer_size);
            string haslo = System.Text.Encoding.ASCII.GetString(buffer2, 0, wielkosc2);
            if (User.UserExists(login))
            {
                string nazwa_zajeta = "0";
                byte[] zajetosc = Encoding.ASCII.GetBytes(nazwa_zajeta);
                stream.Write(zajetosc, 0, zajetosc.Length);
            }
            else
            {
                User.AddUser(login, haslo);
                string nazwa_zajeta = "1";
                byte[] zajetosc = Encoding.ASCII.GetBytes(nazwa_zajeta);
                stream.Write(zajetosc, 0, zajetosc.Length);
                string path = (Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Server_Support\\Client_Movies\\" + login + ".csv");
                var plik = File.Create(path);
                plik.Close();
            }
        }

        private void wyslij(NetworkStream stream, string wiadomosc)
        {
            byte[] msg = Encoding.ASCII.GetBytes(wiadomosc);
            stream.Write(msg, 0, msg.Length);
        }

        private string odbierz(NetworkStream stream)
        {
            byte[] buffer1 = new byte[Buffer_size];
            int wielkosc1 = stream.Read(buffer1, 0, Buffer_size);
            return System.Text.Encoding.ASCII.GetString(buffer1, 0, wielkosc1);
        }



        private void wyslij_baze_filmow(NetworkStream stream)
        {
            string filepath = (Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Server_Support\\movies.csv");
            StreamReader sr = new StreamReader(filepath);
            string naglowek = sr.ReadLine();
            wyslij(stream, naglowek);
            if(odbierz(stream)!= "1")
            {
                wyslij(stream, "blad");
            }
            else
            {
                while (!sr.EndOfStream)
                {
                    string wiersz = sr.ReadLine();
                    wyslij(stream, wiersz);
                    if (odbierz(stream) != "1")
                        break;
                }
                wyslij(stream, "endoffile");
            }


        }

        private void baza_filmow(NetworkStream stream)
        {
            string filepath = (Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Server_Support\\Client_Movies\\"+login+".csv");
            string[] lines = System.IO.File.ReadAllLines(path: filepath);
            //wyslij_baze_filmow(stream);
            for(int i = 0; i<lines.Length;i++)
            {
                byte[] msg = Encoding.ASCII.GetBytes(lines[i]);
                stream.Write(msg, 0, msg.Length);
                byte[] buffer = new byte[1024];
                int wielkosc = stream.Read(buffer, 0, 1024);
            }
            
            byte[] kon = Encoding.ASCII.GetBytes("endoffile");
            stream.Write(kon, 0, kon.Length);

            string otrzymane = "";
            while(otrzymane!="wyloguj00x015")
            {
                byte[] buffer1 = new byte[Buffer_size];
                int wielkosc1 = stream.Read(buffer1, 0, Buffer_size);
                otrzymane = System.Text.Encoding.ASCII.GetString(buffer1, 0, wielkosc1);
                if(otrzymane!="wyloguj00x015" && otrzymane!="ok133")
                {
                    try
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path: filepath, true))
                        {
                            file.WriteLine(otrzymane);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException("Couldn't add the movie into the base", e);
                    }
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