<#
.SYNOPSIS
Creates two custom power plans: DayProfile and NightProfile.
Each is based on the currently active plan and will be renamed if already exists.
#>

Write-Host "🔧 Creating DayProfile and NightProfile..." -ForegroundColor Cyan

# Get active power scheme
$activeOutput = powercfg /getactivescheme
if ($activeOutput -match "GUID:\s+([a-fA-F0-9\-]+)")
{
    $activeGuid = $matches[1]
    Write-Host "✔ Active Plan GUID: $activeGuid"
}
else
{
    Write-Error "❌ Could not retrieve active power scheme GUID."
    exit 1
}

# Function to check if a plan exists by name
function Get-PowerPlanGuidByName($name) {
    $plans = powercfg /list
    foreach ($line in $plans) {
        if ($line -match "GUID:\s+([a-fA-F0-9\-]+).+\($name\)") {
            return $matches[1]
        }
    }
    return $null
}

# Function to create + rename plan if not already existing
function Create-PlanIfMissing($planName) {
    $existing = Get-PowerPlanGuidByName $planName
    if ($existing) {
        Write-Host "⏩ '$planName' already exists (GUID: $existing)"
        return $existing
    }

    $output = powercfg /duplicatescheme $activeGuid
    if ($output -match "GUID:\s+([a-fA-F0-9\-]+)") {
        $newGuid = $matches[1]
        powercfg /changename $newGuid $planName
        Write-Host "✅ Created $planName with GUID: $newGuid"
        return $newGuid
    } else {
        Write-Error "❌ Failed to create $planName from active scheme."
        return $null
    }
}

# Create both profiles
$dayGuid = Create-PlanIfMissing "DayProfile"
$nightGuid = Create-PlanIfMissing "NightProfile"

# Summary
Write-Host "`n🎉 Power plans setup complete."
Write-Host "DayProfile GUID: $dayGuid"
Write-Host "NightProfile GUID: $nightGuid"
