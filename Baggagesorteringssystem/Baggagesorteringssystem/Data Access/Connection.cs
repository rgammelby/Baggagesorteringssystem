using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace Baggagesorteringssystem.Data_Access
{
    class Connection
    {
        private string ConnectionString = "server=127.0.0.1;uid=root;database=airport_schedule";

        public MySqlConnection Connect()
        {
            var conn = new MySqlConnection(ConnectionString);

            //conn.Open();

            //Console.WriteLine("Connection opened. ");

            return conn;
        }

        public DataTable RetrieveData(string table)
        {
            MySqlConnection conn = Connect();
            conn.Open();

            MySqlDataAdapter sda = new MySqlDataAdapter($"SELECT * FROM airport_schedule.{table};", conn);
            DataTable dt = new DataTable();

            /*MySqlCommand cmd = new MySqlCommand()*/;

            sda.Fill(dt);

            conn.Close();

            return dt;
        }
    }
}
