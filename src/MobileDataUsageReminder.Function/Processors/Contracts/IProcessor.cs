using System.Threading.Tasks;

public interface IProcessor
{
    Task ProcessCommand(EventPayload eventPayload);
}
