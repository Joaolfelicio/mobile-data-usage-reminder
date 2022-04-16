param(
    [string]$TelegramEndpoint,
    [string]$TelegramToken,
    [string]$CommandsFile
)

$setCommandsUrl = $TelegramEndpoint + $TelegramToken + "/setMyCommands"

$commands = Get-Content $CommandsFile -Raw
Write-Output "Commands in file: $($commands)"

$result = Invoke-WebRequest -Uri $setCommandsUrl -Method POST -ContentType "application/json" -Body $commands
Write-Output "POST Result: $($result.Content)"
