﻿using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    class Environment : IL.Environment
    {
        public Dictionary<Symbol, int> pos;
        public Environment outer;
        public Environment() : base()
        {
            pos = new Dictionary<Symbol, int>();
            outer = this;
        }
        public Environment(Environment o, int _) : base()
        {
            pos = new Dictionary<Symbol, int>();
            outer = o;
        }
        public void AddVariable(Symbol s)
        {
            pos.Add(s, VariableList.Count);
            VariableList.Add(new IL.Variable(this));
        }
        public IL.Variable Find(Symbol s)
        {
            int p;
            if (pos.TryGetValue(s, out p))
            {
                return VariableList[p];
            }
            else if (outer != this)
            {
                return outer.Find(s);
            }
            else throw new SyntaxError(string.Format("{0} Not Found", s));
        }
    }
}
