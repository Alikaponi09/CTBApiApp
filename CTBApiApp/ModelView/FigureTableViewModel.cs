namespace CTBApiApp.ModelView
{
    public class FigureTableViewModel
    {
        public string Name { get; set; } = null!;
        public string Pozition { get; set; } = null!;
        public bool IsWhile { get; set; }
        public int ID { get; set; }
        public bool InGame { get; set; } = true;
        public bool IsMoving { get; set; } = false;
    }
}
