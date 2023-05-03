namespace CTBApiApp.ModelView
{
    public class MoveTableViewModel
    {
        public string Move { get; set; } = null!;
        public string Pozition { get; set; } = null!;
        public int ID { get; set; }
        public int PlayerID { get; set; }
        public int ConsignmentID { get; set; }
        public int TourID { get; set; }
        public bool LastMove { get; set; } = false;
        public bool Winner { get; set; } = false;
    }
}
