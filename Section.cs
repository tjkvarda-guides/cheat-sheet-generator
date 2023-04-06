internal class Section : ISaveable
{
    static int sectionCount = 0;

    private int num;

    private string parentTitle;

    private string title;

    private LinkedList<(string, string)> contents;

    private Section(string parentTitle, string title, LinkedList<(string, string)> contents)
    {
        sectionCount++;
        this.num = sectionCount;
        this.parentTitle = parentTitle;
        this.title = title;
        this.contents = contents;
    }

    public async Task Save(StreamWriter writer)
    {
        string underscoredTitle = this.title.Replace(" ", "_");
        await writer.WriteLineAsync($"<h3 id=\"{underscoredTitle}\"><a href=\"#{underscoredTitle}_col\" data-toggle=\"collapse\" class=\"btn btn-primary btn-collapse btn-sm\"></a><a href=\"\">{this.title}</a> <span id=\"{this.parentTitle.ToLower()}_totals_{this.num}\"></span></h3>");
        await writer.WriteLineAsync($"<ul id=\"{underscoredTitle}_col\" class=\"collapse in\">");
        
        int i = 1;
        foreach (var item in this.contents)
        {
            await writer.WriteLineAsync($"<li data-id=\"{this.parentTitle.ToLower()}_{this.num}_{i}\" class=\"f_misc\">{item.Item1} <a href=\"{item.Item2}\">Map</a></li>");
            i++;
        }

        await writer.WriteLineAsync(@"</ul>");
    }

    public static async Task<Section> Create(string parentTitle, string title, StreamReader reader)
    {
        bool end = false;
        LinkedList<(string, string)> contents = new LinkedList<(string, string)>();

        while (!reader.EndOfStream && !end)
        {
            var line = await reader.ReadLineAsync();
            switch (line.Split(' ')[0])
            {
                case "end":
                    end = true;
                    break;
                default:
                    int braceStart = line.IndexOf("{") + 1;
                    (string, string) item = (line.Substring(0, braceStart > 0 ? (braceStart - 2) : line.Length), line.Contains("{") ? line.Substring(braceStart, line.IndexOf("}") - braceStart) : string.Empty);
                    contents.AddLast(item);
                    break;
            }
        }

        return new Section(parentTitle, title, contents);
    }
}