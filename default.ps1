# This script is derived from the dotless build script.
include .\extensions.ps1

properties {
    $path             = resolve-path .
    $buildDirectory   = "$path\build\"
    $nugetDirectory   = "$path\nuget"
    $releaseDirectory = "$path\release\"
    $sourceDirectory  = "$path\src"
    $solution         = "$sourceDirectory\Crowbar.sln"	
    $toolsDirectory   = "$path\tools\"
    $version          = "0.9"
}

$framework = '4.0'

task default -depends release

task clean {
    Remove-Item -Force -Recurse -ErrorAction SilentlyContinue -LiteralPath $buildDirectory, $nugetDirectory, $releaseDirectory
}

task build -depends clean {
    New-Item $buildDirectory -ItemType Directory | Out-Null

    Generate-Assembly-Info `
        -file "$sourceDirectory\Crowbar\Properties\AssemblyInfo.cs" `
        -title "Crowbar $version" `
        -description "Application testing for ASP.NET MVC 3 and 4" `
        -product "Crowbar" `
        -version $version `
        -copyright "Copyright (C) Mattias Rydengren 2012-2013" `

    msbuild $solution /p:OutDir=$buildDirectory /p:Configuration=Release | Out-Null
}

task release -depends build {
    New-Item $releaseDirectory -ItemType Directory | Out-Null

    & $toolsDirectory\7-zip\7za.exe a $releaseDirectory\crowbar-$version-release-net-4.0.zip `
        $buildDirectory\Crowbar.dll `
        $buildDirectory\Crowbar.pdb `
        $buildDirectory\Crowbar.xml `
        license.txt | Out-Null
}

task nuget -depends release {
    New-Item $nugetDirectory\lib\net40 -ItemType Directory | Out-Null
        
    Generate-NuGet-Spec `
        -file "$nugetDirectory\Crowbar.nuspec" `
        -id "Crowbar" `
        -version $version `
        -description "Crowbar is an application testing library for ASP.NET MVC 3 and 4." `
        -author "Mattias Rydengren" `
        -licenseUrl "https://github.com/coderesque/crowbar/blob/master/LICENSE" `
        -projectUrl "https://github.com/coderesque/crowbar" `
        -tags "crowbar testing"
        
    Copy-Item -Destination $nugetDirectory\lib\net40 -LiteralPath `
        $buildDirectory\Crowbar.dll, `
        $buildDirectory\Crowbar.pdb, `
        $buildDirectory\Crowbar.xml 
    
    & $toolsDirectory\nuget-cli\nuget.exe pack $nugetDirectory\Crowbar.nuspec -o $nugetDirectory | Out-Null
    Remove-Item -LiteralPath $nugetDirectory\lib, $nugetDirectory\Crowbar.nuspec -Force -Recurse -ErrorAction SilentlyContinue
}