using Maytech.Quatrix.Diagnostics;

namespace Maytech.Quatrix {


    internal static class Debug {


        private static readonly Logger logger = Logger.GetInstance();


        public static Logger Logger {
            get {
                return logger;
            }
        }
    }
}
