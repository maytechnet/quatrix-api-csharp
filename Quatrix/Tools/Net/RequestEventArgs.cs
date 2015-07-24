using System;

namespace Maytech.Quatrix.Tools.Net {


    public class RequestEventArgs : EventArgs {

        public string ApiCall {
            get;
            set;
        }


        public Method Method {
            get;
            set;
        }


        public string Signature {
            get;
            set;
        }


        public byte[] PostData {
            get;
            set;
        }


        public IQuatrixRequest Request {
            get;
            set;
        }


        public RequestEventArgs ( ) {

        }


        public RequestEventArgs ( IQuatrixRequest request, string api_call, Method method, string signature, byte[] post_data ) {
            this.Request = request;
            this.ApiCall = api_call;
            this.Method = method;
            this.Signature = signature;
            this.PostData = post_data;
        }
    }
}
