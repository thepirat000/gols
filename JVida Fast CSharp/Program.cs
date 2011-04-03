//Thepirat 2011
//thepirat000@hotmail.com
using System.Diagnostics;
using System.Threading;
using System;
using System.Windows.Forms;

namespace JVida_Fast_CSharp
{
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
        /// Determina si existe alguna instancia previa del programa corriendo
        /// </summary>
        public static bool PrevInstance()
        {
            //Obtengo el nombre del ensamblado donde se encuentra ésta función
            string NombreAssembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            //Nombre del mutex según Tipo (visibilidad)
            string mutexName = "Global\\" + NombreAssembly;
            bool newMutexCreated = false;
            try
            {
                //Abro/Creo mutex con nombre único
                mutex = new Mutex(false, mutexName, out newMutexCreated);
                if (newMutexCreated)
                {
                    //Se creó el mutex, NO existe instancia previa
                    return false;
                }
                else
                {
                    //El mutex ya existía, Libero el mutex 
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