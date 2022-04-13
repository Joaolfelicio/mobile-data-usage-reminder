public interface ICommandProcessorBinder
{
    public IProcessor GetCommandProcessor(string commandType);
}
