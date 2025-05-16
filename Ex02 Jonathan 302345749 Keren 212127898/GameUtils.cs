namespace Ex02_Jonathan_302345749_Keren_212127898;

public static class GameUtils
{
    public const int k_NumberOfLettersPerGuess = 4;
    public const int k_NumberOfValidCharacters = 8;
    public const char k_FirstValidChar = 'A';
    public const char k_LastValidChar = 'H';
    public const char k_Quit = 'Q';
    public const string k_Yes = "Y";
    public const string k_No = "N";
    public const int k_MinimumNumberOfGuesses = 4;
    public const int k_MaximumNumberOfGuesses = 10;
    public const string k_InputInstructions = $"Please type your next guess <A B C D> or "
                                              + $"'Q' to quit";
    public const string k_NumberOfGuessesInstructions = $"Please enter the number of guesses or "
                                                        + $"Q to quit.";
}