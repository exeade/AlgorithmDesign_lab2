namespace asd2;

public class Move(
    int[,]? boardState,
    (int Row, int Col) fromPosition,
    (int Row, int Col) toPosition)
{
    public int[,]? BoardState { get; } = boardState;
    public (int Row, int Col) FromPosition { get; } = fromPosition;
    public (int Row, int Col) ToPosition { get; } = toPosition;
}