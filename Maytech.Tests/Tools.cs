using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maytech.Quatrix;
using Maytech.Quatrix.Entity;
using  Maytech.Quatrix.Operations;


namespace Maytech.Tests {


    class Tools {


        public static void DeleteContact( IQuatrixRequest request, string id ) {
            string api_call = string.Concat("/contact/delete/", id);
            request.CreateRequest<Contact>( api_call );
        }


		public static void DeleteEmptyFiles( IQuatrixRequest request, string id ) {
			Metadata mt = FileOperations.GetMetadata( request, id );
			foreach( Metadata m in mt.Content )
				if( m.size == 0 ) {
					m.Delete();
				}
		}		
    }
}
