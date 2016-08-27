@echo off
cls
packages\NuGet.CommandLine.3.4.3\tools\NuGet.exe Install FAKE -OutputDirectory packages -ExcludeVersion
packages\NuGet.CommandLine.3.4.3\tools\NuGet.exe Install ConfigJson -OutputDirectory packages -ExcludeVersion
packages\NuGet.CommandLine.3.4.3\tools\NuGet.exe install xunit.runner.console -OutputDirectory packages\FAKE -ExcludeVersion -Version 2.1.0
packages\FAKE\tools\Fake.exe build.fsx buildType=%1 nugetDeployPath=%2
pause