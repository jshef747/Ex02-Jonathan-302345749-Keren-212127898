namespace Ex02;

public class GuessHistory
{
    public List<string> Guesses { get; } = new();
    public List<string> Feedback { get; } = new();

    public void Reset()
    {
        Guesses.Clear();
        Feedback.Clear();
    }

    public void AddGuess(string i_Guess)
    {
        Guesses.Add(i_Guess);
    }

    public void AddFeedback(string i_Feedback)
    {
        Feedback.Add(i_Feedback);
    }
}