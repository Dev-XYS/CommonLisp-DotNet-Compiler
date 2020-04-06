﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class Variable : IEntity
    {
        public string Name { get; set; }

        public Environment Env { get; set; }

        public Variable(string name, Environment env)
        {
            Name = name;
            Env = env;
        }
    }
}
