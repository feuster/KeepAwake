using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Common.Tools.DotNet;
using Cake.Core;
using Cake.Frosting;
using System.IO;
using System.Reflection;

//disable package related warnings since the cake builder will be disposed after build and never be released
#pragma warning disable NU1903, NU1904

/// <summary>
/// Init cake builder
/// </summary>
public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

/// <summary>
/// Cake builder configuration
/// </summary>
public class BuildContext : FrostingContext
{
    public string DotNetPublishConfiguration { get; set; }
    public string Framework { get; set; }
    public string Runtime { get; set; }
    public bool SelfContained { get; set; }

    public string PublishDir { get; set; }
    public string PublishExeName { get; set; }
    public BuildContext(ICakeContext context)
        : base(context)
    {
        DotNetPublishConfiguration  = context.Argument("configuration", "Release");
        Framework                   = context.Argument("framework", "net8.0-windows");
        Runtime                     = context.Argument("runtime", "win-x64");
        SelfContained               = context.Argument("selfcontained", false);
        PublishDir                  = context.Argument("publishdir", "../Release");
        PublishExeName              = context.Argument("publishexename", "KeepAwake");
    }
}

/// <summary>
/// Clean release folder from previous builds
/// </summary>
[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.CleanDirectory(context.PublishDir);
    }
}

/// <summary>
/// Cake builder dotnet publish
/// </summary>
[TaskName("Publish")]
[IsDependentOn(typeof(CleanTask))]
public sealed class PublishTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        if (File.Exists(context.PublishDir + "/" + context.PublishExeName + ".exe"))
            context.DeleteFile(context.PublishDir + "/" + context.PublishExeName + ".exe");

        context.DotNetPublish($"../KeepAwake.sln", new DotNetPublishSettings
        {
            Configuration = context.DotNetPublishConfiguration,
            WorkingDirectory = $"./",
            Framework = context.Framework,
            Runtime = context.Runtime,
            SelfContained = context.SelfContained,
            PublishSingleFile = true,
            PublishTrimmed = false,
            PublishReadyToRun = false,
            EnableCompressionInSingleFile = false,
            IncludeNativeLibrariesForSelfExtract = true,
            IncludeAllContentForSelfExtract = true,
            ArgumentCustomization = args => args.Append("/p:PublishDir=\""+context.PublishDir+"\"")
                                                .Append("/p:DebugType=None")
                                                .Append("/p:DebugSymbols=false")
                                                .Append("/p:PublishAoT=false")
                                                .Append("/p:TrimMode=partial")
                                                .Append("/p:Platform=\"Any CPU\"")
                                                .Append("/p:AssemblyName=\"" + context.PublishExeName+"\"")
        });
    }
}

/// <summary>
/// Default cake builder task which starts the building process
/// </summary>
[TaskName("Default")]
[IsDependentOn(typeof(PublishTask))]
public class DefaultTask : FrostingTask
{
}