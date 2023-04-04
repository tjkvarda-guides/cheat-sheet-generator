internal interface ISaveable
{
    Task Save(StreamWriter writer);
}