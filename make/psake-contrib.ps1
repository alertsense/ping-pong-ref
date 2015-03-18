function Web-Deploy($projectFile, $profile, $username, $password)
{
	Write-Host "Publishing: $projectFile" -ForeGroundColor GREEN
	Write-Host "PublishProfile: $profile" -ForeGroundColor GREEN
	msbuild $projectFile /v:q /nologo /p:DeployOnBuild=true /p:PublishProfile=$profile /p:VisualStudioVersion=12.0 /p:Password=$password /p:UserName=$username
}

function FixUri
{
	param([URI] $uri)

	$pathAndQuery = $uri.PathAndQuery
	$flagsField = $uri.GetType().GetField("m_Flags", [Reflection.BindingFlags]::NonPublic -bor [Reflection.BindingFlags]::Instance)
	$flagsValue = [UInt64]$flagsField.GetValue($uri)
	# remove flags Flags.PathNotCanonical and Flags.QueryNotCanonical
	$flagsValue = [UInt64]($flagsValue -band (-bnot 0x30));
	$flagsField.SetValue($uri, $flagsValue)
}

function Create-CommonAssemblyInfo($version, $applicationName, $filename)
{
"using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany(""AlertSense, Inc."")]
[assembly: AssemblyProduct(""$applicationName"")]
[assembly: AssemblyCopyright(""Copyright © 2014"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]
[assembly: AssemblyVersion(""$version"")]"  | out-file $filename -encoding "UTF8"      
}