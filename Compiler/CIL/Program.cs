﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    class Program
    {
        private List<Environment> EnvList { get; }
        public Dictionary<IL.Environment, Environment> EnvMap { get; }

        private List<Function> FuncList { get; }
        public Dictionary<IL.Function, Function> FuncMap { get; }

        private Function Main { get; }

        public ConstantClass Const { get; }

        public Program(IL.Program prog)
        {
            EnvList = new List<Environment>();
            EnvMap = new Dictionary<IL.Environment, Environment>();
            FuncList = new List<Function>();
            FuncMap = new Dictionary<IL.Function, Function>();

            Const = new ConstantClass();

            foreach (IL.Environment env in prog.EnvList)
            {
                Environment e = new Environment(env);
                EnvList.Add(e);
                EnvMap[env] = e;
            }

            foreach (IL.Function func in prog.FunctionList)
            {
                Function f = new Function(this, func);
                FuncList.Add(f);
                FuncMap[func] = f;
            }
            foreach (Function f in FuncList)
            {
                f.Generate();
            }

            Main = FuncMap[prog.Main];

            Const.Generate();
        }

        public void Emit(System.IO.TextWriter writer)
        {
            Emitter.Writer = writer;

            EmitHeader();

            EmitMain();

            Const.Emit();

            foreach (Environment env in EnvList)
            {
                env.Emit();
            }
            foreach (Function func in FuncList)
            {
                func.Emit();
            }
        }

        private void EmitHeader()
        {
            Emitter.EmitRaw(global::Compiler.Properties.Resources.CILHeader);
        }

        private void EmitMain()
        {
            Emitter.Emit("");
            Emitter.Emit(global::Compiler.Properties.Resources.CILMain, Main.Name);
        }
    }
}