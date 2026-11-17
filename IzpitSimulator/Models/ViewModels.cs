namespace IzpitSimulator.Models;

public record IndexViewModel(int BankSize, IReadOnlyList<string> Categories, int QuestionsPerSheet, int PassScore);

/// <summary>Един въпрос в листовката + подредба на отговорите (индекси към Question.Options).</summary>
public record ExamItem(int Number, Question Question, int[] OptionOrder);

public class SheetViewModel
{
    public List<ExamItem> Items { get; init; } = new();
    public double GenerationMs { get; init; }
    public int PassScore { get; init; }
}

public record ReviewItem(int Number, Question Question, int? Selected)
{
    public bool IsCorrect => Selected == Question.CorrectIndex;
}

public class ResultViewModel
{
    public int Score { get; init; }
    public int Total { get; init; }
    public int PassScore { get; init; }
    public bool Passed => Score >= PassScore;
    public List<ReviewItem> Items { get; init; } = new();
}
