internal class Content : ISaveable
{
    private string title;

    private LinkedList<ISaveable> contents;

    private Content(string title, LinkedList<ISaveable> contents)
    {
        this.title = title;
        this.contents = contents;
    }

    public async Task Save(StreamWriter writer)
    {
        await writer.WriteLineAsync(@"<div class=""container"">");
        await writer.WriteLineAsync(@"  <div class=""row"">");
        await writer.WriteLineAsync(@"    <div class=""col-md-12 text-center"">");
        await writer.WriteLineAsync($"      <h1>{this.title}</h1>");
        await writer.WriteLineAsync(@"    </div>");
        await writer.WriteLineAsync(@"  </div>");

        await writer.WriteLineAsync(@"  <div class=""tab-content"">");

        foreach (var saveable in this.contents)
        {
            await saveable.Save(writer);
        }

        await writer.WriteLineAsync(@"  </div>");
        await writer.WriteLineAsync(@"</div>");
    }

    public static async Task<Content> Create(StreamReader reader)
    {
        bool end = false;
        string title = string.Empty;
        LinkedList<ISaveable> contents = new LinkedList<ISaveable>();

        while (!reader.EndOfStream && !end)
        {
            var line = await reader.ReadLineAsync();
            switch (line.Split(' ')[0])
            {
                case "end":
                    end = true;
                    break;
                case "title":
                    title = line.Substring("title".Length + 1);
                    break;
                case "guidetab":
                    contents.AddLast(await GuideTab.Create(line.Substring("guidetab".Length + 1), reader));
                    break;
                default:
                    break;
            }
        }

        return new Content(title, contents);
    }
}