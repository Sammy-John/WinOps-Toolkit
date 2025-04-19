<#
.SYNOPSIS
Switches power profile based on time of day.
Day: 8 AM – 10 PM → DayProfile
Night: 10 PM – 8 AM → NightProfile
#>

function Get-PowerPlanGuidByName($name) {
    $plans = powercfg /list
    foreach ($line in $plans) {
        if ($line -match "GUID:\s+([a-fA-F0-9\-]+).+\($name\)") {
            return $matches[1]
        }
    }
    return $null
}

# Define time window for Day Profile
$hour = (Get-Date).Hour
$profileName = if ($hour -ge 8 -and $hour -lt 22) { "DayProfile" } else { "NightProfile" }

$guid = Get-PowerPlanGuidByName $profileName

if (-not $guid) {
    Write-Warning "⚠ Could not find profile: $profileName"
    exit 1
}

powercfg /s $guid
Write-Host "✅ AutoSwitched to $profileName (GUID: $guid)" -ForegroundColor Green
