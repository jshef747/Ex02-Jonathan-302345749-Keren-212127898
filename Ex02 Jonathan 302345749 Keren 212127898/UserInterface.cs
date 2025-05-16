namespace Ex02_Jonathan_302345749_Keren_212127898 ;

public class UserInterface
{
    private const int k_LengthOfFeedbackCell = 7;
    private const int k_HeadlineIndex = 0;
    private const int k_HashtagIndex = 1;
    private const string k_Row = "|         |       |";
    private const string k_Separator = "|=========|=======|";
    private static GameLogic? s_GameLogic;
    private static bool s_PlayerHitQuit;
    
    public void StartGame()
    {
        Console.WriteLine("Welcome to the game!");

        bool continuePlaying = true;

        while (continuePlaying && !s_PlayerHitQuit)
        {
            string guessFromUser = "";
            GameLogic.eGameStateIndicator gameStateIndicator;
            
            getTableSizeAndPrint();
            if (s_PlayerHitQuit)
            {
                break;
            }

            for (int currentGuessNumber = 1; currentGuessNumber <= s_GameLogic!.NumberOfGuesses; currentGuessNumber++)
            {
                guessFromUser = promptAndProcessGuess();
                if (s_PlayerHitQuit)
                {
                    break;
                }

                s_GameLogic.GenerateGuessFeedback(guessFromUser);
                gameStateIndicator = s_GameLogic.GetGameStateIndicator(currentGuessNumber);
                printTable(currentGuessNumber);
                if (gameStateIndicator == GameLogic.eGameStateIndicator.Won)
                {
                    Console.WriteLine($"You guessed after {currentGuessNumber} steps!");
                    continuePlaying = askToPlayAgain();
                    break;
                }

                if (gameStateIndicator == GameLogic.eGameStateIndicator.Lost)
                {
                    Console.WriteLine("No more guesses allowed. You Lost.");
                    continuePlaying = askToPlayAgain();
                    break;
                }
            }
        }

        //TODO
        Console.Clear();
        //Ex02.ConsoleUtils.Screen.Clear();
        Console.WriteLine("Goodbye!");
    }

    private string readStringFromUserAndHandleQuit()
    {
        string numberOfGuesses = "";
        ConsoleKeyInfo key = readCharAndHandleQuit();

        while (key.Key != ConsoleKey.Enter && !s_PlayerHitQuit)
        {
            numberOfGuesses += key.KeyChar;
            key = readCharAndHandleQuit();
        }
        
        Console.WriteLine();
        
        return numberOfGuesses;
    }
    
    private ConsoleKeyInfo readCharAndHandleQuit()
    {
        ConsoleKeyInfo keyInputFromUser = Console.ReadKey(true);
        
        while (keyInputFromUser.Key == ConsoleKey.Backspace)
        {
            keyInputFromUser = Console.ReadKey(true);
        }

        Console.Write(keyInputFromUser.KeyChar); // show it on screen as they type
        if (Char.ToUpper(keyInputFromUser.KeyChar) == GameUtils.k_Quit)
        {
            s_PlayerHitQuit = true;
        }

        return keyInputFromUser;
    }

    private bool askToPlayAgain()
    {
        Console.WriteLine($"Would you like to start a new game? ({GameUtils.k_Yes}/{GameUtils.k_No})");
        ConsoleKeyInfo playAgainAnswer = readCharAndHandleQuit();
        
        while (!InputValidator.YesOrNo(playAgainAnswer.KeyChar.ToString()) && !s_PlayerHitQuit)
        {
            Console.WriteLine();
            Console.WriteLine(InputValidator.BadInputMessage);
            playAgainAnswer = readCharAndHandleQuit();
        }

        return s_PlayerHitQuit
                          ? s_GameLogic!.PlayAgainOrNot(GameUtils.k_No)
                          : s_GameLogic!.PlayAgainOrNot(playAgainAnswer.KeyChar.ToString().ToUpper());
    }

    private void getTableSizeAndPrint()
    {
        //TODO
        Console.Clear();
        //Ex02.ConsoleUtils.Screen.Clear();
        Console.WriteLine(GameUtils.k_NumberOfGuessesInstructions);
        string numberOfGuesses = readStringFromUserAndHandleQuit();

        while (!InputValidator.IsValidGuessNumber(numberOfGuesses) && !s_PlayerHitQuit)
        {
            Console.WriteLine(InputValidator.BadInputMessage);
            numberOfGuesses = readStringFromUserAndHandleQuit();
        }

        if (s_PlayerHitQuit)
        {
            return;
        }

        if (s_GameLogic == null)
        {
            s_GameLogic = new GameLogic(int.Parse(numberOfGuesses));
        }
        else
        {
            s_GameLogic.NumberOfGuesses = int.Parse(numberOfGuesses);
        }

        printTable();
    }

    private void printTable(int i_GuessNumber = 0)
    {
        List<string> guessesHistory = s_GameLogic!.GetGuessFromHistory();
        List<string> feedbacksHistory = s_GameLogic.GetFeedbackHistory();
        int currentGuessNumberToPrintIndex = 0;

        //TODO
        Console.Clear();
        //Ex02.ConsoleUtils.Screen.Clear();
        Console.WriteLine("Current board status:");
        Console.WriteLine();
        for (int rowNumber = 0; rowNumber <= s_GameLogic.NumberOfGuesses; rowNumber++)
        {
            if (rowNumber == k_HeadlineIndex)
            {
                Console.WriteLine("|Pins:    |Result:|");
            }
            else if (rowNumber == k_HashtagIndex && i_GuessNumber == 0)
            {
                Console.WriteLine("| # # # # |       |");
            }
            else if (rowNumber <= i_GuessNumber) 
            {
                string guess = guessesHistory[currentGuessNumberToPrintIndex];
                string feedback = feedbacksHistory[currentGuessNumberToPrintIndex];

                Console.WriteLine($"|{formatGuessToPrint(guess)}|{formatFeedbackToPrint(feedback)}|");
                currentGuessNumberToPrintIndex++;
            }
            else
            {
                Console.WriteLine(k_Row);
            }

            Console.WriteLine(k_Separator);
        }
        
        Console.WriteLine();
    }

    private string formatGuessToPrint(string i_Guess)
    {
        return string.Format(
            " {0} {1} {2} {3} ",
            i_Guess[0],
            i_Guess[1],
            i_Guess[2],
            i_Guess[3]);
    }

    private string formatFeedbackToPrint(string i_Feedback)
    {
        string stringToPrint = "";
        int lengthOfFeedback = i_Feedback.Length;

        if (lengthOfFeedback > 0)
        {
            for (int i = 0; i < lengthOfFeedback; i++)
            {
                stringToPrint += i_Feedback[i];
                if (i != GameUtils.k_NumberOfLettersPerGuess - 1)
                {
                    stringToPrint += " ";
                }
            }

            int numberOfSpacesToAdd = (k_LengthOfFeedbackCell - (lengthOfFeedback * 2));
            numberOfSpacesToAdd = numberOfSpacesToAdd < 0 ? 0 : numberOfSpacesToAdd;
            stringToPrint += new string(' ', numberOfSpacesToAdd);
        }
        
        return stringToPrint;
    }
    
    private string promptAndProcessGuess()
    {
        int oldLeft = Console.CursorLeft;
        int oldTop = Console.CursorTop;

        Console.WriteLine(GameUtils.k_InputInstructions);
        string guessFromUser = getGuessInput();

        while (!InputValidator.IsValidGuessInput(guessFromUser) && !s_PlayerHitQuit)
        {
            clearGuessFromScreen(oldLeft, oldTop);
            Console.WriteLine(InputValidator.BadInputMessage);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0} {1} {2} {3}", guessFromUser[0], guessFromUser[1], guessFromUser[2], guessFromUser[3]);
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.CursorTop);
            guessFromUser = getGuessInput();
        }

        clearGuessFromScreen(oldLeft, oldTop);
        
        return guessFromUser;
    }

    private void clearGuessFromScreen(int i_OldLeft, int i_OldTop)
    {
        Console.SetCursorPosition(i_OldLeft, i_OldTop);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(i_OldLeft, i_OldTop);
    }

    private string getGuessInput()
    {
        string guessFromUser = "";

        for (int i = 0; i < GameUtils.k_NumberOfLettersPerGuess; i++)
        {
            ConsoleKeyInfo key = readCharAndHandleQuit();

            if (s_PlayerHitQuit)
            {
                break;
            }
            
            if (key.Key == ConsoleKey.Enter)
            {
                while (i < GameUtils.k_NumberOfLettersPerGuess)
                {
                    guessFromUser += ' ';
                    i++;
                }
            }
            else
            {
                guessFromUser += key.KeyChar;
                Console.Write(" ");
            }
        }

        return guessFromUser;
    }
}