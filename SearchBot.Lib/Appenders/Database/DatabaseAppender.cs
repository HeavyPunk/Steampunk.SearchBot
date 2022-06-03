using SearchBot.Lib.Appenders.Database.Context;
using SearchBot.Lib.Appenders.Database.Models;
using SearchBot.Lib.DataComparators;

namespace SearchBot.Lib.Appenders.Database;

public class DatabaseAppender : IAppender
{
    private readonly IComparer<string> stringComparer = new SimpleStringComparator();
    private readonly FAQContext faqContext;
    
    public DatabaseAppender(FAQContext context)
    {
        this.faqContext = context;
    }
    
    public async Task Add(params string[] fields)
    {
        var faq = new FAQPair
        {
            Question = fields[0],
            Answer = fields[1],
        };

        var oldPair = faqContext.FaqPairs.FirstOrDefault(f => stringComparer.Compare(f.Question, fields[0]) == 0);
        if (oldPair != null)
        {
            oldPair.Question = fields[0];
            oldPair.Answer = fields[1];
            faqContext.FaqPairs.Update(oldPair);
        }
        else
            await faqContext.AddAsync(faq);
        
        await faqContext.SaveChangesAsync();
    }
}