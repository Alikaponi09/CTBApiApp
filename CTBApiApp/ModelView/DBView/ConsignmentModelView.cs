using CTBApiApp.Models;

namespace CTBApiApp.ModelView.DBView
{
    public class ConsignmentModelView
    {
        public int ConsignmentID { get; set; }
        public int TourID { get; set; }
        public int StatusID { get; set; }
        public DateTime DateStart { get; set; }
        public string? GameMove { get; set; }
        public string? TableName { get; set; }

        public ConsignmentPlayerModelView WhitePlayer { get; set; } = null!;
        public ConsignmentPlayerModelView BlackPlayer { get; set; } = null!;
    }
}