using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TobbformosPizzaAlkalmazasEgyTabla.Model;
using MySql.Data.MySqlClient;
using System.Diagnostics;

using TobbformosPizzaAlkalmazasEgyTabla.model;

namespace TobbformosPizzaAlkalmazasEgyTabla.Repository
{
    partial class RepositoryFutarTableDatabase
    {
        private readonly string connectionStringCreate;
        private readonly string connectionString;

       
        public RepositoryFutarTableDatabase()
        {
            ConnectionString cs = new ConnectionString();
            connectionStringCreate = cs.getCreateString();
            connectionString = cs.getConnectionString();
        }

      
        public void createTableFutar()
        {
            string queryUSE = "USE csarp;";
            string queryCreateTable =
                "CREATE TABLE `futar` ( " +
                "   `fazon` int(3) NOT NULL DEFAULT '0', " +
                "   `fnev` varchar(30) COLLATE latin2_hungarian_ci NOT NULL DEFAULT '', " +
                "   `fig` varchar(30) COLLATE latin2_hungarian_ci NOT NULL DEFAULT '' " +
            ")ENGINE = InnoDB; ";
            string queryPrimaryKey =
                "ALTER TABLE `pvevo`  ADD PRIMARY KEY(`fazon`); ";

            MySqlConnection connection =
                new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmdUSE = new MySqlCommand(queryUSE, connection);
                cmdUSE.ExecuteNonQuery();
                MySqlCommand cmdCreateTable = new MySqlCommand(queryCreateTable, connection);
                cmdCreateTable.ExecuteNonQuery();
                MySqlCommand cmdPrimaryKey = new MySqlCommand(queryPrimaryKey, connection);
                cmdPrimaryKey.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Tábla lérehozása sikertelen.");
            }
        }

        /// <summary>
        /// pizza tábla törlése csarp adatbázisból
        /// </summary>
        public void deleteTableFutar()
        {
            string query =
                "USE csarp; " +
                "DROP TABLE futar;";

            MySqlConnection connection =
                new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Tábla törlése nem sikerült.");
            }
        }

        public void deleteDataFromFutarTable()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = Futar.Delet();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Tesztadatok törlése sikertelen.");
            }
        }
    }
}
