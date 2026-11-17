using IzpitSimulator.Models;
using Microsoft.EntityFrameworkCore;

namespace IzpitSimulator.Data;

public interface IQuestionRepository
{
    IReadOnlyList<Question> GetAll();
    Question? GetById(int id);
}

/// <summary>
/// Чете въпросите от SQL базата. Зареждат се веднъж (при първо ползване) и се държат в паметта,
/// така че генерирането на листовка не чака базата. След промяна на въпроси в SQL – рестартирайте приложението.
/// </summary>
public class QuestionRepository : IQuestionRepository
{
    private readonly Lazy<(IReadOnlyList<Question> All, Dictionary<int, Question> ById)> _cache;

    public QuestionRepository(IServiceScopeFactory scopeFactory)
    {
        _cache = new Lazy<(IReadOnlyList<Question>, Dictionary<int, Question>)>(() =>
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var list = db.Questions.AsNoTracking().Include(q => q.Options).OrderBy(q => q.Id)
                .ToList().Select(Map).ToList();
            return (list, list.ToDictionary(q => q.Id));
        });
    }

    public IReadOnlyList<Question> GetAll() => _cache.Value.All;

    public Question? GetById(int id) => _cache.Value.ById.GetValueOrDefault(id);

    private static Question Map(QuestionEntity e)
    {
        var opts = e.Options.OrderBy(o => o.SortOrder).ToList();
        return new Question(e.Id, e.Category, e.Text, opts.Select(o => o.Text).ToArray(),
            opts.FindIndex(o => o.IsCorrect), e.Explanation, e.Sign);
    }
}
