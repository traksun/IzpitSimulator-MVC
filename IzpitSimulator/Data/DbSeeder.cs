namespace IzpitSimulator.Data;

public static class DbSeeder
{
    /// <summary>Създава базата и таблиците (ако ги няма) и ги пълни с начални въпроси от QuestionBank.</summary>
    public static void Initialize(AppDbContext db)
    {
        db.Database.EnsureCreated();
        if (db.Questions.Any()) return;

        foreach (var q in QuestionBank.All)
        {
            db.Questions.Add(new QuestionEntity
            {
                Category = q.Category,
                Text = q.Text,
                Explanation = q.Explanation,
                Sign = q.Sign,
                Options = q.Options.Select((t, i) => new AnswerOptionEntity
                {
                    Text = t,
                    IsCorrect = i == q.CorrectIndex,
                    SortOrder = i
                }).ToList()
            });
        }
        db.SaveChanges();
    }
}
