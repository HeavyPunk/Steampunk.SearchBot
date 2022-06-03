using SearchBot.Lib.Appenders.Database.Context;
using SearchBot.Lib.Appenders.Database.Models;
using SearchBot.Lib.DataComparators;

namespace SearchBot.Lib.Appenders.Database;

public class DatabaseAppender : IAppender
{
    private readonly IComparer<string> stringComparer = new SimpleStringComparator();
    
    public async Task Add(params string[] fields)
    {
        await using var db = new FAQContext();
        var faq = new FAQPair
        {
            Question = fields[0],
            Answer = fields[1],
        };

        var oldPair = db.FaqPairs.FirstOrDefault(f => stringComparer.Compare(f.Question, fields[0]) == 0);
        if (oldPair != null)
        {
            oldPair.Question = fields[0];
            oldPair.Answer = fields[1];
            db.FaqPairs.Update(oldPair);
        }
        else
            await db.AddAsync(faq);
        
        await db.SaveChangesAsync();
    }
}