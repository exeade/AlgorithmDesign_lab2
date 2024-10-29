namespace asd2;

static class Program
{
    private static readonly string[] MainMenuOptions = ["Select solving algorithm", "Select board input method", "Solve",
        "Exit"];
    private static readonly string[] SolvingAlgorithms = ["BFS", "RBFS"];
    private static readonly string[] BoardInputMethods = ["Input manually", "Generate automatically"];
        
    private static int _selectedMainOption;
    private static int _selectedSolvingAlgorithm = -1;
    private static int _selectedBoardInputMethod = -1;
    private static EightQueensBoard? _currentBoard;

    static void Main()
    {
        while (true)
        {
            DrawMainMenu();

            ConsoleKeyInfo key = Console.ReadKey();
                
            if (key.Key == ConsoleKey.DownArrow)
                _selectedMainOption = (_selectedMainOption + 1) % MainMenuOptions.Length;
            else if (key.Key == ConsoleKey.UpArrow)
                _selectedMainOption = (_selectedMainOption - 1 + MainMenuOptions.Length) % MainMenuOptions.Length;
            else if (key.Key == ConsoleKey.Enter)
            {
                switch (_selectedMainOption)
                {
                    case 0:
                        SelectSolvingAlgorithm();
                        break;
                    case 1:
                        SelectBoardInputMethod();
                        break;
                    case 2:
                        if (_selectedSolvingAlgorithm != -1 && _selectedBoardInputMethod != -1)
                        {
                            SolvePuzzle();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nPlease select both solving algorithm and board input method.\n" +
                                              "Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        return;
                }
            }
        }
    }

    private static void DrawMainMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.WriteLine("=== 8 Queens Problem Solver ===");
        Console.ResetColor();

        for (int i = 0; i < MainMenuOptions.Length; i++)
        {
            if (i == _selectedMainOption)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"> {MainMenuOptions[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"  {MainMenuOptions[i]}");
            }
        }

        Console.WriteLine();
        Console.Write($"Selected solving algorithm: ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"{(_selectedSolvingAlgorithm == -1 ? "none" : SolvingAlgorithms[_selectedSolvingAlgorithm])}");
        Console.ResetColor();
        Console.Write($"Selected board input method: ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"{(_selectedBoardInputMethod == -1 ? "none" : BoardInputMethods[_selectedBoardInputMethod])}");
        Console.ResetColor();
        
        if (_selectedBoardInputMethod == 1 && _currentBoard != null)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Generated board: ");
            Console.ResetColor();
            _currentBoard.PrintBoard();
        }
    }

    private static void SelectSolvingAlgorithm()
    {
        int selectedOption = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select solving algorithm:");

            for (int i = 0; i < SolvingAlgorithms.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {SolvingAlgorithms[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {SolvingAlgorithms[i]}");
                }
            }

            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.DownArrow)
                selectedOption = (selectedOption + 1) % SolvingAlgorithms.Length;
            else if (key.Key == ConsoleKey.UpArrow)
                selectedOption = (selectedOption - 1 + SolvingAlgorithms.Length) % SolvingAlgorithms.Length;
            else if (key.Key == ConsoleKey.Enter)
            {
                _selectedSolvingAlgorithm = selectedOption;
                break;
            }
        }
    }

    private static void SelectBoardInputMethod()
    {
        int selectedOption = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select board input method:");

            for (int i = 0; i < BoardInputMethods.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {BoardInputMethods[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {BoardInputMethods[i]}");
                }
            }

            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.DownArrow)
                selectedOption = (selectedOption + 1) % BoardInputMethods.Length;
            else if (key.Key == ConsoleKey.UpArrow)
                selectedOption = (selectedOption - 1 + BoardInputMethods.Length) % BoardInputMethods.Length;
            else if (key.Key == ConsoleKey.Enter)
            {
                _selectedBoardInputMethod = selectedOption;
                if (_selectedBoardInputMethod == 1)
                {
                    _currentBoard = new EightQueensBoard();
                    _currentBoard.GenerateRandomPosition();
                }
                else
                {
                    _currentBoard = null;
                }
                break;
            }
        }
    }

    private static void SolvePuzzle()
    {
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Solving the problem...");
        Console.ResetColor();

        EightQueensBoard game = new EightQueensBoard();

        if (_selectedBoardInputMethod == 0)
            game.InitializeBoardManually();
        else if (_selectedBoardInputMethod == 1)
            if (_currentBoard != null)
            {
                game = _currentBoard;
                game.PrintBoard();
            }

        Node? result = null;
        Node initial = new Node(game);
        
        if(_selectedSolvingAlgorithm == 0)
            result = Bfs.BreadthFirstSearch(initial);
        else if (_selectedSolvingAlgorithm == 1)
        {
            result = Rbfs.SearchRbfs(initial);
        }
        
        if (result != null)
        {
            Console.WriteLine("\nSolution found:");
            
            Node? current = result;
            List<Node?> path = new List<Node?>();
            
            while (current.Parent != null)
            {
                path.Add(current);
                current = current.Parent;
            }
            
            for (int i = path.Count - 1; i >= 0; i--)
            {
                path[i]?.BoardState.PrintBoard(path[i]?.Action);
            }
            
            result.BoardState.PrintBoard();
            Console.WriteLine("Algorithm Performance Summary:");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"{"Iterations:",-20} {result.Iterations,5}");
            Console.WriteLine($"{"Total Nodes:",-20} {result.TotalNodes,5}");
            Console.WriteLine($"{"Nodes in Memory:",-20} {result.NodesInMemory,5}");
            Console.WriteLine(new string('-', 40));
        }
        else
        {
            Console.WriteLine("\nNo solution found.");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nPress any key to return to main menu...");
        Console.ResetColor();
        Console.ReadKey();
    }
}
