Param($newdirectory)

$sitename = "SimaNextStep"

# Check that parameteres are supplied
if ($newdirectory -eq $null)
{
    throw 'Parameter -NewDirectory must be set'
}

# Check for powershell errors and exitcode errors from spawned processes
function CheckError() 
{
    if ($global:lastexitcode -ne 0) 
    {
        throw 'Exitcode error found'
    }

    if ($error.Count -ne 0) 
    {
        throw 'Errors was found, stopping script'
    }

    return 
}

# Clear previous errors
$global:lastexitcode = 0
$error.Clear()

# Load IIS management assembly
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.Web.Administration") > $null
CheckError

# Create a new IIS manager object
$iis = new-object Microsoft.Web.Administration.ServerManager
CheckError

# Find the site which needs change
$site = $iis.sites | Where { $_.Name -eq $sitename }
CheckError

if ($site -eq $null)
{
    throw 'Site "' + $sitename + '" not found'
}

# Find the root application in the site
$application = $site.Applications | Where { $_.Path -eq "/" }
CheckError

if ($application -eq $null)
{
    throw 'Root application not found'
}

# Find the root virtual directory in the application
$directory = $application.VirtualDirectories | Where { $_.Path -eq "/" }
CheckError

if ($directory -eq $null)
{
    throw 'Root directory not found'
}

Write-Host "Current physical path is : " $directory.PhysicalPath

# Check that the new directory exists
if (!(test-path $newdirectory -pathtype container))
{
    throw 'New directory "' + $newdirectory + '" not found'
}

Write-Host "Changing to physical path : " $newdirectory

# Change the physical path for the virtual directory
$directory.PhysicalPath = $newdirectory
CheckError

# Commit changes
$iis.CommitChanges()
CheckError

Write-Host 'Alle changes committed'
