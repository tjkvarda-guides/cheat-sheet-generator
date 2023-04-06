using System.Collections.Specialized;

internal class GuideTab : ISaveable
{
    private string title;

    private OrderedDictionary contents;

    private GuideTab(string title, OrderedDictionary contents)
    {
        this.title = title;
        this.contents = contents;
    }

    public async Task Save(StreamWriter writer)
    {
        await writer.WriteLineAsync($"<div class=\"tab-pane active\" id=\"tab{this.title}\">");

        await writer.WriteLineAsync($"  <h2>{this.title} Checklist <span id=\"{this.title.ToLower()}_overall_total\"></span></h2>");
        await writer.WriteLineAsync(@"  <ul class=""table_of_contents"">");
        
        int i = 1;
        foreach (string key in contents.Keys)
        {
            await writer.WriteLineAsync($"    <li><a href=\"#{key.Replace(" ", "_")}\">{key}</a> <span id=\"{this.title.ToLower()}_nav_totals_{i}\"></span></li>");
            i++;
        }

        await writer.WriteLineAsync(@"  </ul>");

        await writer.WriteLineAsync(@"  <div class=""form-group"">");
        await writer.WriteLineAsync($"    <input type=\"search\" id=\"{this.title.ToLower()}_search\" class=\"form-control\" placeholder=\"Start typing to filter results...\" />");
        await writer.WriteLineAsync(@"  </div>");

        await writer.WriteLineAsync($"  <div id=\"{this.title.ToLower()}_list\">");

        foreach (ISaveable saveable in this.contents.Values)
        {
            await saveable.Save(writer);
        }

        await writer.WriteLineAsync(@"  </div>");

        await writer.WriteLineAsync(@"</div>");
    }

    public static async Task<GuideTab> Create(string title, StreamReader reader)
    {
        bool end = false;
        OrderedDictionary contents = new OrderedDictionary();

        while (!reader.EndOfStream && !end)
        {
            var line = await reader.ReadLineAsync();
            switch (line.Split(' ')[0])
            {
                case "end":
                    end = true;
                    break;
                case "section":
                    string sectionTitle = line.Substring("section".Length + 1);
                    contents.Add(sectionTitle, await Section.Create(title, sectionTitle, reader));
                    break;
                default:
                    break;
            }
        }

        return new GuideTab(title, contents);
    }
}