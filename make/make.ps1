[CmdletBinding()]
Param(

)

$scriptPath = Split-Path $MyInvocation.InvocationName
Import-Module (join-path $scriptPath "..\src\packages\psake.4.4.1\tools\psake.psm1" -Resolve)

$props = @{}

$buildFile = join-path $scriptPath "default.ps1" -Resolve

invoke-psake -framework '4.0' -properties $props -taskList Publish-Raspberry -buildFile $buildFile