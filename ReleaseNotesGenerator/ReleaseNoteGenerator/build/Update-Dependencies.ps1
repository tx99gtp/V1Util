param([string]$projectNuspec, [string]$projectName, [string]$projectPath)

Function DisplayInfo([string] $text)
{
   Write-Host ""
   Write-Host "****************************"
   Write-Host $text
   Write-Host "****************************"
   Write-Host ""
}

Function RemoveDependencies([System.Xml.XmlNode]$dependenciesGroup)
{
   foreach($dependency in $dependenciesGroup.Dependency)
   {
      if(!$dependency.version.Contains("$version$"))
      {
         $dependenciesGroup.RemoveChild($dependency)
      }
   } 
}

Function RemoveDependenciesFromGroups($dependenciesGroupList)
{
   foreach($group in $dependenciesGroupList)
   {
      RemoveDependencies $group
   }
}

Function AddPackagesToAllDependencyGroups($dependenciesGroupList, $packageList, $nuspecDoc)
{
   foreach($group in $dependenciesGroupList)
   {
      AddPackagesToDependencyGroups $group $packageList $nuspecDoc
   }
}

Function AddPackagesToDependencyGroups([System.Xml.XmlNode]$dependenciesGroup, $packageList, $nuspecDoc)
{
    foreach($package in $packageList)
    {
        if(!$package.id.Equals("") -And !$package.developmentDependency)
        {
            Write-Host "Create dependency for: "$package.id"version: "$package.version
            $dependency = $nuspecDoc.CreateElement('dependency')   
            $dependency.SetAttribute('id',$package.id)
            $packageVersion = [string]::Format("{0}",$package.version)
            $dependency.SetAttribute('version',$packageVersion)             
            $dependenciesGroup.AppendChild($dependency)          
        }   
    }
}

$csprojMustBeSaved = $false
$packageFileName = [System.IO.Path]::Combine($projectPath, "packages.config")

If(Test-Path  $packageFileName)
{
    $csprojMustBeSaved = $false
    Write-host "Project is:" $projectName
    Write-Host "Nuspec file is:" $packageFileName   
    $projectFullName = [System.IO.Path]::Combine($projectPath, $projectName)
    $nuspecFullName = [System.IO.Path]::Combine($projectPath, $projectNuspec)

    Write-Host "Open Nuspec File: " $packageFileName
    $nuspecStream =  [System.IO.File]::OpenText($nuspecFullName)
    $nuspecDoc = New-Object System.Xml.XmlDocument  
    $nuspecDoc.Load($nuspecStream)
    $nuspecStream.Dispose()

    Write-Host "Open project File: " $projectFullName
    $csprojStream =  [System.IO.File]::OpenText($projectFullName)
    $csprojDoc = New-Object System.Xml.XmlDocument
    $csprojDoc.Load($csprojStream)  
    $csprojStream.Dispose()

    $xnm = New-Object System.Xml.XmlNamespaceManager(New-Object System.Xml.NameTable);
    $xnm.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

    [System.Xml.XmlNode]$metaData = $nuspecDoc.package.metadata

    DisplayInfo("Open packages.config File: $packageFileName")
    $packageFile = New-Object System.Xml.XmlDocument    
    $packageFile.Load($packageFileName)

    if($metaData.dependencies)
    {
        DisplayInfo("Updating dependencies in nuspec")
        foreach($dependencies in $metaData.dependencies)
        {
           $allgroups = $dependencies.group
           RemoveDependenciesFromGroups $allgroups
           AddPackagesToAllDependencyGroups $allgroups $packageFile.packages.ChildNodes $nuspecDoc
        }       
        DisplayInfo("Dependencies Updated")
    }
    else
    {
        $dependencies = $nuspecDoc.CreateElement('dependencies')   
        $nuspecDoc.package.metadata.AppendChild($dependencies)

        $dependenciesGroup = $nuspecDoc.CreateElement('group')
        $dependencies.AppendChild($dependenciesGroup)
        
        AddPackagesToDependencyGroups $dependenciesGroup $packageFile.packages.ChildNodes $nuspecDoc
    }

    if($csprojMustBeSaved -eq $true)
    {
        Write-Host "Save Csproj File: "$projectFullName
        $csprojDoc.Save($projectFullName)

        Write-host "File Saved successfully" 
    }

    Write-Host "Save Nuspec File: " $projectPath$projectNuspec
    $nuspecDoc.Save($nuspecFullName);   
    Write-host "File Saved successfully" 
}
else
{
    Write-Host "There is no " $packageFileName " skip this step"  
}