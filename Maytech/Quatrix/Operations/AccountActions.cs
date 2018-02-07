using Maytech.Quatrix.Entity;
using System.Net;

namespace Maytech.Quatrix.Operations {

    /// <summary>
    /// Describes operations which can be performed with accounts
    /// </summary>
    public static class AccountActions {

        public static AccountInfo AccountInfo(this IQuatrixRequest session) {
            try {
                return session.Get<AccountInfo>(QRequest.ACCOUNT_METADATA);
            }
            catch(Newtonsoft.Json.JsonReaderException) {
                //host can be in black list
                throw new QuatrixInvalidHostException();
            }
            catch(Newtonsoft.Json.JsonSerializationException) {
                //host can be in black list
                throw new QuatrixInvalidHostException();
            }
            catch(QuatrixObjectNotFoundException) {
                throw new QuatrixInvalidHostException();
            }
            catch(WebException ex) {
                int code = System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                if(HttpRequest.HOST_NOT_RESOLVED == code) {
                    throw new QuatrixInvalidHostException();
                }
                HttpWebResponse resp = ex.Response as HttpWebResponse;
                if(resp == null) {
                    throw;
                }
                if(resp.StatusCode == HttpStatusCode.NotFound) {
                    throw new QuatrixInvalidHostException();
                }
                throw;
            }
        }
    }
}