using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Board
    {
        private readonly List<BoardCell> cells = new();

        public Board()
        {
            InitializeCells();
        }

        public IReadOnlyList<BoardCell> Cells => cells.AsReadOnly();

        public BoardCell GetCell(Position position)
        {
            return cells.Find(cell => cell.Position == position)!;
        }

        public BoardCell GetFirstCell()
        {
            return cells.First();
        }

        public BoardCell GetLastCell()
        {
            return cells.Last();
        }

        public Result<BoardCell> GetNextCell(Position origin)
        {
            var next = cells
                .SkipWhile(cell => cell.Position != origin)
                .Skip(1)
                .FirstOrDefault();

            if (next is null)
            {
                return Result<BoardCell>.Failure($"There's no cell after {origin}.");
            }

            return next;
        }

        public Result<BoardCell> GetPreviousCell(Position origin)
        {
            var previous = cells
                .Reverse<BoardCell>()
                .SkipWhile(cell => cell.Position != origin)
                .Skip(1)
                .FirstOrDefault();

            if (previous is null)
            {
                return Result<BoardCell>.Failure($"There's no cell before {origin}.");
            }

            return previous;
        }

        public IReadOnlyList<NeighboringBoardCell> ListNeighboringCells(Position origin, Player asPlayer)
        {
            var neighbors = new List<NeighboringBoardCell>();
            void AddPositionsIfValid(Result<Position> position, Result<Position> nextPosition, MoveDirection direction)
            {
                if (position.IsSuccess)
                {
                    neighbors.Add(new NeighboringBoardCell(
                        GetCell(position.Value!),
                        nextPosition.IsSuccess ? GetCell(nextPosition.Value!) : null,
                        direction));
                }
            }

            AddPositionsIfValid(
                Position.Try(origin.Row.Number + 1, origin.Column.Number - 1),
                Position.Try(origin.Row.Number + 2, origin.Column.Number - 2),
                asPlayer == Player.Player1 ? MoveDirection.Advancing : MoveDirection.Receding);

            AddPositionsIfValid(
                Position.Try(origin.Row.Number + 1, origin.Column.Number + 1),
                Position.Try(origin.Row.Number + 2, origin.Column.Number + 2),
                asPlayer == Player.Player1 ? MoveDirection.Advancing : MoveDirection.Receding);

            AddPositionsIfValid(
                Position.Try(origin.Row.Number - 1, origin.Column.Number - 1),
                Position.Try(origin.Row.Number - 2, origin.Column.Number - 2),
                asPlayer == Player.Player1 ? MoveDirection.Receding : MoveDirection.Advancing);

            AddPositionsIfValid(
                Position.Try(origin.Row.Number - 1, origin.Column.Number + 1),
                Position.Try(origin.Row.Number - 2, origin.Column.Number + 2),
                asPlayer == Player.Player1 ? MoveDirection.Receding : MoveDirection.Advancing);

            return neighbors.AsReadOnly();
        }

        private void InitializeCells()
        {
            foreach (var row in Enumerable.Range(Row.MinRowNumber, Row.MaxRowNumber))
            {
                foreach (var column in Enumerable.Range(Column.MinColumnNumber, Column.MaxColumnNumber))
                {
                    var position = Position.Try(row, column);
                    if (position.IsSuccess)
                    {
                        cells.Add(new BoardCell(position.Value!));
                    }
                }
            }
        }
    }
}
