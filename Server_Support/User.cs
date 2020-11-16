using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Server_Support
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public static string filepath  = @"\users.csv";

        public static bool UserExists(string login)
        {
            bool exists = false;

            string[] lines = System.IO.File.ReadAllLines(path: filepath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                if (login == fields[1])
                {
                    exists = true;
                }
            }
            return exists;

        }

        public static void AddUser(string ID, string login, string password, string filepath)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine(ID + "," + login + "," + password);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Couldn't add the user ", e);
            }
        }

        public static bool Login(string login, string password, string filepath)
        {
            bool logged = false;

            string[] lines = System.IO.File.ReadAllLines(filepath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                if (login == fields[1] && password == fields[2])
                {
                    logged = true;
                }
            }
            return logged;


        }

    }
}