using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

public class MobileDataUsageReminderDispatcherFunction
{
    private readonly ICommandProcessorBinder _commandProcessorBinder;

    public MobileDataUsageReminderDispatcherFunction(ICommandProcessorBinder commandProcessorBinder)
    {
        _commandProcessorBinder = commandProcessorBinder;
    }

    [FunctionName(nameof(MobileDataUsageReminderDispatcherFunction))]
    public async Task Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest request, ILogger logger)
    {
        var eventJson = await request.ReadAsStringAsync();
        var eventPayload = JsonSerializer.Deserialize(eventJson, EventPayloadContext.Default.EventPayload);

        ValidateEventPayload(eventPayload);

        logger.LogInformation("Bot trigger with commandType: {commandType}", eventPayload.Message.CommandType);

        var processor = _commandProcessorBinder.GetCommandProcessor(eventPayload.Message.CommandType);

        await processor.ProcessCommand(eventPayload);
    }

    private static void ValidateEventPayload(EventPayload eventData)
    {
        ArgumentNullException.ThrowIfNull(eventData?.Message?.CommandType, nameof(eventData.Message.CommandType));
        ArgumentNullException.ThrowIfNull(eventData?.Message?.From?.Id, "From.Id");
    }
}

