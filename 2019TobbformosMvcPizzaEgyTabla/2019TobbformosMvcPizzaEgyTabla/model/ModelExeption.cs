using System;
using System.Runtime.Serialization;

namespace TobbbformosPizzaAlkalmazasEgyTabla.Model
{
    [Serializable]
    internal class ModelPizzaNotValidPriceExeption : Exception
    {
        public ModelPizzaNotValidPriceExeption()
        {
        }

        public ModelPizzaNotValidPriceExeption(string message) : base(message)
        {
        }

        public ModelPizzaNotValidPriceExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModelPizzaNotValidPriceExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}