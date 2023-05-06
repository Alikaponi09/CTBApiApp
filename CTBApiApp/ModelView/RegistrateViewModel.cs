namespace CTBApiApp.ModelView
{
    public class RegistrateViewModel
    {
        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public string? LastName { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
