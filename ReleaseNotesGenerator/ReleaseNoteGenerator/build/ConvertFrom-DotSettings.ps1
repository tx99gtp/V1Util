<#
.SYNOPSIS
Converts *.sln.DotSettings file to xml format suitable to be used with dotcover from command line

.DESCRIPTION
Converts DotSettings file (created from DotCover VS Extension) to its xml representation that can be used with dotcover from command line

.PARAMETER inputFileath
A xxx.sln.DotSettings file

.PARAMETER executable
Value for the "<TargetExecutable>" node in the generated Xml

.PARAMETER arguments
Value for the "<TargetArguments>" node in the generated Xml

.PARAMETER testGlob
Produces list of test assemblies to be added to the "<TargetArguments>" node in the generated Xml

.PARAMETER coverageOutput
Value for the "<Output>" node in the generated Xml

.PARAMETER workingDirectory
Value for the "<TargetWorkingDir>" node in the generated Xml

.PARAMETER reportType
Value for the "<ReportType>" node in the generated Xml


.NOTES
Author : Nikolay Pshenichny
Modified by : Ben Brandt

Requires PowerShell v3 or higher.

.EXAMPLE
.\ConvertFrom-DotSettings.ps1 -inputFile MySolution.sln.DotSettings
#> 
param
(
   [Parameter(Mandatory=$true)]
   [string]$inputFile,
   [string]$executable = "C:\Program Files (x86)\NUnit 2.6.4\bin\nunit-console.exe",
   [Parameter(Mandatory=$true)]
   [string]$arguments,
   [string]$testGlob,
   [string]$coverageOutput, 
   [string]$workingDirectory,
   [string]$reportType = "HTML"
)


function ToFilterEntryNode([System.Xml.XmlDocument]$ownerDocument, [System.Xml.XmlElement]$dotCoverFilterNode)
{
   $filtersFragment = $ownerDocument.CreateDocumentFragment()
   $filtersFragment.InnerXml = @"

   <FilterEntry>
      <ModuleMask>$($dotCoverFilterNode.ModuleMask)</ModuleMask>
      <ModuleVersionMask>$($dotCoverFilterNode.ModuleVersionMask)</ModuleVersionMask>
      <ClassMask>$($dotCoverFilterNode.ClassMask)</ClassMask>
      <FunctionMask>$($dotCoverFilterNode.FunctionMask)</FunctionMask>
      <IsEnabled>$($dotCoverFilterNode.IsEnabled)</IsEnabled>
   </FilterEntry>
"@
    return $filtersFragment
}

if (!$workingDirectory) {
   $workingDirectory = Join-Path $PSScriptRoot "..\src\"
}

$workingDirectory = Convert-Path $workingDirectory

pushd $workingDirectory


# Read the xxx.sln.DotSettings file
[xml]$dotSettingsFile = Get-Content $inputFile

# Get the string resource that has filters and convert it to xml
$resource = $dotSettingsFile.ResourceDictionary.String | Where {$_.Key -eq "/Default/FilterSettingsManager/CoverageFilterXml/@EntryValue"} | Select-Object Key, InnerText
$filters = [xml]$resource.InnerText

if (!$coverageOutput) {
   $coverageOutput = Join-Path $workingDirectory "coverage-results"
   $inputBaseName = [io.path]::GetFileNameWithoutExtension($inputFile)
   $coverageOutput = Join-Path $coverageOutput $inputBaseName
   $coverageOutput = [io.path]::ChangeExtension($coverageOutput, ".$reportType")
}

if($testGlob)
{
	$testList = (Get-ChildItem -recurse $testGlob | % { $_.FullName }) -join " "
	$arguments = "$arguments $testList"
}


# Prepare output "template"
[xml]$output = @"
<?xml version="1.0" encoding="utf-8"?>
<AnalyseParams>
   <TargetExecutable>$executable</TargetExecutable>
   <TargetWorkingDir>$workingDirectory</TargetWorkingDir>
   <TargetArguments>$arguments</TargetArguments>
   <Output>$coverageOutput</Output>
   <ReportType>$reportType</ReportType>   
   <Filters>
      <IncludeFilters/>
      <ExcludeFilters/>
   </Filters>
</AnalyseParams>
"@

# Convert "IncludeFilters" data
$includeFilters = $filters.SelectSingleNode("//data/IncludeFilters")
if ($includeFilters.HasChildNodes -eq $True)
{
   $outputIncludeFiltersNode = $output.SelectSingleNode("//AnalyseParams/Filters/IncludeFilters")

   foreach($includeFilter in $includeFilters.Filter)
   {
      $filterEntry = ToFilterEntryNode -ownerDocument $output -dotCoverFilterNode $includeFilter
      $appendedChild = $outputIncludeFiltersNode.AppendChild($filterEntry)
   }
}

# Convert "ExcludeFilters" data
$excludeFilters = $filters.SelectSingleNode("//data/ExcludeFilters")
if ($excludeFilters.HasChildNodes -eq $True)
{
   $outputIncludeFiltersNode = $output.SelectSingleNode("//AnalyseParams/Filters/ExcludeFilters")

   foreach($excludeFilter in $excludeFilters.Filter)
   {
      $filterEntry = ToFilterEntryNode -ownerDocument $output -dotCoverFilterNode $excludeFilter
      $appendedChild = $outputIncludeFiltersNode.AppendChild($filterEntry)
   }
}

#Note: Writing to console output directly (ie. $output.Save([Console]::Out)) will change encoding to IBM437
$memoryStream = New-Object System.IO.MemoryStream
$output.Save($memoryStream)
$memoryStream.Position = 0
$reader = New-Object System.IO.StreamReader $memoryStream
$reader.ReadToEnd()

popd