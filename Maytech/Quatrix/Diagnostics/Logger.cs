using System;
using System.Diagnostics;
using System.Threading;

namespace Maytech.Quatrix.Diagnostics {


    public sealed class Logger {


        /*
         
            Log format: 
            1 - date/time
            2 - assembly
            3 - thread id
            4 - state (log level)
            5 - type (exception or something else)
            6 - message

        */


        private static Logger loggerInstance = null;                //logger instance
        private string logName = string.Empty;


        private string fatal = LogLevel.Fatal.ToString().ToUpper();
        private string error = LogLevel.Error.ToString().ToUpper();
        private string info = LogLevel.Info.ToString().ToUpper();
        private string debug = LogLevel.Debug.ToString().ToUpper();


        /// <summary>
        /// true - to add Thread ID to WRITE method
        /// </summary>
        public bool UseThreadId { get; set; }


        public bool AutoFlush {
            get {
                return System.Diagnostics.Debug.AutoFlush;
            }
            set {
                System.Diagnostics.Debug.AutoFlush = value;
            }
        }


        public LogLevel WriteOnLevel { get; set; }


        public string LogFullName {
            get {
                return logName;
            }
            set {
                if( logName == value ) {
                    return;
                }
                //TODO: handle max log file -> rename with plus one
                logName = value;
                TextWriterTraceListener lis = new TextWriterTraceListener( logName );
                System.Diagnostics.Debug.Listeners.Clear();
                System.Diagnostics.Debug.Listeners.Add( lis );
            }
        }


        private Logger() {
            WriteOnLevel = LogLevel.Fatal;
            //Default name is today with time stamp
            logName = string.Format( "{0}.log", Utils.GetTimestampString() );
        }


        public void Debug( string assembly, params object[] msg_params ) {
            WriteLog( assembly, LogLevel.Debug, debug, "-", msg_params );
        }


        public void Info( string assembly, params object[] msg_params ) {
            WriteLog( assembly, LogLevel.Info, info, "-", msg_params );
        }


        public void Error( string assembly, Exception ex ) {
            WriteLog( assembly, LogLevel.Error, error, ex, ex.Message );
        }


        public void Error( string assembly, Exception ex, string exception_type ) {
            WriteLog( assembly, LogLevel.Error, error, exception_type, ex.Message );
        }


        public void Fatal( string assembly, Exception ex ) {
            WriteLog( assembly, LogLevel.Fatal, fatal, ex, ex.Message );
        }


        public void WriteCustom( string assembly, LogLevel level, object type, params object[] msg_params ) {
            WriteLog( assembly, level, null, type, msg_params );
        }


        private void WriteLog( string assembly, LogLevel level, string levelString, object type, params object[] msg_params ) {
            if( WriteOnLevel < level ) {
                return;
            }
            if( string.IsNullOrEmpty( levelString ) ) {
                levelString = level.ToString().ToLower();
            }
            //create message
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach( object item in msg_params ) {
                if( item != null ) {
                    sb.Append( item );
                }
            }
            //write to log
            Trace.WriteLine(
                string.Format( "{0} {1} {2} {3} {4} '{5}'",
                    DateTime.Now.ToString(),
                    assembly,
                    UseThreadId ? Thread.CurrentThread.ManagedThreadId : -1,
                    levelString,
                    type,
                    sb.ToString()
                    )
                );
        }


        /// <summary>
        /// Write log to file
        /// </summary>
        public void Flush() {
            System.Diagnostics.Debug.Flush();
        }


        public static Logger GetInstance() {
            if( loggerInstance == null ) {
                loggerInstance = new Logger();
            }
            return loggerInstance;
        }
    }
}
