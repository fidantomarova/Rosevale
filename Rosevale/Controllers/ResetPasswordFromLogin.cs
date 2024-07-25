namespace Rosevale.Controllers
{
    internal class ResetPasswordFromLogin
    {
        public string Token { get; set; }
        public string Password { get; internal set; }
        public string Email { get; internal set; }
    }
}