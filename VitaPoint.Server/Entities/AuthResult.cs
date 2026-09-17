namespace VitaPoint.Server.Entities
{
    public class AuthResult
    {
        public bool Success { get; set; } = false;
        public string? Token { get; set; } = "";
        public string? RefreshToken { get; set; } = "";
        public string? ErrorMessage { get; set; } = "";
        public AuthErrorType ErrorType { get; set; } = AuthErrorType.ProcessFailed;
    }

    public enum AuthErrorType { None, NotFound, InvalidCredentials, ProcessFailed }
}
