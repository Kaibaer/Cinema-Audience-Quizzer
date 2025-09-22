public class Guess
{
    public string SeatNumber { get; set; }
    public int RuntimeGuess { get; set; }
    public int? Difference { get; set; } // Calculated after reveal
    public Guid GameSessionId { get; set; }
}
