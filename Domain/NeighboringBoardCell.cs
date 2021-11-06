using System.Collections.Generic;

namespace Domain
{
    public class NeighboringBoardCell : ValueObject
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Cell;
            yield return NextCell;
            yield return Direction;
        }
    }
}
