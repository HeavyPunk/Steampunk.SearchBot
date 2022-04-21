using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using SearchBot.Worker.Jobs;
using Node = SearchBot.Lib.Config.Node;

namespace SearchBot.Lib.Scanners.SiteScanner;

public class SiteScanner : IScanner<Node, Dictionary<string, string>>
{
    private Regex sentenceRegex = new("[A-Za-zА-ЯА-яёЁ0-9 .,/\\*@?><&%$#! }{'\"-_—();:`~№^]+", RegexOptions.Compiled);
    public Dictionary<string, string> Scan(Node source, ArgsContainer<Node> toFind)
    {
        var titles = GetTitles(source);
        var result = BuildFaqCollection(titles);
        return result;
    }

    //todo: Переписать сопоставление вопроса к ответу
    private Dictionary<string, string> BuildFaqCollection(IEnumerable<string>? siteContent)
    {
        if (siteContent is null)
            return new Dictionary<string, string>();
        var questionRegex = new Regex("[A-Za-zА-ЯА-яёЁ0-9 .,/\\*@?><&%$#! }{'\"-_—();:`~№^]+[?]{1}", RegexOptions.Compiled);
        var res = new Dictionary<string, string>();
        string curQuestion = null;
        var answContainer = new List<string>();
        foreach (var str in siteContent)
        {
            if (questionRegex.IsMatch(str))
            {
                if (curQuestion is not null && answContainer.Count > 0)
                {
                    res.TryAdd(curQuestion, string.Join(' ', answContainer));
                    answContainer = new List<string>();
                }

                curQuestion = str;
                continue;
            }
            if (curQuestion is null)
                continue;
            answContainer.Add(str);
        }

        return res;
    }

    private IEnumerable<string>? GetTitles(Node node)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var address = node.Href;
        var context = BrowsingContext.New(config);
        var document = context.OpenAsync(address).GetAwaiter().GetResult();
        var mainCell = document.QuerySelector(node.MainBlockSelector); //TODO: null check
        var titles = new List<string>();
        if (mainCell == null)
            return null;
        GetElementText(mainCell, titles, new Dictionary<string, string>());
        return titles;
    }

    private void GetElementText(IElement element, IList<string> contents, IDictionary<string, string> hrefs) //TODO: переписать на pipe-структуру для модульности и дополняемости
    {
        if (element.Children is { Length: > 0 })
        {
            foreach (var child in element.Children)
                GetElementText(child, contents, hrefs);
        }

        var content = element.TextContent;
        if (!string.IsNullOrEmpty(content) && element is IHtmlAnchorElement link)
        {
            hrefs[content] = link.Href;
            return;
        }

        if (string.IsNullOrEmpty(content))
            return;
        var res = sentenceRegex.Matches(content);
        foreach (Match match in res)
        {
            var toAdd = match.Value;
            if (contents.Count > 0 && contents[^1].Equals(toAdd))
                continue;
            foreach (var (hrefText, href) in hrefs)
            {
                if (!toAdd.Contains(hrefText))
                    continue;
                toAdd = toAdd.Replace(hrefText, href);
                hrefs.Remove(hrefText);
                break;
            }
            contents.Add(toAdd);
        }
    }
}