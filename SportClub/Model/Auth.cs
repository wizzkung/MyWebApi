namespace SportClub.Model
{
    public class Auth
    {

    }

    public class Users
    {
        public string login { get; set; }
        public string password { get; set; }
    }

    public class ReturnStatus
    {
        public string? result { get; set; }
        public string? error { get; set; }
        public StatusEnum status { get; set; }
    
    }

    public enum StatusEnum
    {
        OK = 1,
        ERROR = 0
    }
    public class UserData
    {
        public bool IsAuthenticated { get; set; }
        public string Status { get; set; }
        public string UserType { get; set; }
    }

    public class AuthResponse
    {
        public string Status { get; set; }
        public string Token { get; set; }
        public string UserStatus { get; set; }
        public string UserType { get; set; }
        public string Message { get; set; }
    }
}
