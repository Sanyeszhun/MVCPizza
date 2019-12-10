﻿using System;
using System.Runtime.Serialization;

namespace TobbformosPizzaAlkalmazasEgyTabla.model
{
    [Serializable]
    internal class ModelFutarNotValidNameExeption : Exception
    {
        public ModelFutarNotValidNameExeption()
        {
        }

        public ModelFutarNotValidNameExeption(string message) : base(message)
        {
        }

        public ModelFutarNotValidNameExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModelFutarNotValidNameExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}