using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nimedicus.Utils
{
    public static class ThreadHelper
    {
        /// <summary>
        /// Provides easier way to execute code in Application.Current.Dispatcher, preferably to pass executing code via lambda: ExecuteInMainDispatcher(()=>{ dowork(); });
        /// </summary>
        /// <param name="action">Lambda expression or action</param>
        /// <param name="synchronous">defines whether action should be executed as synchronous, synchronous by default, handle with care</param>
        public static void ExecuteInMainDispatcher(Action action, bool synchronous = true)
        {
            var appDispatcher = Application.Current?.Dispatcher;
            if (appDispatcher == null)
                return;

            if (appDispatcher.CheckAccess())
            {
                if (synchronous)
                {
                    action.Invoke();
                }
                else
                {
                    Task.Run(action);
                }
            }
            else
            {
                if (synchronous)
                {
                    appDispatcher.Invoke(action);
                }
                else
                {
                    appDispatcher.BeginInvoke(action);
                }
            }
        }
    }
}
