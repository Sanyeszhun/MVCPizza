using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobbformosPizzaAlkalmazasEgyTabla.model;
using TobbformosPizzaAlkalmazasEgyTabla.Model;

namespace TobbformosPizzaAlkalmazasEgyTabla.Repository
{
    partial class Repository
    {
        public Repository()
        {
            pizzas = new List<Pizza>();
            futar = new  List<Futar>();
        }        
    }
}
