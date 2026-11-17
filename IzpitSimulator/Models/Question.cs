namespace IzpitSimulator.Models;

/// <summary>Въпрос от базата данни. Options са отговорите, CorrectIndex е индексът на верния (от 0).</summary>
public record Question(
    int Id,
    string Category,
    string Text,
    string[] Options,
    int CorrectIndex,
    string Explanation,
    string? Sign = null);
