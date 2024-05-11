namespace ECommerce.Core.Helpers
{
    /// <summary>
    /// Contains constant values used throughout the application.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Represents the maximum value of a decimal number as a string.
        /// </summary>
        public const string MAX_DECIMAL_VALUE_STRING = "79228162514264337593543950335";

        /// <summary>
        /// Represents a generic error message displayed to users when an unexpected error occurs.
        /// </summary>
        public const string GENERIC_ERROR_MESSAGE = "An error occurred. Please, try again later.";

        #region UserRoles

        /// <summary>
        /// Represents the role of an administrator user.
        /// </summary>
        public const string ROLE_ADMIN = "Admin";

        /// <summary>
        /// Represents the role of a customer user.
        /// </summary>
        public const string ROLE_CUSTOMER = "Customer";

        #endregion
    }
}
