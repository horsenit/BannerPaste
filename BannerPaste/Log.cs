using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BannerPaste
{
    class Log
    {
        static StreamWriter Writer;

        static Log()
        {
            try
            {
                var dir = Path.GetDirectoryName(Path.GetFullPath(Assembly.GetExecutingAssembly().Location));
                Writer = new StreamWriter(new FileStream(Path.Combine(dir, "../../log.txt"), FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void write(string msg)
        {
            if (Writer == null) return;
            try
            {
                Writer.WriteLine($"{DateTime.Now.ToString("[HH:mm:ss.fff]")} {msg}");
                Writer.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void write(Exception ex)
        {
            Log.write($@"{ex.GetType().Name} {ex.Message}
{ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Log.write(ex.InnerException);
            }
        }
    }
}
