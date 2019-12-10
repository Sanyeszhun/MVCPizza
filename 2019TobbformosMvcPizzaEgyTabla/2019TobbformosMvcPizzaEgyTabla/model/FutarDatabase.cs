using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobbformosPizzaAlkalmazasEgyTabla.model
{
     partial class Futar
    {
        public string getInsert()
        {
            return
                "INSERT INTO `futar` (`fazon`, `fnev`, `fig`) " +
                "VALUES ('" +
                id +
                "', '" +
                getNeme() +
                "', '" +
                getIg() +
                "');";
        }

        public string getUpdate(int id)
        {
            return
                "UPDATE `futar` SET `fnev` = '" +
                getNeme() +
                "', `fig` = '" +
                getIg() +
                "' WHERE `futar`.`fazon` = " +
                id;
        }

        public static string getSQLCommandDeleteAllRecord()
        {
            return "DELETE FROM futar";
        }

        public static string getSQLCommandGetAllRecord()
        {
            return "SELECT * FROM futar";
        }
    }
}


