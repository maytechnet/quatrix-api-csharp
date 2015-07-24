
namespace Maytech.Quatrix {

    [System.Serializable]
    public sealed class Account : QEntity {

        internal Account ( ) {
        
        }


        [Newtonsoft.Json.JsonProperty]
        public string account_id {
            get {
                return base.id;
            }
            internal set {
                base.id = value;
            }
        }
    }
}
