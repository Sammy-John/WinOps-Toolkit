# Load configuration file (JSON) containing source path and folder rules
$configPath = "D:\Scripts\DownloadSorter\config.json"
$config = Get-Content $configPath | ConvertFrom-Json

# Set source path and destination base, resolving %USERPROFILE% placeholders
$source = $config.source -replace "%USERPROFILE%", $env:USERPROFILE
$rules = $config.rules

# Get path to known Windows folders like Documents or Pictures
function Get-KnownFolderPath {
    param ([string]$name)
    switch ($name) {
        "Documents" { return "D:\Documents" }
        "Pictures"  { return "D:\Pictures" }
        "Videos"    { return "D:\Videos" }
        "PDFs"      { return "D:\PDFs" }
        "Installers"{ return "D:\Installers" }
        default     { return Join-Path "D:\" $name }
    }
}


# Initialize logging directory and log file
$logDir = "D:\Scripts\DownloadSorter\Logs"
if (-not (Test-Path $logDir)) { New-Item -ItemType Directory -Path $logDir | Out-Null }
$logFile = Join-Path $logDir ("DownloadSort_" + (Get-Date -Format "yyyy-MM-dd") + ".log")


# Define helper function to write messages to the log file
function Log {
    param ([string]$msg)
    Add-Content -Path $logFile -Value $msg
}

Log "Logging initialized: $logFile"
Write-Host "Logging initialized: $logFile"


# Log the start of the sorting process
Log "=== Sorting started at $(Get-Date) ==="
Log "Source folder: $source"

# Loop through each rule in the config
foreach ($folder in $rules.PSObject.Properties.Name) {
    $patterns = $rules.$folder

    # Use known folder paths like Documents/Pictures if available
    $target = Get-KnownFolderPath $folder

    # Create destination folder if it doesn't exist
    if (-not (Test-Path $target)) {
        New-Item -ItemType Directory -Path $target | Out-Null
        Log "Created folder: $target"
    }

    # Special handling for folder rules
    if ($patterns -contains "<FOLDERS>") {
        # Get all directories (excluding system/hidden)
        $folders = Get-ChildItem -Path $source -Directory -Force | Where-Object { $_.Name -ne $folder }
        foreach ($dir in $folders) {
            $destination = Join-Path $target $dir.Name
            $baseName = $dir.Name
            $counter = 1

            while (Test-Path $destination) {
                $newName = "$baseName ($counter)"
                $destination = Join-Path $target $newName
                $counter++
            }

            Move-Item -Path $dir.FullName -Destination $destination
            Log "Moved folder '$($dir.Name)' to '$destination' (renamed if needed)"
        }
    }
    else {
        # Handle regular file pattern rules
        foreach ($pattern in $patterns) {
			$files = Get-ChildItem -Path $source -Filter $pattern -File -ErrorAction SilentlyContinue
			Log "Pattern: $pattern → Found $($files.Count) file(s)"
			Write-Host "Pattern: $pattern → Found $($files.Count) file(s)"

			foreach ($file in $files) {
				$baseName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
				$ext = [System.IO.Path]::GetExtension($file.Name)
				$destination = Join-Path $target $file.Name
				$counter = 1

				while (Test-Path $destination) {
					$newName = "$baseName ($counter)$ext"
					$destination = Join-Path $target $newName
					$counter++
				}

				Move-Item -Path $file.FullName -Destination $destination
				Log "Moved '$($file.Name)' to '$destination' (renamed if needed)"
				Write-Host "Moved '$($file.Name)' to '$destination'"
			}
		}
    }
}

# Log the end of the sorting process
Log "=== Sorting complete ===`n"
