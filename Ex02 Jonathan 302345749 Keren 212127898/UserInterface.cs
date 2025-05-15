namespace Ex02;

public static class UserInterface
{
    private const int k_LengthOfFeedbackCell = 7;
    private const string k_Row = "|         |       |";
    private const string k_Separator = "|=========|=======|";
    private static GameLogic? s_GameLogic;
    private static bool s_QuitGame;
    public static void StartGame()
    {
        Console.WriteLine("Welcome to the game!");

        bool continuePlaying = true;

        while (continuePlaying && !s_QuitGame)
        {
            getTableSizeAndPrint();
            if (s_QuitGame)
            {
                break;
            }

            for (int currentGuessNumber = 1; currentGuessNumber <= s_GameLogic!.NumberOfGuesses; currentGuessNumber++)
            {
                string guessFromUser = promptAndProcessGuess();
                if (s_QuitGame)
                {
                    break;
                }

                GameLogic.eGameStateIndicator gameStateIndicator =
                    s_GameLogic.GenerateGuessFeedback(guessFromUser, currentGuessNumber);

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

        //TODO change to his clear!!
        Console.Clear();
        Console.WriteLine("Goodbye!");
    }

    private static string readNonNullStringAndHandleQuit()
    {
        string? inputFromUser = Console.ReadLine();

        while (inputFromUser is null)
        {
            Console.WriteLine("Input cannot be null.");
            inputFromUser = Console.ReadLine();
        }

        if (inputFromUser.ToUpper() == GameUtils.k_Quit.ToString())
        {
            s_QuitGame = true;
        }

        return inputFromUser;
    }

    private static ConsoleKeyInfo readCharAndHandleQuit()
    {
        ConsoleKeyInfo keyInputFromUser = Console.ReadKey(true);
        while (keyInputFromUser.Key == ConsoleKey.Backspace)
        {
            keyInputFromUser = Console.ReadKey(true);
        }

        Console.Write(keyInputFromUser.KeyChar); // show it on screen as they type

        if (Char.ToUpper(keyInputFromUser.KeyChar) == GameUtils.k_Quit)
        {
            s_QuitGame = true;
        }

        return keyInputFromUser;
    }

    private static bool askToPlayAgain()
    {
        Console.WriteLine($"Would you like to start a new game? ({GameUtils.k_Yes}/{GameUtils.k_No})");
        ConsoleKeyInfo playAgainAnswer = readCharAndHandleQuit();
        bool returnValue;

        while (!InputValidator.YesOrNo(playAgainAnswer.KeyChar.ToString()) && !s_QuitGame)
        {
            playAgainAnswer = readCharAndHandleQuit();
        }

        returnValue = s_QuitGame
                          ? s_GameLogic!.PlayAgainOrNot(GameUtils.k_No)
                          : s_GameLogic!.PlayAgainOrNot(playAgainAnswer.KeyChar.ToString());

        return returnValue;
    }

    private static void getTableSizeAndPrint()
    {
        //TODO CHANGE TO HIS MAJESTIES!
        Console.Clear();
        Console.WriteLine(GameUtils.k_NumberOfGuessesInstructions);
        string numberOfGuesses = readNonNullStringAndHandleQuit();

        while (!InputValidator.IsValidGuessNumber(numberOfGuesses) && !s_QuitGame)
        {
            Console.WriteLine(InputValidator.BadInputMessage);
            numberOfGuesses = readNonNullStringAndHandleQuit();
        }

        if (s_QuitGame)
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

    private static void printTable(int i_GuessNumber = 0)
    {
        List<string> guessesHistory = s_GameLogic!.GetGuessFromHistory();
        List<string> feedbacksHistory = s_GameLogic.GetFeedbackHistory();
        int currentGuessNumberToPrintIndex = 0;

        Console.Clear();
        ////////////TODO//ConsoleUtils.Screen.Clear();
        Console.WriteLine("Current board status:");
        Console.WriteLine();

        for (int rowNumber = 0; rowNumber <= s_GameLogic.NumberOfGuesses; rowNumber++)
        {
            if (rowNumber == 0)  // print headline
            {
                Console.WriteLine("|Pins:    |Result:|");
            }
            else if (rowNumber == 1 && i_GuessNumber == 0)
            {
                Console.WriteLine("| # # # # |       |");
            }
            else if (rowNumber <= i_GuessNumber)  // print guess and feedback
            {
                string guess = guessesHistory[currentGuessNumberToPrintIndex];
                string feedback = feedbacksHistory[currentGuessNumberToPrintIndex];
                string formattedGuess = formatStrToPrint(0, guess);
                string formattedFeedback = formatStrToPrint(1, feedback);

                Console.WriteLine($"|{formattedGuess}|{formattedFeedback}|");

                currentGuessNumberToPrintIndex++;
            }
            else // print empty line
            {
                Console.WriteLine(k_Row);
            }

            Console.WriteLine(k_Separator);
        }

        Console.WriteLine();
    }

    private static string formatStrToPrint(int i_GuessOrFeedbackFlag, string i_StringToPrint)
    {
        string strToPrint = "";

        if (i_GuessOrFeedbackFlag == 0)
        {
            strToPrint = string.Format(
                " {0} {1} {2} {3} ",
                i_StringToPrint[0],
                i_StringToPrint[1],
                i_StringToPrint[2],
                i_StringToPrint[3]);
        }
        else
        {
            int lengthOfFeedback = i_StringToPrint.Length;

            if (lengthOfFeedback > 0)
            {
                string newStringToPrint = "";

                for (int i = 0; i < lengthOfFeedback; i++)
                {
                    newStringToPrint += i_StringToPrint[i];
                    if (i != GameUtils.k_NumberOfLettersPerGuess - 1)
                    {
                        newStringToPrint += " ";
                    }
                }

                int numberOfSpacesToAdd = (k_LengthOfFeedbackCell - (lengthOfFeedback * 2));
                numberOfSpacesToAdd = numberOfSpacesToAdd < 0 ? 0 : numberOfSpacesToAdd;
                newStringToPrint += new string(' ', numberOfSpacesToAdd);

                strToPrint = newStringToPrint;
            }
        }

        return strToPrint;
    }

    private static string promptAndProcessGuess()
    {
        int oldLeft = Console.CursorLeft;
        int oldTop = Console.CursorTop;

        Console.WriteLine(GameUtils.k_InputInstructions);

        string guessFromUser = getGuessInput();

        while (!InputValidator.IsValidGuessInput(guessFromUser) && !s_QuitGame)
        {
            clearGuessFromScreen(oldLeft, oldTop);
            Console.WriteLine(InputValidator.BadInputMessage);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0} {1} {2} {3}", guessFromUser[0], guessFromUser[1], guessFromUser[2], guessFromUser[3]);
            Console.ResetColor();
            int newTop = Console.CursorTop;
            Console.SetCursorPosition(0, newTop);
            guessFromUser = getGuessInput();
        }

        clearGuessFromScreen(oldLeft, oldTop);

        return guessFromUser;
    }

    private static void clearGuessFromScreen(int i_OldLeft, int i_OldTop)
    {
        Console.SetCursorPosition(i_OldLeft, i_OldTop);
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.WriteLine(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(i_OldLeft, i_OldTop);
    }

    private static string getGuessInput()
    {
        string guessFromUser = "";

        for (int i = 0; i < GameUtils.k_NumberOfLettersPerGuess; i++)
        {
            ConsoleKeyInfo key = readCharAndHandleQuit();

            if (s_QuitGame)
            {
                break;
            }

            // if hit enter, fill guess with spaces
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