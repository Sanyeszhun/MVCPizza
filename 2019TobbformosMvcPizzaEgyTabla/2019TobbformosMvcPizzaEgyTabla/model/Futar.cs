using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobbformosPizzaAlkalmazasEgyTabla.model
{
    class Futar
    {
        private int id;
        private string name;
        private int ig;

        public Futar(int id, string name, int ig)
        {
            this.id = id;
            this.name = name;
            this.ig = ig;
        }
        public Futar(int id, string name, string ig)
        {
            this.id = id;
            if (!isValidName(name))
                throw new ModelFutarNotValidNameExeption("A futár neve nem megfelelő!");
            if (!isValidPrice(ig))
                throw new ModelFutarNotValidPriceExeption("A pizza ára nem megfelelő!");
            this.name = name;
            this.ig = Convert.ToInt32(ig);
        }

        public void update(Futar modified)
        {
            this.name = modified.getNeme();
            this.ig = modified.getPrice();
        }

        private bool isValidPrice(string price)
        {
            int eredmeny = 0;
            if (int.TryParse(price, out eredmeny))
                return true;
            else
                return false;
        }

        private bool isValidName(string name)
        {
            if (name == string.Empty)
                return false;
            if (!char.IsUpper(name.ElementAt(0)))
                return false;
            for (int i = 1; i < name.Length; i = i + 1)
                if (
                    !char.IsLetter(name.ElementAt(i))
                        &&
                    (!char.IsWhiteSpace(name.ElementAt(i)))

                    )
                    return false;
            return true;
        }

        public void setID(int id)
        {
            this.id = id;
        }
        public void setName(string name)
        {
            this.name = name;
        }
        public void setPrice(int price)
        {
            this.price = price;
        }
        public int getId()
        {
            return id;
        }
        public string getNeme()
        {
            return name;
        }
        public int getPrice()
        {
            return price;
        }
    }
}

    }
}
