namespace Ex02_Jonathan_302345749_Keren_212127898;

public static class InputValidator
{
    public static string? BadInputMessage { private set; get; }

    private static bool checkUniqueCharacters(string i_Guess)
    {
        bool[] uniqueLetterInGuessFlagArray = new bool[GameUtils.k_NumberOfValidCharacters];
        int countOfUniqueLetters = 0;

        foreach (char c in i_Guess)
        {
            uniqueLetterInGuessFlagArray[c - GameUtils.k_FirstValidChar] = true;
        }

        foreach (bool uniqueLetterInGuess in uniqueLetterInGuessFlagArray)
        {
            if (uniqueLetterInGuess)
            {
                countOfUniqueLetters++;
            }
        }

        return countOfUniqueLetters == 4;
    }

    private static bool isValidCharacters(string i_Guess)
    {
        bool validGuessLetters = true;

        foreach (char c in i_Guess)
        {
            if (c < GameUtils.k_FirstValidChar || c > GameUtils.k_LastValidChar)
            {
                validGuessLetters = false;
            }
        }

        return validGuessLetters;
    }

    public static bool IsValidGuessInput(string i_Guess)
    {
        bool isValidGuess = true;

        if (i_Guess.Length != GameUtils.k_NumberOfLettersPerGuess)
        {
            BadInputMessage = $"Input must be {GameUtils.k_NumberOfLettersPerGuess} letters long!"
                              + $" Please try again or press '{GameUtils.k_Quit}' to quit.";
            isValidGuess = false;
        }
        else if (!isValidCharacters(i_Guess))
        {
            BadInputMessage = $"Input must consist the letters between '{GameUtils.k_FirstValidChar}'"
                              + $" and '{GameUtils.k_LastValidChar}' only!"
                              + $" Please try again or press '{GameUtils.k_Quit}' to quit.";
            isValidGuess = false;
        }
        else if (!checkUniqueCharacters(i_Guess))
        {
            BadInputMessage = $"Each character must be different than the others!"
                              + $" Please try again or press '{GameUtils.k_Quit}' to quit.";
            isValidGuess = false;
        }

        return isValidGuess;
    }

    public static bool IsValidGuessNumber(string i_Guess)
    {
        bool isValidGuessNumber = int.TryParse(i_Guess, out int guessInt);

        if (!isValidGuessNumber)
        {
            BadInputMessage = $"The number of guesses must be an integer! "
                              + $"Please try again or press '{GameUtils.k_Quit}' to quit.";
            isValidGuessNumber = false;
        }
        else if (guessInt < GameUtils.k_MinimumNumberOfGuesses || guessInt > GameUtils.k_MaximumNumberOfGuesses)
        {
            BadInputMessage = $"The number of guesses must be between {GameUtils.k_MinimumNumberOfGuesses}"
                              + $" and {GameUtils.k_MaximumNumberOfGuesses}! "
                              + $"Please try again or press '{GameUtils.k_Quit}' to quit.";
            isValidGuessNumber = false;
        }

        return isValidGuessNumber;
    }

    public static bool YesOrNo(string i_YesOrNoInput)
    {
        bool isValidAnswer = true;

        if (i_YesOrNoInput.Length != 1)
        {
            BadInputMessage = $"Input must be a single character! ({GameUtils.k_Yes}/{GameUtils.k_No})";
            isValidAnswer = false;
        }
        else if (i_YesOrNoInput[0].ToString().ToUpper() != GameUtils.k_Yes && i_YesOrNoInput[0].ToString().ToUpper() != GameUtils.k_No)
        {
            BadInputMessage = $"Input must be a valid character! ({GameUtils.k_Yes}/{GameUtils.k_No})";
            isValidAnswer = false;
        }

        return isValidAnswer;
    }
}