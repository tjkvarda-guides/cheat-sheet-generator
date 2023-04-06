internal class Body : ISaveable
{
    private LinkedList<ISaveable> contents;

    private Body(LinkedList<ISaveable> contents)
    {
        this.contents = contents;
    }

    public async Task Save(StreamWriter writer)
    {
        await writer.WriteLineAsync(@"<body>");

        foreach (var saveable in this.contents)
        {
            await saveable.Save(writer);
        }

        await writer.WriteLineAsync(@"<script src=""https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js""></script>");
        await writer.WriteLineAsync(@"<script src=""https://cdn.rawgit.com/andris9/jStorage/v0.4.12/jstorage.min.js""></script>");
        await writer.WriteLineAsync(@"<script src=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"" integrity=""sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS"" crossorigin=""anonymous""></script>");
        await writer.WriteLineAsync(@"<script src=""https://cdnjs.cloudflare.com/ajax/libs/jets/0.8.0/jets.min.js""></script>");
        await writer.WriteLineAsync(@"<script src=""js/jquery.highlight.js""></script>");
        await writer.WriteLineAsync(@"<script src=""js/main.js""></script>");

        await writer.WriteLineAsync(@"</body>");
    }

    public static async Task<Body> Create(StreamReader reader)
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
                case "nav":
                    contents.AddLast(await Nav.Create(reader));
                    break;
                case "content":
                    contents.AddLast(await Content.Create(reader));
                    break;
                default:
                    break;
            }
        }

        return new Body(contents);
    }
}