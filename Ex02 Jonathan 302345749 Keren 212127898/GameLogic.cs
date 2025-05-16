namespace Ex02_Jonathan_302345749_Keren_212127898;

public class GameLogic
{
    private const char k_HitAndSameIndex = 'V';
    private const char k_HitAndWrongIndex = 'X';
    private readonly RandomGameWord r_RandomGameWord;
    private readonly GuessHistory r_MGuessHistory;
    public int NumberOfGuesses { get; set; }
    private int NumberOfv { get; set; }

    public enum eGameStateIndicator
    {
        Won,
        Lost,
        Continue
    }

    private class RandomGameWord
    {
        public string? RandomWord { private set; get; }
        private readonly Random r_Random;

        public RandomGameWord()
        {
            r_Random = new Random();
            generateRandomWord();
        }

        public void Reset()
        {
            generateRandomWord();
        }

        private void generateRandomWord()
        {
            string randomWordToBuild = "";

            while (randomWordToBuild.Length < GameUtils.k_NumberOfLettersPerGuess)
            {
                char randomLetter = (char)(GameUtils.k_FirstValidChar + r_Random.Next(GameUtils.k_NumberOfValidCharacters));

                if (!randomWordToBuild.Contains(randomLetter))
                {
                    randomWordToBuild += randomLetter;
                }
            }

            RandomWord = randomWordToBuild;
        }
    }


    public GameLogic(int i_NumberOfGuess)
    {
        r_RandomGameWord = new RandomGameWord();
        r_MGuessHistory = new GuessHistory();
        NumberOfGuesses = i_NumberOfGuess;
    }

    private void reset()
    {
        r_RandomGameWord.Reset();
        r_MGuessHistory.Reset();
    }

    public bool PlayAgainOrNot(string i_PlayAgain)
    {
        bool willPlayAgain = false;

        if (i_PlayAgain == GameUtils.k_Yes)
        {
            reset();
            willPlayAgain = true;
        }

        return willPlayAgain;
    }

    public void GenerateGuessFeedback(string i_Guess)
    {
        string guessFeedback = "";
        int numberOfX = countHitsAndMisplaced(i_Guess);

        for (int i = 0; i < NumberOfv; i++)
        {
            guessFeedback += k_HitAndSameIndex;
        }

        for (int i = 0; i < numberOfX; i++)
        {
            guessFeedback += k_HitAndWrongIndex;
        }

        r_MGuessHistory.AddGuess(i_Guess);
        r_MGuessHistory.AddFeedback(guessFeedback);
    }

    private int countHitsAndMisplaced(string i_Guess)
    {
        NumberOfv = 0;
        int numberOfX = 0;
        
        foreach (char letter in i_Guess)
        {
            if (r_RandomGameWord.RandomWord != null)
            {
                if (r_RandomGameWord.RandomWord.Contains(letter)
                    && r_RandomGameWord.RandomWord.IndexOf(letter) == i_Guess.IndexOf(letter))
                {
                    NumberOfv++;
                }
                else if (r_RandomGameWord.RandomWord.Contains(letter))
                {
                    numberOfX++;
                }
            }
        }
        
        return numberOfX;
    }
    
    public eGameStateIndicator GetGameStateIndicator(int i_CurrentGuessNumber)
    {
        eGameStateIndicator gameStateIndicator;

        if (NumberOfv == GameUtils.k_NumberOfLettersPerGuess)
        {
            gameStateIndicator = eGameStateIndicator.Won;
        }
        else if (i_CurrentGuessNumber < NumberOfGuesses)
        {
            gameStateIndicator = eGameStateIndicator.Continue;
        }
        else
        {
            gameStateIndicator = eGameStateIndicator.Lost;
        }

        return gameStateIndicator;
    }

    public List<string> GetGuessFromHistory()
    {
        return r_MGuessHistory.Guesses;
    }

    public List<string> GetFeedbackHistory()
    {
        return r_MGuessHistory.Feedback;
    }
}