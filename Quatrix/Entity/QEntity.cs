
namespace Maytech.Quatrix {

    [System.Serializable]
    public abstract class QEntity : IQEntity {

        internal QEntity ( ) {

        }


        public QEntity ( IQuatrixRequest request ) {
            this.Request = request;
        }


        public IQuatrixRequest Request {
            get;
            internal set;
        }

        
        [Newtonsoft.Json.JsonProperty]
        public string id {
            get;
            internal set;
        }
    }
}
