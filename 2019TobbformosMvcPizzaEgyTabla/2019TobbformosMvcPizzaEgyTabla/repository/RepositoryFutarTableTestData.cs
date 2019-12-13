using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobbbformosPizzaAlkalmazasEgyTabla.Repository;

namespace TobbbformosPizzaAlkalmazasEgyTabla.repository
{
    partial class RepositoryFutarDatabaseTable
    {
        public void fillFutarokWithTestDataFromSQLCommand()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                string query =
                    "INSERT INTO `pfutar` (`fazon`, `fnev`, `ftel`) VALUES " +
                            " (1, 'Sanyi', '+36205662233'), " +
                            " (2, 'PEti', '+36205662234'), " +
                            " (3, 'Kabát', '+36205662231'), " +
                            " (4, 'Ronaldo', '+36205662237'); ";
                
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
