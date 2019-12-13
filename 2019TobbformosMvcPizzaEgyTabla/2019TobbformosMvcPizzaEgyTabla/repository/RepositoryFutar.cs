using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using TobbbformosPizzaAlkalmazasEgyTabla.Model;
using TobbbformosPizzaAlkalmazasEgyTabla.Repository;

namespace TobbbformosPizzaAlkalmazasEgyTabla.repository
{
    partial class FRepository
    {
        List<Futar> futarok;

        public List<Futar> getFutarok()
        {
            return futarok;
        }

        public void setFutarok(List<Futar> futarok)
        {
            this.futarok = futarok;
        }

        public DataTable getPizzaDataTableFromList()
        {
            DataTable futarDT = new DataTable();
            futarDT.Columns.Add("azon", typeof(int));
            futarDT.Columns.Add("nev", typeof(string));
            futarDT.Columns.Add("tel", typeof(string));
            foreach (Futar f in futarok)
            {
                futarDT.Rows.Add(f.getId(), f.getName(), f.getTel());
            }
            return futarDT;
        }

        private void fillFutarListFromDataTable(DataTable futardt)
        {
            foreach (DataRow row in futardt.Rows)
            {
                int id = Convert.ToInt32(row[0]);
                string name = row[1].ToString();
                string tel = row[2].ToString();
                Futar f = new Futar(id, name, tel);
                futarok.Add(f);
            }
        }

        public void deleteFutarFromListByID(int id)
        {
            Futar f = futarok.Find(x => x.getId() == id);
            if (f != null)
            {
                futarok.Remove(f);
            }
            else
            {
                throw new RepositoryExceptionCantDelete("A futárt nem lehetett törölni.");
            }  
        }

        public void updateFutarInList(int id, Futar modified)
        {
            Futar f = futarok.Find(x => x.getId() == id);
            if (f != null)
            {
                f.update(modified);
            }
            else
            {
                throw new RepositoryExceptionCantModified("A futár módosítása nem sikerült");
            }  
        }

        public void addFutarToList(Futar ujFutar)
        {
            try
            {
                futarok.Add(ujFutar);
            }
            catch (Exception ex)
            {
                throw new RepositoryExceptionCantAdd("A futár hozzáadása nem sikerült");
            }
        }

        public Futar getFutar(int id)
        {
            return futarok.Find(x => x.getId() == id);
        }

        public int getNextFutarId()
        {
            if (futarok.Count == 0)
            {
                return 1;
            }
            else
            {
                return futarok.Max(x => x.getId()) + 1;
            }     
        }
    }
}
