namespace asd2;

    public enum MoveDirection
    {
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft
    }
    public class EightQueensBoard
    {
        private static readonly Random Random = new Random();

        public readonly int[,] ThisBoard;

        public EightQueensBoard(int[,]? board = null)
        {
            ThisBoard = board ?? new int[8, 8];
            if (board == null)
            {
                GenerateEmptyBoard();
            }
        }

        private void GenerateEmptyBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ThisBoard[i, j] = 0;
                }
            }
        }

        public void GenerateRandomPosition()
        {
            for (int i = 0; i < 8; i++)
            {
                int row = i;
                int col = Random.Next(0, 8);
                ThisBoard[row, col] = 1;
            }
            
            PrintBoard();
        }
        
        public void InitializeBoardManually()
        {
            PrintBoard();
            
            List<(int row, int col)> queenPositions = new();
    
            Console.WriteLine("Please enter the positions of 8 queens (e.g., A7, b2).");

            while (queenPositions.Count < 8)
            {
                string? input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid position.");
                    Console.ResetColor();
                    continue;
                }

                if (input.Length < 2 || input.Length > 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid position format. Please enter in the format 'A1' to 'H8'.");
                    Console.ResetColor();
                    continue;
                }

                char column = char.ToUpper(input[0]);
                char rowChar = input[1];

                if (column < 'A' || column > 'H' || rowChar < '1' || rowChar > '8')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid position. Columns must be A-H and rows must be 1-8.");
                    Console.ResetColor();
                    continue;
                }

                int col = column - 'A';
                int row = 8 - (rowChar - '0');

                if (ThisBoard[row, col] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("A queen is already placed at that position. Please choose another.");
                    Console.ResetColor();
                    continue;
                }

                ThisBoard[row, col] = 1;
                queenPositions.Add((row, col)); 
                Console.WriteLine($"Queen placed at {input}. Current count: {queenPositions.Count}.");
            }

            PrintBoard();
        }

        public void PrintBoard(Move? move = null)
        {
            char[] columns = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];
            int marginLeft = 4;

            Console.Write(new string(' ', marginLeft + 1));
            foreach (char column in columns)
            {
                Console.Write($" {column} ");
            }
            Console.WriteLine();

            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{8 - i} ");

                for (int n = 0; n < marginLeft; n++)
                {
                    Console.Write(" ");
                }

                for (int j = 0; j < 8; j++)
                {
                    Console.BackgroundColor = (i + j) % 2 == 0 ? ConsoleColor.White : ConsoleColor.Gray;
                    
                    if (move != null && move.FromPosition.Row == i && move.FromPosition.Col == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" Q ");
                    }
                    else if (move != null && move.ToPosition.Row == i && move.ToPosition.Col == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" Q ");
                    }
                    else if (ThisBoard[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(" Q ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }

                    Console.ResetColor();
                }
                Console.WriteLine($" {8 - i}");
            }

            Console.Write(new string(' ', marginLeft + 1));
            foreach (char column in columns)
            {
                Console.Write($" {column} ");
            }
            Console.WriteLine("\n---------------------------------\n");
        }
        
        public bool HasConflicts()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ThisBoard[i, j] == 1)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            if (k != i && ThisBoard[k, j] == 1)
                                return true;
                        }
                        
                        for (int n = 0; n < 8; n++)
                        {
                            if (n != j && ThisBoard[i, n] == 1)
                                return true;
                        }
                        
                        for (int k = 1; k < 8; k++)
                        {
                            if (i - k >= 0 && j - k >= 0 && ThisBoard[i - k, j - k] == 1)
                                return true;

                            if (i - k >= 0 && j + k < 8 && ThisBoard[i - k, j + k] == 1)
                                return true;

                            if (i + k < 8 && j - k >= 0 && ThisBoard[i + k, j - k] == 1)
                                return true; 

                            if (i + k < 8 && j + k < 8 && ThisBoard[i + k, j + k] == 1)
                                return true; 
                        }
                    }
                }
            }

            return false; 
        }

        public List<Move> GetPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();
            int[,] currentBoard = CopyBoard(ThisBoard);

            for (int queenIndex = 0; queenIndex < 8; queenIndex++)
            {
                int queenCount = -1;
                int currentRow = -1;
                int currentCol = -1;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (currentBoard[i, j] == 1)
                        {
                            queenCount++;
                            if(queenCount == queenIndex)
                            {
                                currentRow = i;
                                currentCol = j;
                                break;
                            }
                        }
                    }
                    if (currentRow != -1) break;
                }

                if (currentRow == -1) continue;

                foreach (MoveDirection direction in Enum.GetValues(typeof(MoveDirection)))
                {
                    int[,]? newBoardState = MoveQueen(currentBoard, (currentRow, currentCol), direction,
                        out var newPosition);
                    if (newBoardState != null) possibleMoves.Add(new Move(newBoardState,
                        (currentRow, currentCol), newPosition));
                }
            }

            return possibleMoves;
        }

        private int[,]? MoveQueen(int[,] board, (int row, int col) queenPosition, MoveDirection direction,
            out (int newRow, int newCol) newPosition)
        {
            int newRow = queenPosition.row;
            int newCol = queenPosition.col;
    
            switch (direction)
            {
                case MoveDirection.Left:
                    newCol -= 1;
                    break;
                case MoveDirection.TopLeft:
                    newRow -= 1;
                    newCol -= 1;
                    break;
                case MoveDirection.Top:
                    newRow -= 1;
                    break;
                case MoveDirection.TopRight:
                    newRow -= 1;
                    newCol += 1;
                    break;
                case MoveDirection.Right:
                    newCol += 1;
                    break;
                case MoveDirection.BottomRight:
                    newRow += 1;
                    newCol += 1;
                    break;
                case MoveDirection.Bottom:
                    newRow += 1;
                    break;
                case MoveDirection.BottomLeft:
                    newRow += 1;
                    newCol -= 1;
                    break;
            }
    
            if (newRow < 0 || newRow >= 8 || newCol < 0 || newCol >= 8 || board[newRow, newCol] == 1)
            {
                newPosition = (newRow, newCol);
                return null;
            }
    
            int[,] newBoardState = CopyBoard(board);
            newBoardState[queenPosition.row, queenPosition.col] = 0;
            newBoardState[newRow, newCol] = 1;

            newPosition = (newRow, newCol);
            return newBoardState;
        }
        
        private static int[,] CopyBoard(int[,] originalBoard)
        {
            int[,] newBoard = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    newBoard[i, j] = originalBoard[i, j];
                }
            }
            return newBoard;
        }
    }
