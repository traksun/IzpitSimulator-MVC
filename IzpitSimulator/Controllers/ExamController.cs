using IzpitSimulator.Data;
using IzpitSimulator.Services;
using Microsoft.AspNetCore.Mvc;

namespace IzpitSimulator.Controllers;

public class ExamController : Controller
{
    private readonly IExamService _exam;
    private readonly AppDbContext _db;

    public ExamController(IExamService exam, AppDbContext db)
    {
        _exam = exam;
        _db = db;
    }

    // GET /  – начален екран с правила
    public IActionResult Index() => View(_exam.GetInfo());

    // GET /Exam/Sheet – генерира нова листовка при всяко отваряне
    public IActionResult Sheet() => View(_exam.GenerateSheet());

    // POST /Exam/Result – оценява отговорите и записва опита в SQL
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Result(int[] ids, Dictionary<int, int> answers)
    {
        if (ids is null || ids.Length == 0)
            return RedirectToAction(nameof(Index));

        var result = _exam.Evaluate(ids, answers ?? new Dictionary<int, int>());

        _db.ExamAttempts.Add(new ExamAttempt
        {
            TakenAt = DateTime.Now,
            Score = result.Score,
            Total = result.Total,
            Passed = result.Passed
        });
        _db.SaveChanges();

        return View(result);
    }

    // GET /Exam/History – последните 20 опита от базата
    public IActionResult History() =>
        View(_db.ExamAttempts.OrderByDescending(a => a.TakenAt).Take(20).ToList());
}
