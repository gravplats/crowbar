# This script is derived from the dotless build script.
include .\extensions.ps1

properties {
    $path              = resolve-path .
    $buildDirectory    = "$path\build"
	$binariesDirectory = "$buildDirectory\binaries\"
    $nugetDirectory    = "$buildDirectory\nuget\"
    $releaseDirectory  = "$buildDirectory\release\"
    $sourceDirectory   = "$path\src"
    $solution          = "$sourceDirectory\Crowbar.sln"	
    $toolsDirectory    = "$path\tools\"
    $version           = "0.9"
}

$framework = '4.0'

task default -depends release

task clean {
    Remove-Item -Force -Recurse -ErrorAction SilentlyContinue -LiteralPath $buildDirectory
}

task build -depends clean {
    New-Item $binariesDirectory -ItemType Directory | Out-Null

    Generate-Assembly-Info `
        -file "$sourceDirectory\Crowbar\Properties\AssemblyInfo.cs" `
        -title "Crowbar $version" `
        -description "Application testing for ASP.NET MVC 3 and 4" `
        -product "Crowbar" `
        -version $version `
        -copyright "Copyright (C) Mattias Rydengren 2012-2013" `

    msbuild $solution /p:OutDir=$binariesDirectory /p:Configuration=Release | Out-Null
}

task release -depends build {
    New-Item $releaseDirectory -ItemType Directory | Out-Null

    & $toolsDirectory\7-zip\7za.exe a $releaseDirectory\crowbar-$version-release-net-4.0.zip `
        $binariesDirectory\Crowbar.dll `
        $binariesDirectory\Crowbar.pdb `
        $binariesDirectory\Crowbar.xml `
        LICENSE | Out-Null
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
        $binariesDirectory\Crowbar.dll, `
        $binariesDirectory\Crowbar.pdb, `
        $binariesDirectory\Crowbar.xml 
    
    & $toolsDirectory\nuget-cli\nuget.exe pack $nugetDirectory\Crowbar.nuspec -o $nugetDirectory | Out-Null
    Remove-Item -LiteralPath $nugetDirectory\lib, $nugetDirectory\Crowbar.nuspec -Force -Recurse -ErrorAction SilentlyContinue
}