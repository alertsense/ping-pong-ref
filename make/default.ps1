Framework "4.0"

properties {
    $projectName = "AlertSense.PingPong"
    $projectConfig = "Debug"
    $projectVersion = "1.0.0"

	$publishProfile = "Local"
	$publishUserName = ""
	$publishPassword = ""

    $baseDir = Resolve-Path ..\
    $srcDir = join-path $baseDir "\src"

    $rabbitMqApiUrl = "http://localhost:15672/api"
	$rabbitMqVirtualHost = "pingpong"
	$rabbitMqUser = ""
	$rabbitMqPassword = ""
}

task default -depends Init, Compile

task Init {

}

task Compile -depends Init {
    msbuild /t:clean /v:q /nologo /p:"Configuration=$projectConfig;VisualStudioVersion=12.0" $srcDir\$projectName.sln
    msbuild /t:build /v:q /nologo /p:"Configuration=$projectConfig;VisualStudioVersion=12.0" $srcDir\$projectName.sln
}

task CommonAssemblyInfo {
    create-commonAssemblyInfo "$projectVersion" $projectName "$srcDir\CommonAssemblyInfo.cs"
}

task Publish -depends Init, CommonAssemblyInfo, Compile {
	publish-project $srcDir\AlertSense.Aspen\AlertSense.Aspen.csproj $publishProfile $publishUserName $publishPassword
}

task PrepareRabbitMq -depends Init {
	$rabbitMqSecPass = ConvertTo-SecureString $rabbitMqPassword -AsPlainText -Force
	$rabbitMqCred = New-Object System.Management.Automation.PSCredential ($rabbitMqUser, $rabbitMqSecPass)

	$GetAllExchangesUri = [URI] "$rabbitMqApiUrl/exchanges/$rabbitMqVirtualHost"
	FixUri $GetAllExchangesUri
	$exchanges = Invoke-RestMethod -Uri $GetAllExchangesUri -Credential $rabbitMqCred
	

	foreach($exchange in $exchanges)
	{
		
		if ($exchange.name -like '*servicestack*')
		{
			$GetSingleExchangeUri = [URI] "$rabbitMqApiUrl/exchanges/$rabbitMqVirtualHost/$($exchange.name)"
			FixUri $GetSingleExchangeUri
			Write-Host "DELETE $GetSingleExchangeUri" -ForeGroundColor RED
			Invoke-RestMethod -Method DELETE -Uri $GetSingleExchangeUri -Credential $rabbitMqCred | Format-Table -Property name, type
		}
	}
}

function global:publish-project($projectFile, $profile, $username, $password)
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


<#
Updates the CommonAssemblyInfo.cs with the version and application name
#>
function global:create-commonAssemblyInfo($version,$applicationName,$filename)
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