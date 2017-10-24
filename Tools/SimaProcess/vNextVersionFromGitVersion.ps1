Write-Output "vNextVersionInformation.ps1 version 1.0.0"
Write-Output "Retrieves the next GitVersion number from the repository and sends it back yo the vNext build server"

function Update-NuspecVersion
{
  Param 
  (
    [string]$nugetVersion
  )

  $AllVersionFiles = Get-ChildItem .\Source\* -Include *.nuspec -recurse
  
  foreach ($file in $AllVersionFiles)
  { 
      Write-Host "Modifying nuspec version in file " + $file.FullName
      (Get-Content $file.FullName).replace('$version$', $nugetVersion) | Set-Content $file.FullName
  }
}

$ErrorActionPreference = "Stop"

$sourceBranchName = $Env:BUILD_SOURCEBRANCHNAME
$repositoryName = $Env:BUILD_REPOSITORY_NAME

if (!$repositoryName)
{
    throw "This scripts does not seem to be running inside a vNext build server and with a Git repository"
}

Write-Output "Retrieving current branch version numbers from GitVersion"
$output = & ./Tools/SimaProcess/GitVersion/GitVersion.exe /nofetch /updateassemblyinfo | Out-String
$version = $output | ConvertFrom-Json
$assemblyVersion = $version.AssemblySemver
$assemblyFileVersion = $version.AssemblySemver
$assemblyInformationalVersion = $version.InformationalVersion

Write-Output $output

# If this is rc0 remove the pre-release tag from the version
if ($sourceBranchName -eq "master")
{
  if ($version.FullSemVer.EndsWith("-rc.0"))
  {
    Write-Output "Settings version numbers for release"
    $nugetVersion = $version.MajorMinorPatch
    $buildNumnber = $repositoryName + "-" + $version.MajorMinorPatch
  }
  else
  {
    throw "Unable to get unique version number major.minor.patch, did you forget to GitFlow release with a version number (vX.Y.Z)"
  }
}
else
{
  Write-Output "Settings version numbers for pre-release"
  $nugetVersion = $version.NuGetVersionV2
  $buildNumnber = $repositoryName + "-" + $version.FullSemVer
}

Write-Output "Updating AssemblyInfo files with current branch version numbers"
##./Tools/SimaProcess/GitVersion/GitVersion.exe /updateassemblyinfo /nofetch
Update-NuspecVersion($nugetVersion)

Write-Output "Sending version information back to vNext to use in this build"
Write-Output ("##vso[task.setvariable variable=NugetVersion;]" + $nugetVersion)
Write-Output ("##vso[task.setvariable variable=AssemblyVersion;]" + $assemblyVersion)
Write-Output ("##vso[task.setvariable variable=FileInfoVersion;]" + $assemblyFileVersion)
Write-Output ("##vso[task.setvariable variable=AssemblyInformationalVersion;]" + $assemblyInformationalVersion)
Write-Output ("##vso[task.setvariable variable=build.buildnumber;]" + $buildNumnber)
Write-Output ("##vso[build.updatebuildnumber]" + $buildNumnber)
