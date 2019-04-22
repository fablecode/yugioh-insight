using System;
using carddata.core.Exceptions;

namespace carddata.core.Models
{
    public class YugiohCardCompletion
    {
        public bool IsSuccessful { get; set; }
        public Article Article { get; set; }
        public YugiohCard Card { get; set; }
        public YugiohCardException Exception { get; set; }
    }
}