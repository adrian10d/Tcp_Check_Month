using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace UsersManager
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

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


        public static List<User> GetUsers(string filepath)
        {
            var streamReader = File.OpenText("Pirates.csv");
            var reader = new CsvReader(streamReader);
            return reader.GetRecords<User>().ToList();
        }


    }


}