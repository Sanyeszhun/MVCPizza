﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TobbformosPizzaAlkalmazasEgyTabla.Model;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace TobbformosPizzaAlkalmazasEgyTabla.Repository
{
    partial class RepositoryDatabaseTablePizza
    {
        

        public void fillFutarWithTestDataFromSQLCommand()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                string query =
                    "INSERT INTO `futar` (`fazon`, `fnev`, `par`) VALUES " +
                            " (1, 'Hapci', 'Szeged'), " +
                            " (2, 'Vidor', 'Hódmezővásárhely'), " +
                            " (3, 'Tudor', 'Sándorfalva'), " +
                            " (4, 'Vesuvio', 'Szatymaz'), " +
                            " (5, 'Sorrento', 'Debrecen'); ";
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
