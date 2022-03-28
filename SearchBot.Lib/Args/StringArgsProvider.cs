namespace SearchBot.Worker.Jobs;

public class StringArgsProvider
{
    public ArgsContainer<string> GetArgs()
    {
        return new ArgsContainer<string>
        {
            Arguments = new List<Argument<string>>
            {
                new() { Value = "https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes" },
                new() { Value = "https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes" }
            }
        };
    }
}