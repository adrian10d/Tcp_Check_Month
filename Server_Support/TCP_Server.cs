using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_Support
{
    public abstract class TCP_Server
    {
        #region Fields
        IPAddress iPAddress;
        TcpListener tcpListener;
        int port_number;
        bool running;
        int buffer_size = 1024;
        TcpClient tcpClient;
        NetworkStream stream;
        #endregion
        #region Properties
        public IPAddress IPAddress { get => iPAddress; set { if (!running) iPAddress = value; else throw new Exception("Nie mozna zmienic adresu IP kiedy serwer jest uruchomiony"); } }
        public int Port
        {
            get => port_number; set

            {

                int tmp = port_number;

                if (!running) port_number = value; else throw new Exception("nie można zmienić portu kiedy serwer jest uruchomiony");

                if (!checkPort())

                {

                    port_number = tmp;

                    throw new Exception("błędna wartość portu");

                }

            }

        }
        public int Buffer_size
        {
            get => buffer_size; set

            {

                if (value < 0 || value > 1024 * 1024 * 64) throw new Exception("bledny rozmiar pakietu");

                if (!running) buffer_size = value; else throw new Exception("nie mozna zmienic rozmiaru pakietu kiedy serwer jest uruchomiony");

            }

        }
        protected TcpListener TcpListener { get => tcpListener; set => tcpListener = value; }

        protected TcpClient TcpClient { get => tcpClient; set => tcpClient = value; }

        protected NetworkStream Stream { get => stream; set => stream = value; }
        #endregion
        #region Constructors
        public TCP_Server(IPAddress IP, int port)
        {
            running = false;
            IPAddress = IP;
            Port = port;
            if (!checkPort())
            {
                Port = 8000;
                throw new Exception("Bledna wartosc portu, ustawiam port na 8000");
            }
        }
        #endregion
        #region Functions
        protected bool checkPort()

        {

            if (port_number < 1024 || port_number > 49151) return false;

            return true;

        }
        protected void StartListening()

        {

            TcpListener = new TcpListener(IPAddress, Port);

            TcpListener.Start();

        }
        protected abstract void AcceptClient();
        protected abstract void BeginDataTransmission(NetworkStream stream);
        public abstract void Start();
        #endregion
    }
}