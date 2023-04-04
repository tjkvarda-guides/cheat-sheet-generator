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
                default:
                    break;
            }
        }

        return new Body(contents);
    }
}