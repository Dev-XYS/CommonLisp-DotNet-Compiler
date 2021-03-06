﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    /// <summary>
    /// 表示一个完整的中间代码程序。
    /// 一个程序由一下三部分构成：
    /// (1) 全体函数集合；
    /// (2) 全体环境列表；
    /// (3) 主程序代码。
    /// 对每个函数分别编译为一个Function对象，储存在FunctionLisp中。
    /// 将主程序编译后储存在Main中。
    /// </summary>
    class Program
    {
        /// <summary>
        /// 全体函数列表
        /// </summary>
        public IEnumerable<Function> FunctionList { get; set; }

        /// <summary>
        /// 全体环境列表
        /// </summary>
        public IEnumerable<Environment> EnvList { get; set; }

        /// <summary>
        /// 主函数（无参数）
        /// </summary>
        public Function Main { get; set; }

        public Program()
        {
        }
    }
}
