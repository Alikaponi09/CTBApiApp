using CTBApiApp.Models;

namespace CTBApiApp.ModelView.DBView
{
    public class ConsignmentPlayerModelView
    {
        public int ConsignmentPlayerId { get; set; }

        public int ConsignmentId { get; set; }

        public int PlayerId { get; set; }

        public bool IsWhile { get; set; }

        public double? Result { get; set; }

        public virtual PlayerModelView Player { get; set; } = null!;
    }
}
