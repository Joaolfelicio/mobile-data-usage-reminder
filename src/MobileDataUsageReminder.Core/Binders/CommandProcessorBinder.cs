using System;

public class CommandProcessorBinder : ICommandProcessorBinder
{
    private readonly ICheckUsageProcessor _checkUsageProcessor;

    public CommandProcessorBinder(ICheckUsageProcessor checkUsageProcessor)
    {
        _checkUsageProcessor = checkUsageProcessor;
    }

    public IProcessor GetCommandProcessor(string commandType)
    {
        var commandTypeNormalized = commandType.ToLowerInvariant().Trim();

        return commandTypeNormalized switch
        {
            EventConstants.CheckUsage => _checkUsageProcessor,
            _ => throw new InvalidOperationException($"Unknown command type: {commandTypeNormalized}")
        };
    }
}
