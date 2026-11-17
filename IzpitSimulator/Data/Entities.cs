namespace IzpitSimulator.Data;

/// <summary>Таблица Questions – въпроси.</summary>
public class QuestionEntity
{
    public int Id { get; set; }
    public string Category { get; set; } = "";
    public string Text { get; set; } = "";
    public string Explanation { get; set; } = "";
    public string? Sign { get; set; }
    public List<AnswerOptionEntity> Options { get; set; } = new();
}

/// <summary>Таблица AnswerOptions – възможни отговори (точно един е верен за всеки въпрос).</summary>
public class AnswerOptionEntity
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public QuestionEntity? Question { get; set; }
    public string Text { get; set; } = "";
    public bool IsCorrect { get; set; }
    public int SortOrder { get; set; }
}

/// <summary>Таблица ExamAttempts – история на проведените тестове.</summary>
public class ExamAttempt
{
    public int Id { get; set; }
    public DateTime TakenAt { get; set; }
    public int Score { get; set; }
    public int Total { get; set; }
    public bool Passed { get; set; }
}
