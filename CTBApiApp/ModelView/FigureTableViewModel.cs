namespace CTBApiApp.ModelView
{
    public class FigureTableViewModel
    {
        public int ID { get; set; }
        public string Figure { get; set; } = null!;
        public string Pozition { get; set; } = null!;
        public bool IsWhile { get; set; }
        public bool InGame { get; set; } = true;
        public bool IsMoving { get; set; } = false;
        public int EatID { get; set; } = 0;
    }
}
