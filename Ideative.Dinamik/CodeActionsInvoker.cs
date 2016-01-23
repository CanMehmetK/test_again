using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ideative.Dinamik
{
    public sealed class myCodeActionsInvoker
    {
        private readonly Dictionary<string, Action<string>> _actions = new Dictionary<string, Action<string>>();
        private readonly Dictionary<string, Func<string, string>> _getRules = new Dictionary<string, Func<string, string>>();

        public myCodeActionsInvoker()
        {

        }

        public void AddCompilledType(Type compilledType)
        {
            MethodInfo[] methods = compilledType.GetMethods(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i < (int)methods.Length; i++)
            {
                MethodInfo methodInfo = methods[i];
                //Type type = typeof(CodeActionType);
                string name = methodInfo.Name;
                char[] chrArray = new char[] { '\u005F' };

                this._actions.Add(methodInfo.Name.ToLower(), new Action<string>((string p) => this.InvokeAction(p, methodInfo)));
                // _actions["execute"].Invoke("")
                this._getRules.Add(methodInfo.Name.ToLower(), new Func<string, string>((string p) => this.InvokeRuleGet(p, methodInfo)));
                // _getRules["execute"].Invoke("test")
                break;
            }
        }
        private void InvokeAction(string parameter, MethodInfo methodInfo)
        {
            object[] objArray = new object[] { parameter };
            methodInfo.Invoke(null, objArray);
        }
        private string InvokeRuleGet(string parameter, MethodInfo methodInfo)
        {
            object[] objArray = new object[] { parameter };
            return (string)methodInfo.Invoke(null, objArray);
        }
    }
}
