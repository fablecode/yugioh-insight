using System;
using carddata.core.Models;

namespace carddata.core.Exceptions
{
    public class YugiohCardException
    {
        public YugiohCard Card { get; set; }

        public Exception Exception { get; set; }
    }
}