namespace Domain
{
    public record NeighboringBoardCell
    {
        public NeighboringBoardCell(BoardCell cell, BoardCell? nextCell, MoveDirection direction)
        {
            Cell = cell;
            NextCell = nextCell;
            Direction = direction;
        }

        public BoardCell Cell { get; }

        public BoardCell? NextCell { get; }
        
        public MoveDirection Direction { get; }
    }
}
