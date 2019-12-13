using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobbbformosPizzaAlkalmazasEgyTabla
{
    partial class Futar
    {
        public string getInsert()
        {
            return
                "INSERT INTO `pfutar` (`fazon`, `fnev`, `ftel`) " +
                "VALUES ('" +
                id +
                "', '" +
                getName() +
                "', '" +
                getTel() +
                "');";
        }

        public string getUpdate(int id)
        {
            return
                "UPDATE `pfutar` SET `fnev` = '" +
                getName() +
                "', `ftel` = '" +
                getTel() +
                "' WHERE `pfutar`.`fazon` = " +
                id;
        }

        public static string getSQLCommandDeleteAllRecord()
        {
            return "DELETE FROM pfutar";
        }

        public static string getSQLCommandGetAllRecord()
        {
            return "SELECT * FROM pfutar";
        }
    }
}
