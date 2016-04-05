// Thepirat 2011
// thepirat000@hotmail.com

using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace JVida_Fast_CSharp
{
    public static class Program
    {
        private static Mutex _mutex;

        [STAThread]
        public static void Main(string[] args)
        {
            var initialFilePath = args.Length > 0 ? args[0] : null;
            if (PrevInstance())
            {
                if (initialFilePath != null)
                {
                    SignalLoadPattern(initialFilePath);
                }
                else
                {
                    Application.Exit();
                }
                return;
            }
            Application.Run(new MainForm(initialFilePath));
        }

        private static void SignalLoadPattern(string initialFilePath)
        {
            try
            {
                using (var pipe = new NamedPipeClientStream(".", MainForm.LoadPatternPipeName, PipeDirection.Out))
                using (var stream = new StreamWriter(pipe))
                {
                    pipe.Connect();
                    stream.WriteLine(initialFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load pattern. {ex.Message}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        /// <summary>
        /// Determines if any previous instance of the program is running
        /// </summary>
        public static bool PrevInstance()
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string mutexName = "Global\\" + assemblyName;
            bool newMutexCreated = false;
            try
            {
                _mutex = new Mutex(false, mutexName, out newMutexCreated);
                if (newMutexCreated)
                {
                    return false;
                }
                _mutex.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}