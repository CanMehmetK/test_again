using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ideative.Dinamik
{
    public static class CodeCompiler
    {
        public static List<string> DefaultUsings;
        public static string Namespace;
        private static List<string> _locations;
        private static List<string> _usings;
        private readonly static object Lock;
        public static bool CompillationEnable { get; set; }
        public static bool DebugMode { get; set; }
        public static List<string> Locations
        {
            get
            {
                List<string> strs;
                if (CodeCompiler._locations != null)
                {
                    return CodeCompiler._locations;
                }
                lock (CodeCompiler.Lock)
                {
                    if (CodeCompiler._locations == null)
                    {
                        CodeCompiler.LoadDefaultUsingsAndAssemblies();
                        return CodeCompiler._locations;
                    }
                    else
                    {
                        strs = CodeCompiler._locations;
                    }
                }
                return strs;
            }
        }
        public static List<string> Usings
        {
            get
            {
                List<string> strs;
                if (CodeCompiler._usings != null)
                {
                    return CodeCompiler._usings;
                }
                lock (CodeCompiler.Lock)
                {
                    if (CodeCompiler._usings == null)
                    {
                        CodeCompiler.LoadDefaultUsingsAndAssemblies();
                        return CodeCompiler._usings;
                    }
                    else
                    {
                        strs = CodeCompiler._usings;
                    }
                }
                return strs;
            }
        }

        static CodeCompiler()
        {
            List<string> strs = new List<string>()
            {
                "System",
                "System.Collections",
                "System.Collections.Generic",
                "System.Linq",
                "System.Diagnostics"
            };
            CodeCompiler.DefaultUsings = strs;
            CodeCompiler.Namespace = "Ideative.Dinamik";
            CodeCompiler.Lock = new object();
            CodeCompiler.CompillationEnable = true;
            CodeCompiler.DebugMode = false;
        }

        private static void LoadDefaultUsingsAndAssemblies()
        {
            List<string> strs = new List<string>();
            List<Assembly> list = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();
            List<string> strs1 = new List<string>();
            foreach (Assembly assembly in list)
            {
                try
                {
                    strs1.Add(assembly.Location);
                }
                catch (Exception exception)
                {
                    continue;
                }
                List<string> strs2 = new List<string>();
                strs2.AddRange(CodeCompiler.DefaultUsings);
                strs.AddRange(strs2);
            }
            CodeCompiler._locations = strs1;
            CodeCompiler._usings = strs.Distinct<string>().ToList<string>();
        }
        public static void RegisterAssembly(string longName)
        {
            CodeCompiler.RegisterAssembly(Assembly.Load(longName));
        }

        public static void RegisterAssembly(Assembly assembly)
        {
            CodeCompiler.Locations.Add(assembly.Location);
            CodeCompiler.Usings.AddRange(CodeCompiler.GetAllNamespacesFromAssembly(assembly));
        }
        private static IEnumerable<string> GetAllNamespacesFromAssembly(Assembly loadedAssembly)
        {
            return (
                from t in (IEnumerable<Type>)loadedAssembly.GetTypes()
                select t.Namespace).Distinct<string>().Where<string>((string n) =>
                {
                    if (string.IsNullOrEmpty(n))
                    {
                        return false;
                    }
                    return !n.StartsWith("<");
                });
        }

        public static void CodeActionsInvoker(string className, string actionCode)
        {
            StringBuilder stringBuilderSystem = new StringBuilder();
            StringBuilder stringBuilderCode = new StringBuilder();
            int satirNo = 0;

            // #1 Usings
            foreach (var str in Usings)
            {
                stringBuilderCode.AppendFormat("using {0};\r\n", str);
                satirNo++;
            }
            // #2 NameSpace Def.
            //stringBuilderCode.AppendFormat("namespace {0}_{1} {{", Namespace, "");
            stringBuilderCode.AppendFormat("namespace {0} {{", Namespace, "");
            stringBuilderCode.Append("\r\n");
            satirNo++;
            // #3 Class Def.
            //stringBuilderCode.AppendFormat("public static class {0}_{1}_Class {{", className,"" );
            stringBuilderCode.AppendFormat("public static class {0} {{", className, "");
            stringBuilderCode.Append("\r\n");
            satirNo++;
            // #3 Debug Def.
            if (CodeCompiler.DebugMode)
            {
                actionCode = actionCode.Replace("/*break*/", "System.Diagnostics.Debugger.Break();");
            }
            stringBuilderCode.Append(actionCode);
            stringBuilderCode.Append("}");
            stringBuilderCode.Append("\r\n}");
            using (CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider(new Dictionary<string, string>()
                {
                    { "CompilerVersion", "v4.0" }
                }))
            {
                CompilerParameters compilerParameter = new CompilerParameters();
                if (!CodeCompiler.DebugMode)
                {
                    compilerParameter.GenerateInMemory = true;
                }
                else
                {
                    compilerParameter.GenerateInMemory = false;
                    compilerParameter.IncludeDebugInformation = true;
                    compilerParameter.TempFiles = new TempFileCollection(Environment.GetEnvironmentVariable("TEMP"), true);
                }
                compilerParameter.ReferencedAssemblies.AddRange(CodeCompiler.Locations.ToArray());
                string[] str1 = new string[] { stringBuilderCode.ToString() };
                CompilerResults compilerResult = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameter, str1);
                if (compilerResult.Errors.HasErrors)
                {
                    foreach (CompilerError error in compilerResult.Errors)
                    {
                        if (error.IsWarning)
                        {
                            continue;
                        }
                        int line = error.Line - satirNo - 1;
                        if (line <= 0)
                        {
                            stringBuilderSystem.AppendLine(string.Format("({0}): error {1}: {2}", "Using section", error.ErrorNumber, error.ErrorText));
                        }
                        else
                        {
                            object[] column = new object[] { line, error.Column, error.ErrorNumber, error.ErrorText };
                            stringBuilderSystem.AppendLine(string.Format("({0}:{1}): error {2}: {3}", column));
                        }
                    }
                }

                if (stringBuilderSystem.Length <= 0)
                {
                    Assembly compiledAssembly = compilerResult.CompiledAssembly;
                    var compilledType = compiledAssembly.GetType("Ideative.Dinamik.myClass");// string.Format("{0}.{1}",Namespace,className));

                    //MethodInfo[] methods = compilledType.GetMethods(BindingFlags.Static | BindingFlags.Public);
                    //methods[0].Invoke(null, null);
                    //var t = compilledType.InvokeMember("Execute",
                    //      BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                    //      null, null, null);
                    //Assembly compiledAssembly = compilerResult.CompiledAssembly;
                    //codeActionsInvoker.AddCompilledType(compiledAssembly.GetTypes().First<Type>());
                    //var obj = Activator.CreateInstance(compilledType);
                    new myCodeActionsInvoker().AddCompilledType(compiledAssembly.GetTypes().First<Type>());
                }
            }
        }
    }
}

