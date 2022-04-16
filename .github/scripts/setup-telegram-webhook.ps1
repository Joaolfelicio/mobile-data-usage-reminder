param(
    [string]$TelegramEndpoint,
    [string]$TelegramToken,
    [string]$BaseWebhookUrl,
    [string]$WebhookKey
)

$setWebhookUrl = $TelegramEndpoint + $TelegramToken + "/setWebhook"
$functionWebhookUrl = "$($BaseWebhookUrl)?code=$($WebhookKey)"

Write-Output "$setWebhookUrl"

$webhookPayload = @"
{
    "url": "$functionWebhookUrl"
}
"@

Write-Output $webhookPayload

$result = Invoke-WebRequest -Uri $setWebhookUrl -Method POST -ContentType "application/json" -Body $webhookPayload
Write-Output "POST Result: $($result.Content)"
