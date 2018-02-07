using System;

namespace Maytech.Quatrix {


    [Flags]
    public enum SessionStatus {
        Unauthorized = 0,
        ContactAuthorized = 4,
        PartialLoggedIn = 8,
        AuthorizationCodeRequired = 16,
        UserPinRequired = 32,
        LoggedIn = 1024
    }
}
