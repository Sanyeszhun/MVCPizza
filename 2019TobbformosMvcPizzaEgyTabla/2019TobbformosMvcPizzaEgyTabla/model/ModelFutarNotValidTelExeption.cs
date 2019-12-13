using System;
using System.Runtime.Serialization;

namespace TobbbformosPizzaAlkalmazasEgyTabla
{
    [Serializable]
    internal class ModelFutarNotValidTelExeption : Exception
    {
        public ModelFutarNotValidTelExeption()
        {
        }

        public ModelFutarNotValidTelExeption(string message) : base(message)
        {
        }

        public ModelFutarNotValidTelExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModelFutarNotValidTelExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}