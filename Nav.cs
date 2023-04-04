internal class Nav : ISaveable
{
    private LinkedList<ISaveable> contents;

    private Nav(LinkedList<ISaveable> contents)
    {
        this.contents = contents;
    }

    public async Task Save(StreamWriter writer)
    {
        await writer.WriteLineAsync(@"<nav class=""navbar navbar-default"">");
        await writer.WriteLineAsync(@"  <div class=""container-fluid"">");
        await writer.WriteLineAsync(@"    <div class=""navbar-header"">");
        await writer.WriteLineAsync(@"      <button type=""button"" class=""navbar-toggle collapsed"" data-toggle=""collapse"" data-target=""#nav-collapse"" aria-expanded=""false"">");
        await writer.WriteLineAsync(@"          <span class=""sr-only"">Toggle navigation</span>");
        await writer.WriteLineAsync(@"          <span class=""icon-bar""></span>");
        await writer.WriteLineAsync(@"          <span class=""icon-bar""></span>");
        await writer.WriteLineAsync(@"          <span class=""icon-bar""></span>");
        await writer.WriteLineAsync(@"      </button>");
        await writer.WriteLineAsync(@"    </div>");
        
        await writer.WriteLineAsync(@"      <div class=""collapse navbar-collapse"" id=""nav-collapse"">");
        await writer.WriteLineAsync(@"        <ul class=""nav navbar-nav"">");

        foreach (var saveable in this.contents)
        {
            await saveable.Save(writer);
        }

        await writer.WriteLineAsync(@"        </ul>");
        await writer.WriteLineAsync(@"      </div>");

        await writer.WriteLineAsync(@"  </div>");
        await writer.WriteLineAsync(@"</nav>");
    }

    public static async Task<Nav> Create(StreamReader reader)
    {
        bool end = false;
        LinkedList<ISaveable> contents = new LinkedList<ISaveable>();

        while (!reader.EndOfStream && !end)
        {
            var line = await reader.ReadLineAsync();
            switch (line.Split(' ')[0])
            {
                case "end":
                    end = true;
                    break;
                case "tab":
                    contents.AddLast(await Tab.Create(line));
                    break;
                default:
                    break;
            }
        }

        return new Nav(contents);
    }
}