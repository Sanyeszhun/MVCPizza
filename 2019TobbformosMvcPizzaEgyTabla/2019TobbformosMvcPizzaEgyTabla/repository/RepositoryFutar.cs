using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using TobbbformosPizzaAlkalmazasEgyTabla.Model;
using TobbformosPizzaAlkalmazasEgyTabla.model;

namespace TobbbformosPizzaAlkalmazasEgyTabla.Repository
{
    partial class Repository
    {
        List<Futar> futar;

        public List<Futar> getFutar()
        {
            return futar;
        }

        public void setFutar(List<Futar> futar)
        {
            this.futar = futar;
        }



        public DataTable getFutarDataTableFromList()
        {
            DataTable futarDT = new DataTable();
            futarDT.Columns.Add("azon", typeof(int));
            futarDT.Columns.Add("nev", typeof(string));
            futarDT.Columns.Add("igazolvanyszam", typeof(int));
            foreach (Futar p in futar)
            {
                futarDT.Rows.Add(p.getId(), p.getNeme(), p.getIg());
            }
            return futarDT;
        }

        private void fillFutarListFromDataTable(DataTable futardt)
        {
            foreach (DataRow row in futardt.Rows)
            {
                int azon = Convert.ToInt32(row[0]);
                string nev = row[1].ToString();
                int ig = Convert.ToInt32(row[2]);
                Futar p = new Futar(azon, nev, ig);
                futar.Add(p);
            }
        }

        public void deleteFutarFromList(int id)
        {
            Futar p = futar.Find(x => x.getId() == id);
            if (p != null)
                futar.Remove(p);
            else
                throw new RepositoryExceptionCantDelete("A futárt nem lehetett törölni.");
        }

        public void updateFutarInList(int id,Futar modified)
        {
            Futar p = futar.Find(x => x.getId() == id);
            if (p != null)
                p.update(modified);
            else
                throw new RepositoryExceptionCantModified("A futár módosítása nem sikerült");
        }

        public void addFutarToList(Futar ujFutar)
        {
            try
            {
                futar.Add(ujFutar);
            }
            catch (Exception e)
            {
                throw new RepositoryExceptionCantAdd("A futar hozzáadása nem sikerült");
            }
        }

        public Futar getFutar(int id)
        {
            return futar.Find(x => x.getId() == id);
        }

        public int getNextPFutarId()
        {
            if (futar.Count == 0)
                return 1;
            else
                return futar.Max(x => x.getId()) + 1;
        }
    }
}
