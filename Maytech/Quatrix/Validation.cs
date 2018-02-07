using Maytech.Quatrix.Messages;
using System;
using System.Net.Mail;

namespace Maytech.Quatrix {

    internal static class Validation {

        public const int HOST_MAX_VALIDATION_TIME = 2500;   //2.5 sec
        public const int MIN_PASSWORD_LENGTH = 6;
        public const int MAX_PASSWORD_LENGTH = 64;
        public const int MIN_CHUNK_SIZE = 1000;             //1 kB
        public const int AUTHORIZATION_CODE_LENGTH = 6;
        public const int USER_PIN_LENGTH = 5;

        /// <summary>
        /// Determines whether specified [email is valid ].
        /// </summary>
        /// <param name="email">email to validating.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <returns>True if valid. Otherwise false </returns>
        public static MailAddress Email(string email) {
            return new MailAddress(email);       //this initialization can throw exception
        }

        /// <summary>
        /// Determines whether [ password is valid].
        /// Valid password should be more than 6 and less then 64 symbols
        /// </summary>
        /// <param name="password">Password to validate.</param>
        /// <returns>True if valid. Otherwise - Exception</returns>
        /// <exception cref="NullReferenceException">If password parameter is empty</exception>
        /// <exception cref="ArgumentException">
        /// if password length is longer then 64 or shorter than 6 symbols
        /// </exception>
        internal static bool Password(string password) {
            if(string.IsNullOrEmpty(password)) {
                throw new NullReferenceException(Error.api_parameter_password_empty);
            }
            if(password.Length < MIN_PASSWORD_LENGTH) {
                throw new ArgumentException(Error.api_parameter_passwortd_too_short);
            }
            if(password.Length > MAX_PASSWORD_LENGTH) {
                throw new ArgumentException(Error.api_parameter_password_too_long);
            }
            return true;
        }

        internal static bool AuthorizaionCode(string code) {
            if(string.IsNullOrEmpty(code)) {
                throw new QuatrixParameterException(new QuatrixExceptionArgs { msg = Error.authorization_code_required, code = QuatrixException.MISSING_PARAMETER });
            }
            if(code.Length != AUTHORIZATION_CODE_LENGTH) {
                throw new QuatrixParameterException(new QuatrixExceptionArgs { msg = Error.authorizaion_code_invalid, code = QuatrixException.BAD_PARAMETER_VALUE });
            }
            return true;
        }

        internal static bool UserPin(string code) {
            if(string.IsNullOrEmpty(code)) {
                throw new QuatrixParameterException(new QuatrixExceptionArgs { msg = Error.authorization_code_required, code = QuatrixException.MISSING_PARAMETER });
            }
            if(code.Length != USER_PIN_LENGTH) {
                throw new QuatrixParameterException(new QuatrixExceptionArgs { msg = Error.authorizaion_code_invalid, code = QuatrixException.BAD_PARAMETER_VALUE });
            }
            return true;
        }

        internal static bool ChunkSize(int size) {
            if(size < MIN_CHUNK_SIZE) {
                throw new ArgumentException(Error.invalid_chunk_size);
            }
            return true;
        }
    }
}
