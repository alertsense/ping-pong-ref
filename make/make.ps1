[CmdletBinding()]
Param(
	#[Parameter(Mandatory=$True)]
	#[string]$username,

	#[Parameter(Mandatory=$True)]
	#[Security.SecureString]$password
)

$scriptPath = Split-Path $MyInvocation.InvocationName
Import-Module (join-path $scriptPath "..\src\packages\psake.4.4.1\tools\psake.psm1" -Resolve)

$props = @{}
#$props.publishUserName = $username
#$props.publishPassword = (New-Object System.Management.Automation.PSCredential 'N/A', $password).GetNetworkCredential().Password
$props.rabbitMqApiUrl = "http://192.168.50.181:15672/api"
$props.rabbitMqVirtualHost = "%2Fstaging"
$props.rabbitMqUser = "staging"
$props.rabbitMqPassword = "staging"

$buildFile = join-path $scriptPath "default.ps1" -Resolve

invoke-psake -framework '4.0' -properties $props -taskList Compile, Publish-Raspberry -buildFile $buildFile