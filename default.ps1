# This script is derived from the dotless build script.
include .\extensions.ps1

properties {
    $name                   = "Crowbar"
    $version                = "0.9"
    
    # files that should be part of the nuget.
    $nuget_package_files    = @( "$name.???")

    $root                   = Resolve-Path .
    
    # nuget
    $nuspec                 = "$name.nuspec"
    $nuspec_file            = "$root\$nuspec"
    
    # build
	$build_path             = "$root\build"
	$build_binaries_path    = "$build_path\binaries\"
    $build_nuget_path       = "$build_path\nuget"
    $nuspec_build_file      = "$build_path\nuget\$nuspec"
    
    # source
	$source_path            = "$root\src"
    $solution_file          = "$source_path\$name.sln"    
}

$framework = '4.0'

task default -depends build

task clean {
    Remove-Item -Force -Recurse -ErrorAction SilentlyContinue -LiteralPath $build_path
}

task build -depends clean {
    # read metadata from nuspec and update AssemblyInfo.
    $metadata = ([xml](Get-Content $nuspec_file)).package.metadata
    Generate-Assembly-Info `
        -file "$source_path\AssemblyInfo.cs" `
        -title $metadata.id + " $version" `
        -description $metadata.description `
        -product $metadata.id `
        -version $version `
        -copyright "Copyright (c) Mattias Rydengren 2013"

    New-Item $build_binaries_path -ItemType Directory | Out-Null
    msbuild $solution_file /p:OutDir=$build_binaries_path /p:Configuration=Release /v:q
}

task nuget -depends build {
    # copy files that should be part of the nuget.
    New-Item $build_nuget_path\lib\net40 -ItemType Directory | Out-Null
    $nuget_package_files |% { Copy-Item $build_binaries_path\$_ $build_nuget_path\lib\net40 }

    # make a copy of the nuspec making temporary edits possible.
    Copy-Item $nuspec_file $nuspec_build_file
    
	# update version in the nuspec copy.
    $content = [xml](Get-Content $nuspec_build_file)
	$content.package.metadata.version = $version
	$content.Save($nuspec_build_file)
    
    & "$source_path\.nuget\NuGet.exe" pack $nuspec_build_file -o $build_nuget_path | Out-Null
}