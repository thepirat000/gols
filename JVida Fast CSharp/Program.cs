// Thepirat 2011
// thepirat000@hotmail.com

using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using JVida_Fast_CSharp.Helpers;

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
        public static void Main(string[] args)
        {
            var initialFilePath = args.Length > 0 ? args[0] : null;
            if (PrevInstance())
            {
                if (initialFilePath != null)
                {
                    using (var pipe = new NamedPipeClientStream(".", MainForm.LoadPatternPipeName, PipeDirection.Out))
                    using (var stream = new StreamWriter(pipe))
                    {
                        pipe.Connect();
                        stream.WriteLine(initialFilePath);
                    }
                }
                else
                {
                    Application.Exit();
                }
                return;
            }
            if (HasAdminPrivileges())
            {
                try
                {
                    FileAssociation.AssociateFileTypes(Application.ExecutablePath);
                }
                catch (Exception)
                {
                }
            }
            Application.Run(new MainForm(initialFilePath));
        }

        public static bool HasAdminPrivileges()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator);
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