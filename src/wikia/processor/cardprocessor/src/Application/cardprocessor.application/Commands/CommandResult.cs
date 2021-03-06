﻿using System.Collections.Generic;

namespace cardprocessor.application.Commands
{
    public class CommandResult
    {
        public bool IsSuccessful { get; set; }

        public List<string> Errors { get; set; }

        public object Data { get; set; }
    }
}