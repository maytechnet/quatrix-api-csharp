using System;

namespace Maytech.Quatrix {


    /// <summary>
    ///Represents session lifetime. Can't be inherited
    /// </summary>
    public sealed class SessionKeepAlive : IDisposable {


        private const double LOOP_TIME_MINUTES = 5;       //5 min
        private readonly System.Timers.Timer timer = new System.Timers.Timer();     //background worker for keep-alive


        /// <summary>
        /// Gets or sets a value indicating if the session should reload itself after some time period.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [keep alive]; otherwise, <c>false</c>.
        /// </value>
        public bool KeepAlive {
            get {
                return timer.Enabled;
            }
            set {
                if( !value && timer.Enabled ) {
                    timer.Stop();
                } else if( value && !timer.Enabled ) {
                    timer.Start();
                }
            }
        }


        /// <summary>
        /// Time beetwen sending anoter keep alive request
        /// </summary>
        public TimeSpan LoopTime {
            get {
                return TimeSpan.FromMilliseconds( timer.Interval );
            }
            set {
                timer.Interval = value.TotalMilliseconds;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SessionKeepAlive"/> class with specified authentification properties.
        /// </summary>
        /// <param name="auth">User session</param>
        internal SessionKeepAlive( IQuatrixRequest request ) {
            if( request == null ) {
                throw new ArgumentNullException( "request" );
            }
            LoopTime = TimeSpan.FromMinutes( LOOP_TIME_MINUTES );
            timer.Elapsed += ( o, e ) => {
                try {
                    request.Get( QRequest.SESSION_KEEP_ALIVE );
                }
                catch( Exception ex ) {
                    Debug.Logger.Error( SETTINGS.APP_LOG_NAME, ex );
                    KeepAlive = false;
                }
            };
        }


        public void Dispose() {
            KeepAlive = false;
            timer.Dispose();
        }
    }
}