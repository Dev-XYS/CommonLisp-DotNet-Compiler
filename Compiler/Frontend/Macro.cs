﻿using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    class Macro
    {
        private static Dictionary<Symbol, int> gml = new Dictionary<Symbol, int>();
        public static void Init()
        {
            gml.Add(Symbol.FindOrCreate("DEFUN"), 1);
            gml.Add(Symbol.FindOrCreate("DEFPARAMETER"), 2);
            gml.Add(Symbol.FindOrCreate("AND"), 3);
            gml.Add(Symbol.FindOrCreate("OR"), 4);
        }
        public static bool IsMacro(Symbol s)
        {
            return gml.ContainsKey(s);
        }
        public static Cons DefunExpand(IType form)
        {
            var (t1, tbody) = Util.RequireAtLeast(form, 2, "DEFUN");
            if (!(t1[0] is Symbol name))
                throw new SyntaxError("DEFUN: Invalid name");
            if (!(t1[1] is Cons || t1[1] is null))
                throw new SyntaxError("DEFUN: Invalid parameter list");
            return new Cons(Symbol.Find("PROGN"), new Cons(
                new Cons(Symbol.Find("SPECIAL"), new Cons(name, Lisp.nil)), new Cons(
                    new Cons(Symbol.Find("SETQ"), new Cons(name, new Cons(new Cons(Symbol.Find("LAMBDA"), new Cons(t1[1], tbody)), Lisp.nil))), Lisp.nil)));
        }
        public static Cons DefparameterExpand(IType form)
        {
            var (t1, tbody) = Util.RequireAtLeast(form, 1, "DEFPARAMETER");
            if (!(t1[0] is Symbol name))
                throw new SyntaxError("DEFPARAMETER: Invalid name");
            Cons ret = new Cons(Symbol.Find("PROGN"), new Cons(
                new Cons(Symbol.Find("SPECIAL"), new Cons(name, Lisp.nil)), Lisp.nil));
            if(tbody is Cons c)
            {
                (ret.cdr as Cons).cdr = new Cons(new Cons(Symbol.Find("SETQ"), new Cons(name, new Cons(c.car, Lisp.nil))), Lisp.nil);
            }
            return ret;
        }
        public static IType AndExpand(IType form)
        {
            if (!(form is Cons c))
                return Symbol.Find("T");
            return new Cons(Symbol.Find("IF"), new Cons(c.car, new Cons(AndExpand(c.cdr), new Cons(Symbol.Find("NIL"), Lisp.nil))));
        }
        public static IType OrExpand(IType form)
        {
            if (!(form is Cons c))
                return Symbol.Find("NIL");
            return new Cons(Symbol.Find("IF"), new Cons(c.car, new Cons(Symbol.Find("T"), new Cons(OrExpand(c.cdr), Lisp.nil))));
        }
        public static IType FullExpand(Cons form)
        {
            switch(gml[form.car as Symbol])
            {
                case 1:
                    return DefunExpand(form.cdr);
                case 2:
                    return DefparameterExpand(form.cdr);
                case 3:
                    return AndExpand(form.cdr);
                case 4:
                    return OrExpand(form.cdr);
                default:
                    throw new NotImplementedException("Macros are not implemented yet");
            }
        }

    }
}