<#
.SYNOPSIS
Sets screen and sleep timeouts for DayProfile and NightProfile.
Day = 120 minutes, Night = 2 minutes (AC power).
#>

function Set-PowerTimeouts {
    param (
        [string]$ProfileName,
        [int]$TimeoutMinutes
    )

    Write-Host "⚙ Setting timeouts for $ProfileName..." -ForegroundColor Cyan

    $plans = powercfg /list
    $guid = $null

    foreach ($line in $plans) {
        if ($line -match "GUID:\s+([a-fA-F0-9\-]+).+\($ProfileName\)") {
            $guid = $matches[1]
            break
        }
    }

    if (-not $guid) {
        Write-Warning "⚠ $ProfileName not found. Skipping..."
        return
    }

    powercfg /setacvalueindex $guid SUB_SLEEP STANDBYIDLE $TimeoutMinutes
    powercfg /setacvalueindex $guid SUB_VIDEO VIDEOIDLE $TimeoutMinutes

    Write-Host "✅ Set sleep + screen timeout to $TimeoutMinutes min for $ProfileName (GUID: $guid)"
}

# Apply to both profiles
Set-PowerTimeouts -ProfileName "DayProfile" -TimeoutMinutes 120
Set-PowerTimeouts -ProfileName "NightProfile" -TimeoutMinutes 2

Write-Host "`n🎯 Power plan timeouts updated."
