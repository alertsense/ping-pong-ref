Framework "4.0"

properties {
    $projectName = "AlertSense.PingPong"
    $projectConfig = "Debug"
    $projectVersion = "1.0.0"

    $baseDir = Resolve-Path ..\
    $srcDir = join-path $baseDir "\src"

    $raspberryHost = "pingpong.pi"
    $raspberryUser = "pi"
    $raspberryDirectory = "/home/pi/mono/pingpong"
}

. ./psake-contrib.ps1

task Default -depends Compile
task Publish -depends Publish-Web, Publish-Raspberry

task Compile {
    msbuild /t:build /v:q /nologo /p:"Configuration=$projectConfig;VisualStudioVersion=12.0" $srcDir\$projectName.sln
}

task Clean {
    msbuild /t:clean /v:q /nologo /p:"Configuration=$projectConfig;VisualStudioVersion=12.0" $srcDir\$projectName.sln
}

task Publish-Raspberry -depends Compile {
	$scpTarget = "{0}@{1}:{2}" -f $raspberryUser, $raspberryHost, $raspberryDirectory
	scp -r -i ~/.ssh/id_rsa "../src/Raspberry/PingPongRef/bin/Debug/*" $scpTarget
}