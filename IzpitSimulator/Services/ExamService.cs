using System.Diagnostics;
using IzpitSimulator.Data;
using IzpitSimulator.Models;
using Microsoft.Extensions.Options;

namespace IzpitSimulator.Services;

public interface IExamService
{
    IndexViewModel GetInfo();
    SheetViewModel GenerateSheet();
    ResultViewModel Evaluate(IEnumerable<int> questionIds, IDictionary<int, int> answers);
}

public class ExamService : IExamService
{
    private readonly ExamSettings _settings;
    private readonly IQuestionRepository _questions;

    public ExamService(IOptions<ExamSettings> options, IQuestionRepository questions)
    {
        _settings = options.Value;
        _questions = questions;
    }

    public IndexViewModel GetInfo() => new(
        _questions.GetAll().Count,
        _questions.GetAll().Select(q => q.Category).Distinct().ToList(),
        _settings.QuestionsPerSheet,
        _settings.PassScore);

    /// <summary>
    /// Генерира листовка: въпросите се теглят „на кръгове“ от разбъркани категории,
    /// така че да са равномерно разпределени, после редът им и редът на отговорите се разбъркват.
    /// </summary>
    public SheetViewModel GenerateSheet()
    {
        var sw = Stopwatch.StartNew();

        var pools = _questions.GetAll()
            .GroupBy(q => q.Category)
            .ToDictionary(g => g.Key, g => new Queue<Question>(Shuffle(g)));
        var categoryOrder = Shuffle(pools.Keys);

        int size = Math.Min(_settings.QuestionsPerSheet, _questions.GetAll().Count);
        var picked = new List<Question>();
        while (picked.Count < size)
        {
            bool tookAny = false;
            foreach (var category in categoryOrder)
            {
                if (picked.Count >= size) break;
                if (pools[category].Count > 0)
                {
                    picked.Add(pools[category].Dequeue());
                    tookAny = true;
                }
            }
            if (!tookAny) break;
        }

        var items = Shuffle(picked)
            .Select((q, i) => new ExamItem(i + 1, q, Shuffle(Enumerable.Range(0, q.Options.Length)).ToArray()))
            .ToList();

        sw.Stop();
        return new SheetViewModel { Items = items, GenerationMs = sw.Elapsed.TotalMilliseconds, PassScore = _settings.PassScore };
    }

    /// <summary>Детерминистично оценяване: 1 точка за верен отговор, 0 за грешен или липсващ.</summary>
    public ResultViewModel Evaluate(IEnumerable<int> questionIds, IDictionary<int, int> answers)
    {
        var items = new List<ReviewItem>();
        int number = 1;
        foreach (var id in questionIds.Distinct())
        {
            var q = _questions.GetById(id);
            if (q is null) continue;

            int? selected = answers.TryGetValue(id, out var a) && a >= 0 && a < q.Options.Length ? a : null;
            items.Add(new ReviewItem(number++, q, selected));
        }

        return new ResultViewModel
        {
            Items = items,
            Total = items.Count,
            Score = items.Count(i => i.IsCorrect),
            PassScore = _settings.PassScore
        };
    }

    private static List<T> Shuffle<T>(IEnumerable<T> source)
    {
        var list = source.ToList();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Shared.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }
}
