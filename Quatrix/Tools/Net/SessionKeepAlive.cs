using System.Threading;

namespace Maytech.Quatrix.Tools.Net {

    [System.Serializable]
    public sealed class SessionKeepAlive {

        private IQuatrixAuthentication auth;            //user session
        private readonly System.TimeSpan expire_time;         
        private volatile bool skip = false;
        private bool isAlive = false;



        public bool KeepAlive {
            get;
            set;
        }


        public SessionKeepAlive ( IQuatrixAuthentication auth ) {
            this.auth = auth;
            expire_time = System.TimeSpan.FromMinutes( 10.0 );
        }


        public void Start ( ) {
            if (!isAlive) {
                Thread t = new Thread( ( ) => KeepSession() );
                t.Start();
            }
        }


        public void Skip ( ) {
            skip = true;
        }


        private void Abort ( ) {
            auth.Logout();
            isAlive = false;
        }


        private void KeepSession ( ) {
            while (auth != null && auth.IsLogined) {
                Thread.Sleep( expire_time );
                if (auth == null) {
                    break;
                }
                if (!skip) {
                    if (auth.IsLogined && KeepAlive) {
                        if (auth.KeepAlive()) {
                            skip = false;
                            continue;
                        }
                    }
                    Abort();
                }
                else {
                    skip = false;
                }
            }
        }
    }
}
