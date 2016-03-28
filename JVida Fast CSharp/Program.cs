// Thepirat 2011
// thepirat000@hotmail.com

using System.IO;
using System.Linq;

namespace JVida_Fast_CSharp
{
    using Parsers;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    public static class Program
    {
        private static Mutex mutex;

        [STAThread]
        public static void Main()
        {
            if (PrevInstance())
            {
                Application.Exit();
                return;
            }
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Determines if any previous instance of the program is running
        /// </summary>
        public static bool PrevInstance()
        {
            string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string mutexName = "Global\\" + AssemblyName;
            bool newMutexCreated = false;
            try
            {
                mutex = new Mutex(false, mutexName, out newMutexCreated);
                if (newMutexCreated)
                {
                    return false;
                }
                else
                {
                    mutex.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}