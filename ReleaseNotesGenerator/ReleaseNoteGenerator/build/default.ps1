$psake.use_exit_on_error = $true
properties {
   $msbuildConfig = "Debug"

   $projectExt = ".csproj"
   $currentDir = resolve-path .
   $Invocation = (Get-Variable MyInvocation -Scope 1).Value
   $scriptDir = $psake.build_script_dir 
   $baseDir = Join-Path $psake.build_script_dir ".." | Resolve-Path 
   $artifactDir = Join-Path $baseDir "artifacts"
   $artifactsConfigDir = Join-Path $artifactDir $msbuildConfig
   $sfxArtifacts = Join-Path $artifactsConfigDir "*"
   $nugetDir = Join-Path $artifactDir "nuget\Release"
   $nugetDebugDir = Join-Path $artifactDir "nuget\Debug"
   $srcDir = Join-Path $baseDir "ReleaseNoteGenerator"
   $nugetDebugFeed = "http://proget:81/nuget/DevFeed"
   $nugetFeed = "http://proget:81/nuget/ReleaseFeed"
   $nuspecList = @("ReleaseNoteGenerator.nuspec")

   $toolsDir = Join-Path $scriptDir "Tools\"

   $nugetExe = Join-Path $toolsDir "NuGet\nuget.exe"
   $toolsPackages = Join-Path $toolsDir "packages"
   $toolsPackagesConfig = Join-Path $toolsDir "packages.config"
  
   $gitVersionName = "GitVersion.exe"
   $7zName = "7zr.exe"  
   $slnPath = Join-Path $baseDir "ReleaseNoteGenerator.sln"

   $echoargs = "C:\Program Files (x86)\PowerShell Community Extensions\Pscx3\Pscx\Apps\EchoArgs.exe"
}

function  Find-Tool( [string]$toolName )
{
   $filename = $toolName
   $exePath = dir $toolsPackages -recurse | where { $_.PSIsContainer -eq $false -and $_.Name -eq $filename } | foreach { $_.FullName } | sort -descending | select -first 1 
   $exePath -as [string]
}

Task default -Description "Build and run tests." -depends Build
Task Rebuild -Description "Clean, build, and run tests." -depends Clean, Build
Task IntegrationBuild -Description "The build process used by Jenkins." -depends CleanGenerated, Clean, Build
Task LocalPublish -Description "Publish locally to artifacts folder" -depends IntegrationBuild, NuGetUpdateDependencies, NuGetPack
Task ServerBuild -Description "Jenkins build and copy coverage report" -depends IntegrationBuild
Task FLP -Description "Fast local publish. Just builds and runs NuGet pack." -depends CleanGenerated, Build, NuGetUpdateDependencies, NuGetPack # fast test of local publishing
Task Publish -Description "Publish to ProGet" -depends LocalPublish, Test, NuGetDebugPush, NuGetPush
Task FCup -Description "Update Flow-Cal dependencies from ProGet" -depends NuGetRestore, Update-FCDep

Task CleanGenerated -maxRetries 5 {
    if (Test-Path "$artifactDir") {
      Remove-Item "$artifactDir" -Recurse -Force
    }

    mkdir "$artifactDir"    
}

Task CheckForRequiredPackages {
  Write-Host "Getting Projects for Solution..."
 
  $list = new-Object System.Collections.ArrayList
  $xml =  (Get-Content $slnPath |
     Select-String 'Project\(' |
    ForEach-Object {
      $projectParts = $_ -Split '[,=]' | ForEach-Object { $_.Trim('[ "{}]') };
      $node= New-Object PSObject -Property @{
      Name = $projectParts[1];
      File = $projectParts[2];
      Guid = $projectParts[3]
      }
      $list.Add($node);
         });

  Write-Host "Checking projects package.config for required packages..."
  foreach($node in $list){  
    $name = [io.path]::combine($baseDir, $node.File).ToString();
    $path = [io.path]::GetDirectoryName($name)
    $pathext = [io.path]::GetExtension($name)
    
    if($pathext -eq $projectExt){
      Write-Host "Project Path $name"
      $package = [io.path]::combine($path, 'packages.config').ToString();
      if([System.IO.File]::Exists($package)){   
        $xmldata = [xml](Get-Content $package)    
      }else{
        Write-Host "No package.config exist for $name"
      }
    }
  }
}

Task Build -depends  NuGetRestore {
   Exec { msbuild $slnPath "/target:build" "/p:Configuration=Debug"  }
   Exec { msbuild $slnPath "/target:build" "/p:Configuration=$msbuildConfig"  }
}

Task Clean -depends  NuGetRestore {
    Exec { msbuild $slnPath "/target:build" "/p:Configuration=Debug"  }
  Exec { msbuild $slnPath "/target:clean" "/p:Configuration=$msbuildConfig"  }
}

Task NuGetRestore -depends CheckForRequiredPackages {
   Exec { .$nugetExe restore $slnPath }

   Exec { .$nugetExe restore $toolsPackagesConfig -PackagesDirectory $toolsPackages }
   
   $script:gitVersionExe = Find-Tool $gitVersionName
   $script:7z = Find-Tool $7zName
}

Task Remove-NuGetRestore {
    $RemoveNuGetRestoreScript = Join-Path $scriptDir "Remove-NuGetRestore.ps1"
    Exec { .$RemoveNuGetRestoreScript $baseDir }
}

Task Test -ContinueOnError {
    $script:nunitConsole = Find-Tool $nunitConsoleName

    $testList = Get-ChildItem $nunitTestGlob | % { $_.FullName }
    #Exec { .$echoargs $nunitConsole $testList $nunitArgsArray }
    Exec { .$nunitConsole $testList $nunitArgsArray }
}

Task Update-Tools {
    pushd $toolsDir
    Exec { .$nugetExe update $toolsPackagesConfig -repositoryPath $toolsPackages }

    popd
}

Task NuGetUpdateDependencies {
   ForEach ($nuspec in $nuspecList) {
      $nuspecPath = Join-Path $srcDir $nuspec
      $projectDirectory = Split-Path $nuspecPath 
      $nuspecFile = Split-Path $nuspecPath -leaf
      $projFile = ([io.path]::ChangeExtension($nuspecFile, $projectExt))
      
      $UpdateDependenciesScript = Join-Path $scriptDir Update-Dependencies.ps1
      Exec { .$UpdateDependenciesScript -projectNuspec $nuspecFile -projectName $projFile  -projectPath $projectDirectory}
      
   }
}

Task Update-FCDep {
    Write-Host "Updating FlowCal dependencies for $slnPath"
    Exec { .$nugetExe update $slnPath -NonInteractive -verbosity detailed -Id $packageList  }
}

Task NuGetPack {
   mkdir $nugetDir
   mkdir $nugetDebugDir
   $script:gitVersionExe = Find-Tool $gitVersionName

   $nugetVersion = '1.0.0.1' #.$gitVersionExe /output json /showvariable NuGetVersionV2
   ForEach ($nuspec in $nuspecList) {
      $nuspecFile = Join-Path $srcDir $nuspec
      
      Exec { .$nugetExe pack "$nuspecFile" -OutputDirectory "$nugetDir" -IncludeReferencedProjects -Version $nugetVersion -Prop Configuration=Release -Verbosity detailed }
      Exec { .$nugetExe pack "$nuspecFile" -OutputDirectory "$nugetDebugDir" -IncludeReferencedProjects -Version $nugetVersion -Prop Configuration=Debug -Verbosity detailed }
   }
}

Task NuGetPush {
   Exec { .$nugetExe push "$nugetDir\*.nupkg" -Source "$nugetFeed" -Verbosity detailed }
}

Task NuGetDebugPush {
   Exec { .$nugetExe push "$nugetDebugDir\*.nupkg" -Source "$nugetDebugFeed" -Verbosity detailed }
}

Task CreateTag {
    $script:gitVersionExe = Find-Tool $gitVersionName
    $nugetVersion = '1.0.0.1' #.$gitVersionExe /output json /showvariable NuGetVersionV2
    Exec { git tag $nugetVersion }
    Exec { git push origin $nugetVersion }
}