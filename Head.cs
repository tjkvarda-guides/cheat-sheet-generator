internal class Head : ISaveable
{
    private string title;

    private string description;

    private string author;

    private Head(string title, string description, string author)
    {
        this.title = title;
        this.description = description;
        this.author = author;
    }

    public async Task Save(StreamWriter writer)
    {
        await writer.WriteLineAsync(@"<head>");
        await writer.WriteLineAsync(@"<meta charset=""utf-8"">");
        await writer.WriteLineAsync($"<title>{this.title}</title>");
        await writer.WriteLineAsync(@"<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">");
        await writer.WriteLineAsync(@"<link rel=""shortcut icon"" type=""image/x-icon"" href=""img/favicon.ico?"">");
        await writer.WriteLineAsync(@"<link rel=""apple-touch-icon-precomposed"" href=""img/favicon-152.png"">");
        await writer.WriteLineAsync(@"<link rel=""mask-icon"" href=""img/pinned-tab-icon.svg"" color=""#000000"">");
        await writer.WriteLineAsync($"<meta name=\"description\" content=\"{this.description}\">");
        await writer.WriteLineAsync($"<meta name=\"author\" content=\"{this.author}\">");
        await writer.WriteLineAsync(@"<meta name=""mobile-web-app-capable"" content=""yes"">");
        await writer.WriteLineAsync(@"<link id=""bootstrap"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"" rel=""stylesheet"" crossorigin=""anonymous"">");
        await writer.WriteLineAsync(@"<link href=""css/main.css"" rel=""stylesheet"">");
        await writer.WriteLineAsync(@"</head>");
    }

    public static async Task<Head> Create(StreamReader reader)
    {
        bool end = false;
        string title = string.Empty;
        string description = string.Empty;
        string author = string.Empty;

        while (!reader.EndOfStream && !end)
        {
            var line = await reader.ReadLineAsync();
            switch (line.Split(' ')[0])
            {
                case "end":
                    end = true;
                    break;
                case "title":
                    title = line.Substring("title ".Length);
                    break;
                case "description":
                    description = line.Substring("description ".Length);
                    break;
                case "author":
                    author = line.Substring("author ".Length);
                    break;
                default:
                    break;
            }
        }

        return new Head(title, description, author);
    }
}