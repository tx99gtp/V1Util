# This powershell script helps remove all NuGet configurations and files that got added while enabling NuGet Restore. 
# More details @ http://rahulpnath.com/blog/disable-nuget-package-restore-for-a-net-poject/
param([Parameter(Mandatory=$true)][string]$solutionDirectory) 

 $importNugetTargetsTag= [regex]::escape(@'
<Import Project="$(SolutionDir)\.nuget\NuGet.targets" Condition="Exists('$(SolutionDir)\.nuget\NuGet.targets')" />
'@)

$restorePackagesTag = '<RestorePackages>.*?</RestorePackages>'
$nuGetPackageImportStamp = '<NuGetPackageImportStamp>.*?</NuGetPackageImportStamp>'

$EnsureNuGetPackageBuildImportsTargetTag = '(?smi)<Target Name="EnsureNuGetPackageBuildImports".*?</Target>'

foreach ($f in Get-ChildItem -Recurse -Path $solutionDirectory -Filter *.csproj | sort-object)
{
    $text = Get-Content $f.FullName -Raw
    $text `
        -replace $importNugetTargetsTag, "" `
        -replace $nuGetPackageImportStamp, "" `
        -replace $restorePackagesTag, "" `
        -replace $EnsureNuGetPackageBuildImportsTargetTag, "" `
        | set-content $f.FullName
}

Get-ChildItem -Path $solutionDirectory -include .nuget -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }