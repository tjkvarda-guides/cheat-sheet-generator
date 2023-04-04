
internal class Guide
{
    private string input;

    private LinkedList<ISaveable> contents;

    public Guide(string input)
    {
        this.input = input;
        this.contents = new LinkedList<ISaveable>();
    }

    public async Task Generate()
    {
        using (var file = File.OpenRead(this.input))
        using (var reader = new StreamReader(file))
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                switch (line)
                {
                    case "head":
                        this.contents.AddLast(await Head.Create(reader));
                        break;
                    case "body":
                        this.contents.AddLast(await Body.Create(reader));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public async Task Save(string directory)
    {
        using (var file = File.Create(directory))
        using (var writer = new StreamWriter(file))
        {
            await writer.WriteLineAsync(@"<!DOCTYPE html>");
            await writer.WriteLineAsync(@"<html lang=""en-US"">");

            foreach (var saveable in this.contents)
            {
                await saveable.Save(writer);
            }

            await writer.WriteLineAsync(@"</html>");
        }
    }
}