namespace asd2;

public static class Rbfs
{
    private static int _nodesInMemory;
    private static int _allNodesCount;
    private static int _iterations;
    private static int Func(Node node)
    {
        int[,] state = node.BoardState.ThisBoard;
        int n = state.GetLength(0);
        int m = state.GetLength(1);
        int conflictsNum = 0;
        
        for (int i = 0; i < n; i++)
        {
            int rowQueensNumber = 0;
            for (int j = 0; j < m; j++)
            {
                if (state[i, j] == 1)
                {
                    rowQueensNumber++;
                }
            }
            if (rowQueensNumber > 1)
            {
                conflictsNum += rowQueensNumber - 1;
            }
        }

        for (int j = 0; j < m; j++)
        {
            int colQueensNumber = 0;
            for (int i = 0; i < n; i++)
            {
                if (state[i, j] == 1)
                {
                    colQueensNumber++;
                }
            }
            if (colQueensNumber > 1)
            {
                conflictsNum += colQueensNumber - 1;
            }
        }
        
        conflictsNum += CheckDiagonals(state, n, m, true);
        
        conflictsNum += CheckDiagonals(state, n, m, false);

        return conflictsNum;
    }
    
    private static int CheckDiagonals(int[,] state, int n, int m, bool isMainDiagonal)
    {
        int conflicts = 0;
        
        for (int startCol = 0; startCol < m; startCol++)
        {
            int queensNumber = 0;
            int row = 0;
            int col = startCol;

            while (row < n && col >= 0 && col < m)
            {
                if (state[row, col] == 1)
                {
                    queensNumber++;
                }
                row++;
                col = isMainDiagonal ? col + 1 : col - 1;
            }

            if (queensNumber > 1)
            {
                conflicts += queensNumber - 1;
            }
        }
        
        for (int startRow = 1; startRow < n; startRow++)
        {
            int queensNumber = 0;
            int row = startRow;
            int col = isMainDiagonal ? 0 : m - 1;

            while (row < n && col >= 0 && col < m)
            {
                if (state[row, col] == 1)
                {
                    queensNumber++;
                }
                row++;
                col = isMainDiagonal ? col + 1 : col - 1;
            }

            if (queensNumber > 1)
            {
                conflicts += queensNumber - 1;
            }
        }

        return conflicts;
    }

    private static (Node?, int) RecursiveBestFirstSearch(Node node, int fLimit)
    {
        if (!node.BoardState.HasConflicts())
        {
            return (node, Func(node));
        }

        List<Node> successors = node.Neighbors();
        _iterations++;

        foreach (Node s in successors)
        {
            s.HeuristicsVal = Func(s);
            s.FValue = Math.Max(s.Depth + s.HeuristicsVal, node.FValue);
        }
        
        successors.Sort((node1, node2) => node1.FValue.CompareTo(node2.FValue));
        
        if (successors.Count == 0)
        {
            return (null, Int32.MaxValue);
        }

        _allNodesCount += successors.Count;
        _nodesInMemory += successors.Count;
        
        while (true)
        {
            Node best = successors[0];
            
            if (best.FValue > fLimit)
            {
                _nodesInMemory -= successors.Count;
                return (null, best.FValue);
            }

            int alternative = successors[1].FValue;
            
            (Node? result, best.FValue) = RecursiveBestFirstSearch(best, Math.Min(fLimit, alternative));

            if (result != null)
            {
                return (result, result.FValue);
            }
            
            successors.Sort((node1, node2) => node1.FValue.CompareTo(node2.FValue));
        }
    }

    public static Node? SearchRbfs(Node node)
    {
        node.FValue = node.Depth + Func(node);
        _iterations = 0;
        _nodesInMemory = 1;
        _allNodesCount = 1;
        (Node? result, _) = RecursiveBestFirstSearch(node, Int32.MaxValue);
        if (result != null)
        {
            result.TotalNodes = _allNodesCount;
            result.NodesInMemory = _nodesInMemory;
            result.Iterations = _iterations;
        }
        return result;
    }

}