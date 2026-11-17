namespace IzpitSimulator.Models;

/// <summary>Настройки на изпита (секция "Exam" в appsettings.json).</summary>
public class ExamSettings
{
    public int QuestionsPerSheet { get; set; } = 20;
    public int PassScore { get; set; } = 18;
}
