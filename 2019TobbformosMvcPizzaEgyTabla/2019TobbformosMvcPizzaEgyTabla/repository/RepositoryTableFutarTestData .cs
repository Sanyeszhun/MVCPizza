using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TobbformosPizzaAlkalmazasEgyTabla.Model;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace TobbformosPizzaAlkalmazasEgyTabla.Repository
{
    partial class RepositoryFutarTableDatabase
    {
   

        public void fillFutarWithTestDataFromSQLCommand()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                string query =
                    "INSERT INTO `futar` (`fazon`, `fnev`, `fig`) VALUES " +
                            " (1, 'Mark', 100), " +
                            " (2, 'Andras', 20), " +
                            " (3, 'Jani', 30), " +
                            " (4, 'Pisti', 40), " +
                            " (5, 'Jeno', 50); ";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Tesztadatok feltöltése sikertelen.");
            }
        }
    }
}
