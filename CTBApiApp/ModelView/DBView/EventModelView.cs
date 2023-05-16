namespace CTBApiApp.ModelView.DBView
{
    public class EventModelView
    {
        public int EventID { get; set; }
        public string Name { get; set; } = null!;
        public int PrizeFund { get; set; }
        public string LocationEvent { get; set; } = null!;
        public DateTime DataStart { get; set; }
        public DateTime DataFinish { get; set; }
        public int StatusID { get; set; }
        public int OrganizerID { get; set; }
        public bool IsPublic { get; set; }
        public bool TypeEvent { get; set; }
    }
}