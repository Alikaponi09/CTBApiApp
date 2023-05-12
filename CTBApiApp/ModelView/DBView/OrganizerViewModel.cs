namespace CTBApiApp.ModelView.DBView
{
    public class OrganizerViewModel
    {
        public int OrganizerID { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? Administrator { get; set; } = -1;
    }
}
