#I "packages/FAKE/tools"
#r "packages/FAKE/tools/FakeLib.dll"
#r "System.Xml.Linq"
#r "packages/ConfigJson/lib/ConfigJsonNET.dll"

open System
open System.IO
open Fake.TaskRunnerHelper
open Fake
open Fake.FileUtils
open System.Xml.Linq
open System.Collections.Generic
open Fake.Testing

let buildParam = getBuildParamOrDefault  "buildType" "release" 
// Directories
let baseRootDir="./.build"
let root=baseRootDir+"/build-"+buildParam
let buildDir  = root+"/app/"
let testDir   =root+ "/test"
let deployDir = root+"/deploy/"
let nugetWorkingDir =root+ "/packaging/"
let allPackageFiles = [(buildDir+"OwinDocker.Export.dll")]

let testOutput = FullName "./.build/TestResults"



let nugetDeployPath = getBuildParamOrDefault  "nugetDeployPath" deployDir 

//--------------------------------------------------------------------------------
// Information about the project for Nuget and Assembly info files
//--------------------------------------------------------------------------------

let product = "OwinDocker"
let authors = [ "contactsamie" ]
let copyright = "Copyright © contactsamie 2016"
let company = "contactsamie"
let description = "OwinDocker service"
let tags = ["OwinDocker";"microservice";]
let projectName="OwinDocker.Export"
// Read release notes and version

let BuildFn<'T>= match buildParam with
                  | "debug" -> MSBuildDebug
                  | _       -> MSBuildRelease                  

let BuildVersionType= match buildParam with
                           | "release" -> ""
                           | _         -> "-"+buildParam

let NugetDeployPath= match nugetDeployPath with
                           | "release" -> ""
                           | _         -> "-"+buildParam

// version info
let version =
  match buildServer with
  | TeamCity -> (buildVersion+BuildVersionType)
  | _        -> ("0.0.1"+BuildVersionType)


// Targets
Target "Clean" (fun _ -> 
    CleanDirs [baseRootDir]    
)

Target "RecreateDir" (fun _ ->
    CreateDir baseRootDir
    CreateDir root
    CreateDir deployDir
    CreateDir testDir
    CreateDir buildDir
    CreateDir nugetWorkingDir
    CreateDir testOutput
)

let serviceReferences  =  !! "./OwinDocker/**/*.csproj"

Target "BuildService" (fun _ ->
     MSBuildDebug buildDir "Build" serviceReferences
        |> Log "AppBuild-Output: "
)

let appReferences  =  !! "./OwinDocker.Export/*.csproj"

let testReferences = !! "./OwinDocker.Tests/*.csproj"

Target "BuildExport" (fun _ ->
     BuildFn buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)


Target "BuildTest" (fun _ ->
     BuildFn testDir "Build" testReferences
        |> Log "TestBuild-Output: "
)

Target "xUnitTest" (fun _ ->  
    let xunitTestAssemblies = !! (testDir + "/OwinDocker.Tests.dll")

    let xunitToolPath = findToolInSubPath "xunit.console.exe" "packages/FAKE/xunit.runner.console/tools"
    
    printfn "Using XUnit runner: %s" xunitToolPath
    let runSingleAssembly assembly =
        let assemblyName = Path.GetFileNameWithoutExtension(assembly)
        xUnit2
            (fun p -> { p with XmlOutputPath = Some (testOutput + @"\" + assemblyName + "_xunit_"+buildParam+".xml"); HtmlOutputPath = Some (testOutput + @"\" + assemblyName + "_xunit_"+buildParam+".HTML"); ToolPath = xunitToolPath; TimeOut = System.TimeSpan.FromMinutes 30.0; Parallel = ParallelMode.NoParallelization }) 
            (Seq.singleton assembly)

    xunitTestAssemblies |> Seq.iter (runSingleAssembly)
 
)

Target "CreateNuget" (fun _ ->
    // Copy all the package files into a package folder
    CopyFiles nugetWorkingDir allPackageFiles    

    NuGet (fun p -> 
        { 
          p with
            Authors = authors
            Project = projectName
            Description = description                               
            OutputPath = deployDir
            Summary = description
            WorkingDir = nugetWorkingDir
            Version = version 
         })             
            "OwinDocker.Export.nuspec"
)

Target "Deploy" (fun _ ->
    !! (buildDir + "/**/*.*") 
        -- "*.zip" 
        |> Zip buildDir (deployDir + "OwinDocker." + version + ".zip")
)



Target "RemotePublishNuGet" (fun _ ->     
    !! (deployDir + "*.nupkg") 
      |> Copy NugetDeployPath
)

// Build order
"Clean"
  ==> "RecreateDir"
  ==> "BuildExport"
  ==> "BuildTest"
  ==> "xUnitTest" 
  ==> "BuildService"
  ==> "CreateNuget"
  ==> "RemotePublishNuGet"
  ==> "Deploy"

// start build
RunTargetOrDefault "Deploy"