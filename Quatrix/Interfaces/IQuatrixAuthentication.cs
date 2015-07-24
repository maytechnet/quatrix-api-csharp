
namespace Maytech.Quatrix {

    public interface IQuatrixAuthentication {


        string Email {
            get;
            set;
        }


        string Password {
            get;
            set;
        }


        bool IsLogined {
            get;
        }


        bool Login();


        bool CheckPassword( string password );


        bool Logout();


        bool KeepAlive ( );
    }
}
