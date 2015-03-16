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

	$raspberryHost = "192.168.1.15"
	$raspberryUser = "pi"
	$raspberryDirectory = "/home/pi/mono/pingpongref"

    $rabbitMqApiUrl = "http://localhost:15672/api"
	$rabbitMqVirtualHost = "pingpong"
	$rabbitMqUser = ""
	$rabbitMqPassword = ""
}

. ./psake-contrib.ps1

task Default -depends Compile
task Publish -depends Publish-Web, Publish-Raspberry

task Init {

}

task Compile -depends Init, CommonAssemblyInfo {
    msbuild /t:build /v:q /nologo /p:"Configuration=$projectConfig;VisualStudioVersion=12.0" $srcDir\$projectName.sln
}

task Clean {
    msbuild /t:clean /v:q /nologo /p:"Configuration=$projectConfig;VisualStudioVersion=12.0" $srcDir\$projectName.sln
}

task Publish-Web -depends Compile {
	Web-Deploy $srcDir\AlertSense.PingPong\AlertSense.PingPong.csproj $publishProfile $publishUserName $publishPassword
}

task Publish-Raspberry -depends Compile {
	$scpTarget = "{0}@{1}:{2}" -f $raspberryUser, $raspberryHost, $raspberryDirectory
	scp -r -i ~/.ssh/id_rsa "../src/Raspberry/PingPongRef/bin/Debug" $scpTarget
}

task CommonAssemblyInfo {
    create-commonAssemblyInfo "$projectVersion" $projectName "$srcDir\CommonAssemblyInfo.cs"
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
