include .\extensions.ps1

properties {
    $name                       = "Crowbar"
    $version                    = "0.10"
    
    # files that should be part of the nuget.
    $nuget_core_package_files     = @( "$name.???" )
    $nuget_mustache_package_files = @( "$name.Mustache.???" )

    $root                       = Resolve-Path .
    
    # nuget
    $nuspec                     = "$name.nuspec"
    $nuspec_file                = "$root\$nuspec"
                                
    # build                     
    $build_path                 = "$root\build"
    $build_binaries_path        = "$build_path\binaries\"
    $build_nuget_path           = "$build_path\nuget"
    $build_nuget_core_path      = "$build_nuget_path\core"
    $build_nuget_mustache_path  = "$build_nuget_path\mustache"
                                
    # source                    
    $source_path                = "$root\src"
    $solution_file              = "$source_path\$name.sln"
                                
    # nuget                     
    $nuspec_core                = "$name.nuspec"
    $nuspec_core_build_file     = "$build_nuget_core_path\$nuspec_core"
    $nuspec_core_file           = "$root\$nuspec_core"
    
    $nuspec_mustache            = "$name.Mustache.nuspec"
    $nuspec_mustache_build_file = "$build_nuget_mustache_path\$nuspec_mustache"
    $nuspec_mustache_file       = "$root\$nuspec_mustache"
}

Framework "4.0"

task default -depends build

task clean {
    Remove-Item -Force -Recurse -ErrorAction SilentlyContinue -LiteralPath $build_path
}

task build -depends clean {
    # read metadata from nuspec and update AssemblyInfo.
    $metadata = ([xml](Get-Content $nuspec_file)).package.metadata
    
    # the AssemblyVersionAttribute is no fan of beta versions and will fail the build, thus
    # we will be using the syntax major[.minor[.patch]].beta-version in AssemblyInfo.
    $assembly_version = $version -replace '(\d+\\.)?(\d+\\.)?(\d+)-beta(\d+)', '$1$2$3.$4'
    
    Generate-Assembly-Info `
        -file "$source_path\AssemblyInfo.cs" `
        -title $metadata.id + " $version" `
        -description $metadata.description `
        -product $metadata.id `
        -version $assembly_version `
        -copyright "Copyright (c) Mattias Rydengren 2013"

    New-Item $build_binaries_path -ItemType Directory | Out-Null
    msbuild $solution_file /p:OutDir=$build_binaries_path /p:Configuration=Release /v:q
}

task nuget-core -depends build {
    $build_artifacts = "$build_nuget_path\core"

    # copy files that should be part of the nuget.
    New-Item $build_artifacts\lib\net40 -ItemType Directory | Out-Null
    $nuget_core_package_files |% { Copy-Item $build_binaries_path\$_ $build_artifacts\lib\net40 }

    # make a copy of the nuspec making temporary edits possible.
    Copy-Item $nuspec_core_file $nuspec_core_build_file
    Copy-Item "$root\nuget.core.readme.txt" "$build_nuget_core_path\readme.txt"
    
	# update version in the nuspec copy.
    $content = [xml](Get-Content $nuspec_core_build_file)
	$content.package.metadata.version = $version
	$content.Save($nuspec_core_build_file)
    
    & "$source_path\.nuget\NuGet.exe" pack $nuspec_core_build_file -o $build_artifacts
}

task nuget-mustache -depends build {
    $build_artifacts = "$build_nuget_path\mustache"

    # copy files that should be part of the nuget.
    New-Item $build_artifacts\lib\net40 -ItemType Directory | Out-Null
    $nuget_mustache_package_files |% { Copy-Item $build_binaries_path\$_ $build_artifacts\lib\net40 }

    # make a copy of the nuspec making temporary edits possible.
    Copy-Item $nuspec_mustache_file $nuspec_mustache_build_file
    
	# update version in the nuspec copy.
    $content = [xml](Get-Content $nuspec_mustache_build_file)
	$content.package.metadata.version = $version
	$content.Save($nuspec_mustache_build_file)
    
    & "$source_path\.nuget\NuGet.exe" pack $nuspec_mustache_build_file -o $build_artifacts
}

task nuget -depends nuget-core, nuget-mustache {
    # noop
}