namespace CTBApiApp.ModelView.DBView
{
    public class PlayerModelView
    {
        public int FIDEID { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Passord { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public double ELORating { get; set; }
        public string Contry { get; set; } = null!;
    }
}
