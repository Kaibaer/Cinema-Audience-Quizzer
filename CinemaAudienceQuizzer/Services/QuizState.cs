public class QuizState
{
    public string PosterUrl { get; set; } = "/images/default-poster.jpg";
    public int? ActualRuntime { get; set; }
    public bool RuntimeRevealed { get; set; } = false;
    public List<Guess> Guesses { get; set; } = new();
}