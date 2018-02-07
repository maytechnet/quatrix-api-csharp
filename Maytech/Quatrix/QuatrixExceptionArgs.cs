using Newtonsoft.Json.Linq;

namespace Maytech.Quatrix {


    internal class QuatrixExceptionArgs {

        private const string CODE_KEY = "code";
        private const string MSG_KEY = "msg";

        private const string DETAILS_KEY = "details";
        private const string STATUS_KEY = "status";
        private const string NAME_KEY = "name";


        [Newtonsoft.Json.JsonIgnore]
        internal JToken details { get; set; }


        /// <summary>
        /// Gets or sets error code (key).
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public int code { get; set; }


        /// <summary>
        /// Gets or sets text of error message (value).
        /// </summary>
        /// <value>
        /// The MSG.
        /// </value>
        public string msg { get; set; }


        public string status { get; set; }


        public string name { get; set; }


        internal static QuatrixExceptionArgs Create( HttpResult result ) {
            JToken token = JToken.Parse( result.Result, new JsonLoadSettings { CommentHandling = CommentHandling.Ignore, LineInfoHandling = LineInfoHandling.Ignore } );
            QuatrixExceptionArgs args = new QuatrixExceptionArgs();
            args.code = token.Value<int>( CODE_KEY );
            args.msg = token.Value<string>( MSG_KEY );
            args.details = token.SelectToken( DETAILS_KEY );
            if( args.details != null && args.details.HasValues ) {
                args.status = args.details.Value<string>( STATUS_KEY );
                args.name = args.details.Value<string>( NAME_KEY );
            }
            return args;
        }
    }
}
