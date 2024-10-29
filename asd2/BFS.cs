namespace asd2;

public static class Bfs
{
    private static int _nodesCount;
    private static int _iterations;
    public static Node? BreadthFirstSearch(Node startNode)
    {
        _nodesCount = 0;
        _iterations = 0;
        Queue<Node> q = new Queue<Node>();
        
        q.Enqueue(startNode);
        _nodesCount++;
        
        while (q.Count > 0)
        {
            Node current = q.Dequeue();
            
            if (!current.BoardState.HasConflicts())
            {
                current.TotalNodes = _nodesCount;
                current.NodesInMemory = _nodesCount;
                current.Iterations = _iterations;
                return current;
            }

            _iterations++;
            foreach (Node neighbor in current.Neighbors())
            {
                q.Enqueue(neighbor);
                _nodesCount++;
                if (!neighbor.BoardState.HasConflicts())
                {
                    neighbor.TotalNodes = _nodesCount;
                    neighbor.NodesInMemory = _nodesCount;
                    neighbor.Iterations = _iterations;
                    return neighbor;
                }
            }
        }

        return null;
    }
}