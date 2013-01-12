# This script is derived from the dotless build script.
include .\extensions.ps1

properties {
    $path            = resolve-path .
    
	$path_build      = "$path\build"
	$path_binaries   = "$path_build\binaries\"
    $path_nuget      = "$path_build\nuget\"
    $path_release    = "$path_build\release\"
	$nuspec          = "$path_nuget\Crowbar.nuspec"
    
	$path_source     = "$path\src"
    $solution        = "$path_source\Crowbar.sln"	
    
	$path_tools      = "$path\tools\"
	
    $version         = "0.9"
}

$framework = '4.0'

task default -depends release

task clean {
    Remove-Item -Force -Recurse -ErrorAction SilentlyContinue -LiteralPath $path_build
}

task build -depends clean {
    New-Item $path_binaries -ItemType Directory | Out-Null

    Generate-Assembly-Info `
        -file "$path_source\Crowbar\Properties\AssemblyInfo.cs" `
        -title "Crowbar $version" `
        -description "Application testing for ASP.NET MVC 3 and 4" `
        -product "Crowbar" `
        -version $version `
        -copyright "Copyright (C) Mattias Rydengren 2012-2013" `

    msbuild $solution /p:OutDir=$path_binaries /p:Configuration=Release | Out-Null
}

task release -depends build {
    New-Item $path_release -ItemType Directory | Out-Null

    & $path_tools\7-zip\7za.exe a $path_release\crowbar-$version-release-net-4.0.zip `
        $path_binaries\Crowbar.dll `
        $path_binaries\Crowbar.pdb `
        $path_binaries\Crowbar.xml `
        LICENSE | Out-Null
}

task nuget -depends release {
    New-Item $path_nuget\lib\net40 -ItemType Directory | Out-Null
		
	Copy-Item Crowbar.nuspec $path_nuget
	
	# Update version in nuspec.
    $content = [xml](Get-Content $nuspec)
	$content.package.metadata.version = $version
	$content.Save($nuspec)
		
	Copy-Item -Destination $path_nuget\lib\net40 -LiteralPath `
        $path_binaries\Crowbar.dll, `
        $path_binaries\Crowbar.pdb, `
        $path_binaries\Crowbar.xml 
    
    & $path_tools\nuget-cli\nuget.exe pack $nuspec -o $path_nuget | Out-Null
    Remove-Item -LiteralPath $path_nuget\lib, $nuspec -Force -Recurse -ErrorAction SilentlyContinue
}