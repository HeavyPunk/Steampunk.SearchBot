using AngleSharp;
using SearchBot.Worker.Jobs;

namespace SearchBot.Lib.Scanners.SiteScanner;

public class SiteScanner : IScanner<string, Dictionary<string, string>>
{
    public Dictionary<string, string> Scan(string url, ArgsContainer<string> toFind)
    {
        var titles = GetTitles(url);
        var result = BuildFaqCollection(titles, toFind.Arguments);
        return result;
    }

    //todo: Переписать сопоставление вопроса к ответу
    private Dictionary<string, string> BuildFaqCollection(IEnumerable<string> siteContent, IList<Argument<string>> oldQuestions)
    {
        var res = new Dictionary<string, string>();
        var i = 0;
        foreach (var content in siteContent)
        {
            res.Add(content, oldQuestions[i].Value);
            i++;
            if (i >= oldQuestions.Count)
                break;
        }

        return res;
    }

    private IEnumerable<string> GetTitles(string url)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var address = url;
        var context = BrowsingContext.New(config);
        var document = context.OpenAsync(address).GetAwaiter().GetResult();
        var cellSelector = "tr.vevent td:nth-child(3)";
        var cells = document.QuerySelectorAll(cellSelector);
        var titles = cells.Select(m => m.TextContent);
        return titles;
    }
}