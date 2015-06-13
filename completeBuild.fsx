// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile


RestorePackages()

// Directories
let buildDir  = @".\build\"
let testDir   = @".\test\"
let deployDir = @".\deploy\"
let packagesDir = @".\packages"


// Project info
let authors = ["Andrew O'Connor";"Andrea Magnorsky"]
let projectName = "Discipline Oak"
type ProjectInfo = { 
    Name: string;
    Description: string; 
    Version: string;
  }
let info = {
  Name="Discipline Oak";
  Description =  "Discipline Oak, behaviour tree library";
  Version = if isLocalBuild then "0.1-local" else "0.2"+buildVersion
}

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir; deployDir]
)

Target "SetVersions" (fun _ ->
    CreateCSharpAssemblyInfo "./DisciplineOak/Properties/AssemblyInfo.cs"
        [Attribute.Title info.Name
         Attribute.Description info.Description
         Attribute.Guid "c1dcbc84-7e8b-46f3-a253-9d9527434dee"         
         Attribute.Version info.Version
         Attribute.FileVersion info.Version]
)


Target "Compile" (fun _ ->
    !! @"**\*.csproj"
      |> MSBuildRelease "" "Build"      
      |> Log "AppBuild-Output: "
)

Target "CompileTest" (fun _ ->
    !! @"**\*Tests.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: "
)

Target "NUnitTest" (fun _ ->
    !! (testDir + @"\*Tests.dll")
      |> NUnit (fun p ->
                 {p with
                   DisableShadowCopy = true;
                   OutputFile = testDir + @"TestResults.xml"})
)

Target "CreatePackage" (fun _ ->    
    let nugetPath = ".nuget/NuGet.exe"
    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName            
            Description = info.Description                                           
            OutputPath = deployDir            
            ToolPath = nugetPath
            Summary = info.Description            
            Tags = "behaviour-tree behavior-tree AI"           
            PublishUrl = getBuildParamOrDefault "nugetrepo" ""
            AccessKey = getBuildParamOrDefault "nugetkey" ""            
            Publish = hasBuildParam "nugetkey"  
            }) 
            "nuget/DisciplineOak.nuspec"
)

Target "AndroidPack" (fun _ ->    
    let nugetPath = ".nuget/NuGet.exe"
    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName+"Android"
            Description = info.Description                                           
            OutputPath = deployDir            
            ToolPath = nugetPath
            Summary = info.Description            
            Tags = "behaviour-tree behavior-tree AI"           
            PublishUrl = getBuildParamOrDefault "nugetrepo" ""
            AccessKey = getBuildParamOrDefault "nugetkey" ""            
            Publish = hasBuildParam "nugetkey"  
            }) 
            "nuget/DisciplineOak.Android.nuspec"
)


// Dependencies
"Clean"
  ==> "SetVersions"
  ==> "Compile"
  ==> "CompileTest"
  ==> "NUnitTest"
  ==> "CreatePackage"

"Clean"
  ==> "SetVersions"
  ==> "Compile"
  ==> "AndroidPack"

// start build
RunTargetOrDefault "CreatePackage"