internal class Tab : ISaveable
{
    private string name;

    private Tab(string name)
    {
        this.name = name;
    }

    public async Task Save(StreamWriter writer)
    {
        string target = this.name.Replace(" ", string.Empty);
        await writer.WriteLineAsync($"          <li><a href=\"#tab{target}\" data-toggle=\"tab\" data-target=\"#tab{target},#btnHideCompleted\">{this.name}</a></li>");
    }

    public static async Task<Tab> Create(string line)
    {
        bool end = false;
        string name = line.Substring("tab ".Length);
        return new Tab(name);
    }
}