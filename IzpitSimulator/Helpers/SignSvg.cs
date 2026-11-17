namespace IzpitSimulator.Helpers;

/// <summary>SVG изображения на пътните знаци (ключът се задава в Question.Sign).</summary>
public static class SignSvg
{
    private static readonly Dictionary<string, string> Svgs = new()
    {
        ["stop"] = "<svg viewBox='0 0 100 100' role='img' aria-label='Знак STOP'><polygon points='30,3 70,3 97,30 97,70 70,97 30,97 3,70 3,30' fill='#d11' stroke='#fff' stroke-width='4'/><polygon points='30,3 70,3 97,30 97,70 70,97 30,97 3,70 3,30' fill='none' stroke='#d11' stroke-width='1'/><text x='50' y='60' font-size='24' font-weight='800' text-anchor='middle' fill='#fff' font-family='Arial,sans-serif'>STOP</text></svg>",
        ["yield"] = "<svg viewBox='0 0 100 100' role='img' aria-label='Знак Пропусни'><polygon points='50,88 10,16 90,16' fill='#fff' stroke='#d11' stroke-width='12' stroke-linejoin='round'/></svg>",
        ["noentry"] = "<svg viewBox='0 0 100 100' role='img' aria-label='Забранено влизане'><circle cx='50' cy='50' r='47' fill='#d11'/><rect x='20' y='42' width='60' height='16' fill='#fff'/></svg>",
        ["blue"] = "<svg viewBox='0 0 100 100' role='img' aria-label='Син кръгъл знак'><circle cx='50' cy='50' r='47' fill='#1d5fd6'/><path d='M50 18 L74 46 H59 V80 H41 V46 H26 Z' fill='#fff'/></svg>",
        ["speed"] = "<svg viewBox='0 0 100 100' role='img' aria-label='Ограничение на скоростта 50'><circle cx='50' cy='50' r='42' fill='#fff' stroke='#d11' stroke-width='10'/><text x='50' y='63' font-size='38' font-weight='800' text-anchor='middle' fill='#111' font-family='Arial,sans-serif'>50</text></svg>",
    };

    public static string Get(string key) => Svgs.TryGetValue(key, out var svg) ? svg : string.Empty;
}
