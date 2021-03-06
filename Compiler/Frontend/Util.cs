﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runtime;

namespace Compiler.Frontend
{
    static class Util
    {
        public static IType[] RequireExactly(IType list, int n, string name = "unknown")
        {
            List<IType> ret = new List<IType>();
            while (n-- > 0)
            {
                if (!(list is Cons c))
                    throw new SyntaxError(string.Format("{0}: Insufficient arguments", name));
                ret.Add(c.car);
                list = c.cdr;
            }
            if (list != Lisp.nil) throw new SyntaxError(string.Format("{0}: Too many arguments", name));
            return ret.ToArray();
        }
        public static (IType[], IType) RequireAtLeast(IType list, int n, string name = "unknown")
        {
            List<IType> ret1 = new List<IType>();
            while(n-- > 0)
            {
                if (!(list is Cons c))
                    throw new SyntaxError(string.Format("{0}: Insufficient arguments", name));
                ret1.Add(c.car);
                list = c.cdr;
            }
            return (ret1.ToArray(), list);
        }
        public static IType[] ListToArray(IType list)
        {
            List<IType> ret = new List<IType>();
            while(list is Cons c)
            {
                ret.Add(c.car);
                list = c.cdr;
            }
            return ret.ToArray();
        }
        public static void ParseLambdaList(Cons llist, Environment env, Function f)
        {
            var list = ListToArray(llist);
            foreach(var i in list)
            {
                if (!(i is Symbol s))
                    throw new SyntaxError("Illegal parameter list");
                if(!s.Name.StartsWith("&")) f.AddParam(s);
            }
        }
        public static string RandomString(int length)
        {
            var r = new Random();
            var ret = new char[length];
            for(int i = 0; i < length; ++i)
            {
                int cur = r.Next(62);
                ret[i] = (char)(cur < 26 ? 'a' + cur : cur < 52 ? 'A' + cur - 26 : '0' + cur - 52);
            }
            return string.Join("", ret);
        }
    }
}
