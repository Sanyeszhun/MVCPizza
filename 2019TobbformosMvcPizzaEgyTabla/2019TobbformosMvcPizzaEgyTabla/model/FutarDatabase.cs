using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobbbformosPizzaAlkalmazasEgyTabla.Model
{
    partial class Futar
    {
        public string getInsert()
        {
            return
                "INSERT INTO `futar` (`fazon`, `fnev`, ``) " +
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
                "UPDATE `ppizza` SET `pnev` = '" +
                getNeme() +
                "', `par` = '" +
                getPrice() +
                "' WHERE `ppizza`.`pazon` = " +
                id;
        }

        public static string getSQLCommandDeleteAllRecord()
        {
            return "DELETE FROM ppizza";
        }

        public static string getSQLCommandGetAllRecord()
        {
            return "SELECT * FROM ppizza";
        }
    }
}
