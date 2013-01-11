# This script is derived from the dotless build script.
function Generate-Assembly-Info
{
    param(
        [string]$file = $(throw "file is a required parameter."),
        [string]$title,
        [string]$description,
        [string]$company,
        [string]$product,
        [string]$copyright,
        [string]$version		
    )
    
    $assemblyInfo = 
"using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyTitle(""$title"")]
[assembly: AssemblyDescription(""$description"")]
[assembly: AssemblyCompany(""$company"")]
[assembly: AssemblyProduct(""$product"")]
[assembly: AssemblyCopyright(""$copyright"")]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyInformationalVersion(""$version"")]
[assembly: AssemblyFileVersion(""$version"")]
[assembly: AssemblyDelaySign(false)]"

    Generate-File-Content `
        -file $file `
        -content $assemblyInfo
}

function Generate-NuGet-Spec
{
    param(
        [string]$file = $(throw "file is a required parameter."),
        [string]$id = $(throw "id is a required parameter."),
        [string]$version = $(throw "version is a required parameter."),
        [string]$description = $(throw "description is a required parameter."),
        [string]$author = $(throw "author is a required parameter."),
        [string]$licenseUrl = $(throw "licenseUrl is a required parameter."),
        [string]$projectUrl = $(throw "projectUrl is a required parameter."),
        [string]$tags = $(throw "tags is a required parameter.")
    )

    $nugetSpec =
"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>$id</id>
    <version>$version</version>
    <description>$description</description>
    <authors>$author</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <licenseUrl>$licenseUrl</licenseUrl>
    <projectUrl>$projectUrl</projectUrl>
    <tags>$tags</tags>
	<dependencies>
	  <dependency id=""CsQuery"" version=""[1.3,1.4)"" />
	</dependencies>
  </metadata>
</package>"

    Generate-File-Content `
        -file $file `
        -content $nugetSpec
}

function Generate-File-Content
{
    param(
        [string]$file = $(throw "file is a required parameter."),
        [string]$content = $(throw "content is a required parameter.")
    )
    
    $dir = [System.IO.Path]::GetDirectoryName($file)
    if ([System.IO.Directory]::Exists($dir) -eq $false)
    {
        [System.IO.Directory]::CreateDirectory($dir)
    }

    Out-File -FilePath $file -Encoding UTF8 -InputObject $content
}