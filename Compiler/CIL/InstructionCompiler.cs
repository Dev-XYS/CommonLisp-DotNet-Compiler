﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    using I = Instructions;

    partial class Function
    {
        public void GenInstruction(IL.IInstruction instr)
        {
            if (instr is IL.CallInstruction)
            {
                GenCall(instr as IL.CallInstruction);
            }
            else if (instr is IL.FunctionInstruction)
            {
                GenFunction(instr as IL.FunctionInstruction);
            }
            else if (instr is IL.MoveInstruction)
            {
                GenMove(instr as IL.MoveInstruction);
            }
            else if (instr is IL.ReturnInstruction)
            {
                GenReturn(instr as IL.ReturnInstruction);
            }
            else
            {
                Gen(new I.NotImplemented());
            }
        }

        private void Gen(Instruction instr)
        {
            InstructionList.Add(instr);
        }

        private void GenLoadVariable(IL.Variable var)
        {
            if (var.Env != null)
            {
                // Load environment
                Gen(new I.LoadArgument { ArgNo = 0 });
                Gen(new I.LoadField { Field = EnvMap[var.Env] });
                Gen(new I.LoadField { Field = EnvMap[var.Env].Environment.VarMap[var] });
            }
        }

        private void GenLoadConst(Runtime.IType c)
        {
            if (c is Runtime.TInteger)
            {
                Runtime.TInteger i = c as Runtime.TInteger;
                Gen(new I.LoadInt { Value = i.Value });
                Gen(new I.NewObject { Type = new RuntimeObject { Type = typeof(Runtime.TInteger) } });
            }
            else if (c.GetType().Namespace.StartsWith("Runtime.Function"))
            {
                Gen(new I.NewObject { Type = new RuntimeObject { Type = c.GetType() } });
            }
            else
            {
                Gen(new I.LoadNull { });
            }
        }

        private void GenLoadEntity(IL.IEntity e)
        {
            if (e == null)
            {
                Gen(new I.LoadNull { });
            }
            else if (e is IL.Variable)
            {
                GenLoadVariable(e as IL.Variable);
            }
            else
            {
                IL.ImmediateNumber imm = e as IL.ImmediateNumber;
                GenLoadConst(imm.Imm);
            }
        }

        private void GenStoreTemp(IL.Variable var)
        {
            if (var.Env != null)
            {
                Gen(new I.LoadArgument { ArgNo = 0 });
                Gen(new I.LoadField { Field = EnvMap[var.Env] });
                Gen(new I.Load { Loc = 0 });
                Gen(new I.StoreField { Field = EnvMap[var.Env].Environment.VarMap[var] });
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void GenCall(IL.CallInstruction instr)
        {
            GenLoadVariable(instr.Function);
            Gen(new I.LoadInt { Value = instr.Parameters.Count });
            Gen(new I.NewArray { Type = "[Runtime]Runtime.IType" });
            Gen(new I.Store { Loc = 1 });
            int no = 0;
            foreach (IL.IEntity e in instr.Parameters)
            {
                Gen(new I.Load { Loc = 1 });
                Gen(new I.LoadInt { Value = no++ });
                GenLoadEntity(e);
                Gen(new I.StoreElement { });
            }
            Gen(new I.Load { Loc = 1 });
            Gen(new I.CallVirtual { });
            Gen(new I.Store { Loc = 0 });
            GenStoreTemp(instr.Destination);
        }

        private void GenFunction(IL.FunctionInstruction instr)
        {
            foreach (EnvironmentMember env in EnvList)
            {
                Gen(new I.LoadArgument { ArgNo = 0 });
                Gen(new I.LoadField { Field = env });
            }
            Gen(new I.NewObject { Type = Program.FuncMap[instr.Function] });
            Gen(new I.Store { Loc = 0 });
            GenStoreTemp(instr.Destination);
        }

        private void GenMove(IL.MoveInstruction instr)
        {
            GenLoadEntity(instr.Source);
            Gen(new I.Store { Loc = 0 });
            GenStoreTemp(instr.Destination);
        }

        private void GenReturn(IL.ReturnInstruction instr)
        {
            GenLoadEntity(instr.Value);
            Gen(new I.Return { });
        }
    }
}
