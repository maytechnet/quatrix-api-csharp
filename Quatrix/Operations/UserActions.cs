using Maytech.Quatrix.Messages;
using Maytech.Quatrix.Tools;
using Maytech.Quatrix.Tools.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Maytech.Quatrix.Operations {

    public static class UserActions {


        public static UserShareMetadata GetUserShareMetadata ( this IQEntity e ) {
            return e.Request.MakeApiCall<UserShareMetadata>( string.Format( "/user/ishare_metadata/{0}", e.id ) );
        }


        public static QuotaInfo GetUserQuotaInfo ( this IQuatrixRequest r ) {
            return r.MakeApiCall<QuotaInfo>( "/user/info" );
        }


        public static UserPgpInfo GetPgpInfo ( this Profile profile ) {
            return profile.Request.MakeApiCall<UserPgpInfo>( string.Format( "/user/key/{0}", profile.id ) );
        }


        public static IEnumerable<User> GetUsers ( this IQuatrixRequest request ) {
            request.Enable();
            string api_call = "/user/get";
            string json = request.CreateRequest( api_call );
            return JToken.Parse( json ).ToObject<List<User>>();
        }


        public static bool IsEnoughtSpace ( this QuotaInfo quota, long data_lenght ) {
            return quota.acc_used + data_lenght < quota.acc_limit;
        }


        public static bool IsEnoughtSpace ( this IQuatrixRequest request, long data_lenght ) {
            QuotaInfo qi = request.GetUserQuotaInfo();
            return qi.IsEnoughtSpace( data_lenght );
        }
        

        /// <summary>
        /// This API call is used to request user password reset email.
        /// </summary>
        /// <param name="host">Quatrix name</param>
        /// <param name="email">User email</param>
        /// <returns></returns>
        public static bool ResetPassword ( string host, string email ) {
            IQuatrixRequest request = new QApi( host, email );
            request.Enable( false );
            try {
                Uri u = null;
                host.IsValidHost( out u );
                host = u.ToString();
            }
            catch (QException ex) {
                throw ex;
            }
            string api_call = "/user/request_password_reset";
            string api_uri = request.CreateApiUri( api_call );
            string res = Processing.CreateAnonymRequest( api_uri, Parameters.ResetPass( email ), Error.api_credential_incorrect );
            return !string.IsNullOrEmpty( res );
        }
    }
}



