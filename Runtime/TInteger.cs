﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class TInteger : Number
    {
        public int Value { get; }
        public TInteger(int x)
        {
            Value = x;
        }
        public static TInteger From(Number src)
        {
            if (src is TFloat f)
                return new TInteger(Convert.ToInt32(f.Value));
            else if (src is TInteger i)
                return i;
            else throw new NotImplementedException(string.Format("Not implemented conversion: {0} to TInteger", src));
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public override Number Add(Number rhs)
        {
            if (!(rhs is TInteger r))
                throw new RuntimeException("Invalid call: Add(TInteger)");
            return new TInteger(Value + r.Value);
        }

        public override Number Subtract(Number rhs)
        {
            if (!(rhs is TInteger r))
                throw new RuntimeException("Invalid call: Subtract(TInteger)");
            return new TInteger(Value - r.Value);
        }

        public override Number Multiply(Number rhs)
        {
            if (!(rhs is TInteger r))
                throw new RuntimeException("Invalid call: Multiply(TInteger)");
            return new TInteger(Value * r.Value);
        }

        public override Number Divide(Number rhs)
        {
            if (!(rhs is TInteger r))
                throw new RuntimeException("Invalid call: Divide(TInteger)");
            if (r.Value == 0)
                throw new Function.Arith.ArithmeticException("Division by zero!(TInteger)");
            if (Value / r.Value == Convert.ToDouble(Value) / r.Value)
                return new TInteger(Value / r.Value);
            else return new TFloat(Convert.ToDouble(Value) / r.Value);
        }
        public TInteger IntDivide(TInteger rhs)
        {
            return new TInteger(Value / rhs.Value);
        }

        public override Number Negate()
        {
            return new TInteger(-Value);
        }

        public override bool LessThan(Number rhs)
        {
            if (!(rhs is TInteger r))
                throw new RuntimeException("Invalid call: LessThan(TInteger)");
            return Value < r.Value;
        }

        public override Number Reciprocal()
        {
            if (Value == 0)
                throw new Function.Arith.ArithmeticException("Division by zero!(TInteger)");
            if (Value == 1)
                return this;
            return new TFloat(1.0 / Value);
        }

        public override bool Equal(Number rhs)
        {
            if (!(rhs is TInteger r))
                throw new RuntimeException("Invalid call: Equal(TInteger)");
            return Value == r.Value;
        }
    }
}
