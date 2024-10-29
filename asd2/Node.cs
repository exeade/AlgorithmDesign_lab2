namespace asd2;

public class Node
{
    public int Iterations;
    public int TotalNodes;
    public int NodesInMemory;
    public EightQueensBoard BoardState { get; }
    public Node? Parent { get; }
    public Move? Action { get; }
    public int Depth { get; }
    public int HeuristicsVal { get; set; }
    public int FValue { get; set; }
    public Node(EightQueensBoard boardState)
    {
        BoardState = boardState;
        Parent = null;
        Action = null;
        Depth = 0;
    }

    private Node(EightQueensBoard boardState, Node parent, Move action)
    {
        BoardState = boardState;
        Parent = parent;
        Action = action;
        Depth = parent.Depth + 1;
    }

    public List<Node> Neighbors()
    {
        List<Node> newNeighbors = new List<Node>();
        
        foreach (Move move in BoardState.GetPossibleMoves())
        {
            newNeighbors.Add(new Node(new EightQueensBoard(move.BoardState), this, move));
        }
        
        return newNeighbors;
    }
    
}