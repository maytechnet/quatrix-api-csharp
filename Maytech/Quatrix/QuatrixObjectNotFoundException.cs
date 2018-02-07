using System;

namespace Maytech.Quatrix {

    public class QuatrixObjectNotFoundException : QuatrixException {

        internal QuatrixObjectNotFoundException(QuatrixExceptionArgs args, Exception inner) : base(args, inner) { }
    }
}
