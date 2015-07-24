using Maytech.Quatrix.Tools.Net;
using System.Net;

namespace Maytech.Quatrix {
    
    public interface IQuatrixRequest {

        string CreateRequest ( string api_call );


        string CreateRequest ( string api_call, byte[] data );


        string CreateRequest ( string api_call, Method method, string signature_subject, byte[] data );


        T CreateRequest<T> ( string api_call ) where T : QEntity;


        T CreateRequest<T> ( string api_call, byte[] data ) where T : QEntity;


        T CreateRequest<T> ( string api_call, Method method, string signature_subject, byte[] data ) where T : QEntity;


        string ApiUri {
            get;
        }

        string Host {
            get;
        }


        string CreateApiUri( string api_call );


        string CreateHostUri ( string api_call );

        
        bool IsLogined {
            get;
        }


        string VersionApi {
            get;
        }
    }
}
