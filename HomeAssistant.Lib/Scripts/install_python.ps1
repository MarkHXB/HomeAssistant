# Check if Python is installed
if (-not (Test-Path (Join-Path $env:ProgramFiles 'Python'))) {
    # Install Python
    Invoke-WebRequest -Uri "https://www.python.org/ftp/python/3.10.2/python-3.10.2-amd64.exe" -OutFile "$env:TEMP\python-3.10.2-amd64.exe"
    Start-Process -FilePath "$env:TEMP\python-3.10.2-amd64.exe" -ArgumentList "/quiet", "/passive", "/norestart" -Wait
}

# Install required Python packages from all requirements.txt files found recursively
$requirementsFiles = Get-ChildItem -Path $PSScriptRoot -Filter 'requirements.txt' -Recurse
foreach ($file in $requirementsFiles) {
    $requirementsPath = $file.FullName
    Write-Host "Installing Python packages from $requirementsPath"
    pip install -r $requirementsPath
}
