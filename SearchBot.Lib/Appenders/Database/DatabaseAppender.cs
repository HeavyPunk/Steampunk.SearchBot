using SearchBot.Lib.Appenders.Database.Context;
using SearchBot.Lib.Appenders.Database.Models;

namespace SearchBot.Lib.Appenders.Database;

public class DatabaseAppender : IAppender
{
    public async Task Add(params string[] fields)
    {
        await using var db = new FAQContext();
        var faq = new FAQPair
        {
            Question = fields[0],
            Answer = fields[1],
        };

        var existing = db.FaqPairs.FirstOrDefault(f => f.Question.Equals(fields[0], StringComparison.InvariantCulture));
        if (existing != null)
        {
            db.FaqPairs.Update(new FAQPair
            {
                Id = existing.Id,
                Answer = faq.Answer,
                Question = faq.Question,
            });
        }
        else
            await db.FaqPairs.AddAsync(faq);
        await db.SaveChangesAsync();
    }
}