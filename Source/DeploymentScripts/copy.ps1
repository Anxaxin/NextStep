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

write-host 'Doing nothing'
