<#
.SYNOPSIS
Activates the NightProfile power plan.
#>

$target = "NightProfile"
$plans = powercfg /list
$guid = $null

foreach ($line in $plans) {
    if ($line -match "GUID:\s+([a-fA-F0-9\-]+).+\($target\)") {
        $guid = $matches[1]
        break
    }
}

if (-not $guid) {
    Write-Warning "⚠ Could not find a power plan named '$target'"
    exit 1
}

powercfg /s $guid
Write-Host "✅ Switched to $target (GUID: $guid)" -ForegroundColor Green
