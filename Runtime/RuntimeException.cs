﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message)
        {
        }
    }
}
