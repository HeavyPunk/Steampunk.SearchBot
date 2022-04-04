using AngleSharp;
using AngleSharp.Dom;
using SearchBot.Worker.Jobs;
using Node = SearchBot.Lib.Config.Node;

namespace SearchBot.Lib.Scanners.SiteScanner;

public class SiteScanner : IScanner<Node, Dictionary<string, string>>
{
    public Dictionary<string, string> Scan(Node source, ArgsContainer<Node> toFind)
    {
        var titles = GetTitles(source);
        //var result = BuildFaqCollection(titles, null);
        return new Dictionary<string, string>();
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

    private IEnumerable<string> GetTitles(Node node)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var address = node.Href;
        var context = BrowsingContext.New(config);
        var document = context.OpenAsync(address).GetAwaiter().GetResult();
        var mainCell = document.QuerySelector(node.MainBlockSelector); //TODO: null check
        var titles = new List<string>();
        GetElementText(mainCell, titles);
        return titles;
    }

    private void GetElementText(IElement element, ICollection<string> contents)
    {
        if (element.Children is { Length: > 0 })
        {
            foreach (var child in element.Children)
                GetElementText(child, contents);
        }
        
        contents.Add(element.TextContent);
    }
}