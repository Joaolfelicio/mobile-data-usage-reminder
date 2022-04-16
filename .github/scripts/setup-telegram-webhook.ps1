param(
    [string]$TelegramEndpoint,
    [string]$TelegramToken,
    [string]$FunctionUrl,
    [string]$FunctionAppName
)

$functionKey = (Get-AzResource -Name $FunctionAppName | Invoke-AzResourceAction -Action host/default/listkeys -Force).functionKeys.default

$setWebhookUrl = $TelegramEndpoint + $TelegramToken + "/setWebhook"
$functionWebhookUrl = "$($FunctionUrl)?code=$($functionKey)"

Write-Output "$setWebhookUrl"

$webhookPayload = @"
{
    "url": "$functionWebhookUrl"
}
"@

Write-Output $webhookPayload

$result = Invoke-WebRequest -Uri $setWebhookUrl -Method POST -ContentType "application/json" -Body $webhookPayload
Write-Output "POST Result: $($result.Content)"
