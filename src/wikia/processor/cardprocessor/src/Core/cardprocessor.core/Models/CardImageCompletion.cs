using System;
using System.Collections.Generic;

namespace cardprocessor.core.Models
{
    public class CardImageCompletion
    {
        public bool IsSuccessful { get; set; }
        public List<string> Errors { get; set; }
        public Exception Exception { get; set; }
    }
}