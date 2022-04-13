using System.Threading.Tasks;

public interface IProcessor
{
    Task ProcessCommand(CommandEventPayload eventPayload);
}
