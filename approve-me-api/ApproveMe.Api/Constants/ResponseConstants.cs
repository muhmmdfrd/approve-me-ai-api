namespace ApproveMe.Api.Constants;

public class ResponseConstant
{
    #region Code
    // Success Code
    public const string SUCCESS_CODE = "0000";

    // Error Code
    public const string INTERNAL_SERVER_ERROR_CODE = "5000";
    public const string BAD_REQUEST_CODE = "4000";
    public const string FORBIDDEN_CODE = "4300";
    public const string UNAUTHORIZED_CODE = "4400";
    public const string SESSION_EXPIRED_CODE = "4100";
    public const string RECORD_NOT_FOUND_CODE = "4512";
    public const string DATABASE_CONNECTION_CODE = "3000";
    public const string DATABASE_UNIQUE_CODE = "3013";
    public const string FAILED_CODE = "9999";
    #endregion

    #region Message
    // Success Message
    public const string SUCCESS_MESSAGE = "Success.";
    public const string CREATED_MESSAGE = "Data created.";
    
    // Error Message
    public const string INTERNAL_SERVER_ERROR = "Internal server error.";
    public const string BAD_REQUEST_MESSAGE = "Bad request.";
    public const string UNAUTHORIZED_MESSAGE = "Unauthorized.";
    public const string SESSION_EXPIRED_MESSAGE = "Sesssion is expired.";
    public const string FORBIDDEN_MESSAGE = "Forbidden.";
    public const string DATABASE_CONNECTION = "Unable to connect db server.";
    public const string FAILED_MESSAGE = "Something went wrong.";

    #endregion
}