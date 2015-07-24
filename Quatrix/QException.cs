using System;

namespace Maytech.Quatrix {

    public class QException : Exception {

        public QException ( string message )
            : base( message ) {
            base.HelpLink = "https://docs.maytech.net/display/MD/Error+Codes";
        }


        public int code {
            get;
            set;
        }


        public string msg {
            get;
            set;
        }
    }
}
