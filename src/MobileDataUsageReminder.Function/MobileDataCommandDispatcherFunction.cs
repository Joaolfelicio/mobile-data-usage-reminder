using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

public class MobileDataCommandDispatcherFunction
{
    private readonly ICommandProcessorBinder _commandProcessorBinder;

    public MobileDataCommandDispatcherFunction(ICommandProcessorBinder commandProcessorBinder)
    {
        _commandProcessorBinder = commandProcessorBinder;
    }

    [FunctionName(nameof(MobileDataCommandDispatcherFunction))]
    public async Task Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest request, ILogger logger)
    {
        var eventPayload = JsonSerializer.Deserialize(await request.ReadAsStringAsync(), CommandEventPayloadContext.Default.CommandEventPayload);

        ValidateEventPayload(eventPayload);

        logger.LogInformation("Bot trigger with commandType: {commandType}", eventPayload.Message.CommandType);

        var processor = _commandProcessorBinder.GetCommandProcessor(eventPayload.Message.CommandType);

        await processor.ProcessCommand(eventPayload);
    }

    private static void ValidateEventPayload(CommandEventPayload eventPayload)
    {
        ArgumentNullException.ThrowIfNull(eventPayload?.Message?.CommandType, nameof(eventPayload.Message.CommandType));
        ArgumentNullException.ThrowIfNull(eventPayload?.Message?.From?.Id, "From.Id");
    }
}

