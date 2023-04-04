var guide = new Guide(args[0]);
await guide.Generate();
await guide.Save(args[1]);