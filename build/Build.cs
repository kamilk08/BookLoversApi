using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NUnit;
using Nuke.Common.Tools.VSTest;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

[CheckBuildProjectConfigurations]
[AzurePipelines(AzurePipelinesImage.Windows2019,    
    InvokedTargets = new[]
    {
        nameof(Clean), nameof(Restore), nameof(Compile),
        nameof(ArchitectureTests), nameof(UnitTests),
        nameof(PrepareInputFiles), nameof(PrepareSqlServer),
        nameof(CreateAuthContext), nameof(CreateBookcaseContext),
        nameof(CreatePublicationsContext),nameof(CreateLibrariansContext),
        nameof(CreateRatingsContext),nameof(CreateReadersContext),
        nameof(RunAuthIntegrationTests),nameof(RunLibrariansIntegrationTests),
        nameof(RunRatingsIntegrationTests)
    }
)]
public partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.RunRatingsIntegrationTests);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    public Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            EnsureCleanDirectory(WorkingDirectory);
        });

    public Target Restore => _ => _
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(Solution)
                .SetTargets("Restore"));
        });

    public Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(Solution)
                .SetTargets("Rebuild")
                .SetConfiguration(Configuration)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .SetNodeReuse(IsLocalBuild));
        });

    public Target ArchitectureTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var assemblyNames = Solution.GetProjectsWithTests()
                .GetTestProjectPaths(Configuration);

            VSTestTasks.VSTest(cfg => cfg
                .AddTestCaseFilters("ArchitectureTests")
                .SetTestAssemblies(assemblyNames)
            );
        });

    public Target UnitTests => _ => _
        .DependsOn(ArchitectureTests)
        .Executes(() =>
        {
            var assemblyNames = Solution.GetProjectsWithTests()
                .GetTestProjectPaths(Configuration);

            VSTestTasks.VSTest(cfg => cfg
                .AddTestCaseFilters("UnitTests")
                .SetTestAssemblies(assemblyNames)
            );
        });


    public Target PrepareInputFiles => _ => _
        .After(UnitTests)
        .DependsOn(Clean)
        .Executes(() =>
        {
            string createAuthDatabaseFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateAuthDatabaseScriptName;
            string createAuthStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateAuthStructureScriptName;
            string createBookcaseDatabaseFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateBookcaseDatabaseScriptName;
            string createBookcaseStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateBookcaseStructureScriptName;
            string createBookcaseStoreFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateBookcaseStoreScriptName;
            string createBookcaseStoreStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateBookcaseStoreStructureScriptName;

            string createPublicationsDatabaseFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreatePublicationsDatabaseScriptName;
            string createPublicationsStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreatePublicationsStructureScriptName;
            string createPublicationsStoreFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreatePublicationsStoreScriptName;
            string createPublicationsStoreStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreatePublicationsStoreStructureScriptName;

            string createLibrariansDatabaseFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateLibrariansDatabaseScriptName;
            string createLibrariansStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateLibrariansStructureScriptName;

            string createRatingsDatabaseFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateRatingsDatabaseScriptName;
            string createRatingsStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateRatingsStructureScriptName;

            string createReadersDatabaseFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateReadersDatabaseScriptName;
            string createReadersStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateReadersStructureScriptName;
            string createReadersStoreFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateReadersStoreScriptName;
            string createReadersStoreStructureFileTarget =
                BuildConstants.InputFilesDirectory / BuildConstants.CreateReadersStoreStructureScriptName;

            CopyFile(BuildConstants.CreateAuthDatabasePath, createAuthDatabaseFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateAuthStructurePath, createAuthStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);

            CopyFile(BuildConstants.CreateBookcaseDatabasePath, createBookcaseDatabaseFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateBookcaseStructurePath, createBookcaseStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateBookcaseStorePath, createBookcaseStoreFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateBookcaseStoreStructurePath, createBookcaseStoreStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);

            CopyFile(BuildConstants.CreatePublicationsDatabasePath, createPublicationsDatabaseFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreatePublicationsStructurePath, createPublicationsStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreatePublicationsStorePath, createPublicationsStoreFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreatePublicationsStoreStructurePath, createPublicationsStoreStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);

            CopyFile(BuildConstants.CreateLibrariansDatabasePath, createLibrariansDatabaseFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateLibrariansStructurePath, createLibrariansStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);

            CopyFile(BuildConstants.CreateRatingsDatabasePath, createRatingsDatabaseFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateRatingsStructurePath, createRatingsStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);

            CopyFile(BuildConstants.CreateReadersDatabasePath, createReadersDatabaseFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateReadersStructurePath, createReadersStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateReadersStorePath, createReadersStoreFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
            CopyFile(BuildConstants.CreateReadersStoreStructurePath, createReadersStoreStructureFileTarget,
                FileExistsPolicy.OverwriteIfNewer);
        });


    Target PrepareSqlServer => _ => _
        .DependsOn(PrepareInputFiles)
        .Executes(() =>
        {
            // DockerTasks.DockerStop(s => s.SetContainers("/sql-server-db"));
            DockerTasks.DockerRun(s => s
                .EnableRm()
                .SetName("sql-server-db")
                .SetImage("mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04")
                .SetEnv(
                    $"SA_PASSWORD={BuildConstants.SqlServerPassword}",
                    "ACCEPT_EULA=Y",
                    "MSSQL_RPC_PORT=135",
                    "MSSQL_DTC_TCP_PORT=51000"
                )
                .SetPublish(
                    $"{BuildConstants.SqlServerPort}:1433",
                    $"51433:1433",
                    $"51000:51000"
                )
                .SetMount(
                    $"type=bind,source=\"{BuildConstants.InputFilesDirectory}\",target=/{BuildConstants.InputFilesDirectoryName}")
                .EnableDetach()
            );

            //WAIT FOR DOCKER TO INITIALIZE MS SQL INSTANCE
            Task.Delay(TimeSpan.FromSeconds(30)).GetAwaiter().GetResult();
        });

    Target CreateAuthContext => _ => _
        .DependsOn(PrepareSqlServer)
        .Executes(() =>
        {
            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -S localhost -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateAuthDatabaseScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateAuthStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));
        });

    Target CreateBookcaseContext => _ => _
        .DependsOn(CreateAuthContext)
        .Executes(() =>
        {
            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateBookcaseDatabaseScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateBookcaseStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateBookcaseStoreScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateBookcaseStoreStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));
        });

    Target CreatePublicationsContext => _ => _
        .DependsOn(CreateBookcaseContext)
        .Executes(() =>
        {
            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreatePublicationsDatabaseScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreatePublicationsStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreatePublicationsStoreScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreatePublicationsStoreStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));
        });

    Target CreateLibrariansContext => _ => _
        .DependsOn(CreatePublicationsContext)
        .Executes(() =>
        {
            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -S localhost -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateLibrariansDatabaseScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateLibrariansStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));
        });

    Target CreateRatingsContext => _ => _
        .DependsOn(CreateLibrariansContext)
        .Executes(() =>
        {
            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -S localhost -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateRatingsDatabaseScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateRatingsStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));
        });

    Target CreateReadersContext => _ => _
        .DependsOn(CreateRatingsContext)
        .Executes(() =>
        {
            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateReadersDatabaseScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateReadersStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateReadersStoreScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));

            DockerTasks.DockerExec(s => s
                .EnableInteractive()
                .SetContainer("sql-server-db")
                .SetCommand("/bin/sh")
                .SetArgs("-c",
                    $"./opt/mssql-tools/bin/sqlcmd -d master -i ./{BuildConstants.InputFilesDirectoryName}/{BuildConstants.CreateReadersStoreStructureScriptName} -U {BuildConstants.SqlServerUser} -P {BuildConstants.SqlServerPassword}"));
        });

    Target RunAuthIntegrationTests => _ => _
        .DependsOn(CreateReadersContext)
        .Executes(() =>
        {
            var authTestProject = Solution.GetProject("BookLovers.Auth.Tests");
            var projectDllPath = authTestProject.GetProjectPath(Configuration);
    
            Environment.SetEnvironmentVariable("AuthContextConnectionString",
                BuildConstants.CreateConnectionString("AuthorizationContext"));
    
            VSTestTasks.VSTest(cfg => cfg
                .AddTestCaseFilters("IntegrationTests")
                .SetTestAssemblies(projectDllPath));
        });



    Target RunLibrariansIntegrationTests => _ => _
        .DependsOn(RunAuthIntegrationTests)
        .Executes(() =>
        {
            var librariansTestProject = Solution.GetProject("BookLovers.Librarians.Tests");
            var projectDllPath = librariansTestProject.GetProjectPath(Configuration);

            Environment.SetEnvironmentVariable("LibrariansConnectionString",
                BuildConstants.CreateConnectionString("LibrariansContext"));

            VSTestTasks.VSTest(cfg => cfg
                .AddTestCaseFilters("IntegrationTests")
                .SetTestAssemblies(projectDllPath)
            );
        });

    Target RunRatingsIntegrationTests => _ => _
        .DependsOn(RunLibrariansIntegrationTests)
        .Executes(() =>
        {
            var ratingsTestProject = Solution.GetProject("BookLovers.Ratings.Tests");
            var projectDllPath = ratingsTestProject.GetProjectPath(Configuration);

            Environment.SetEnvironmentVariable("RatingsConnectionString",
                BuildConstants.CreateConnectionString("RatingsContext"));

            VSTestTasks.VSTest(cfg => cfg
                .AddTestCaseFilters("IntegrationTests")
                .SetTestAssemblies(projectDllPath));
        });

    Target StopSqlContainer => _ => _
        .After(RunRatingsIntegrationTests)
        .Executes(() =>
        {
            DockerTasks.DockerStop(s => s.SetContainers("/sql-server-db"));
        });


    //E2E Tests and some integration tests are not working on 
    //windows container due to the fact that 
    //either i am unable to correctly setup ms sql container
    // that allows to perform distributed transactions
    // or such an option is not available on Docker for Windows for now

    // Target RunAuthE2ETests => _ => _
    //     .DependsOn(RunAuthIntegrationTests)
    //     .Executes(() =>
    //     {
    //         var authTestProject = Solution.GetProject("BookLovers.Auth.Tests");
    //         var projectDllPath = authTestProject.GetProjectPath(Configuration);
    //
    //         Environment.SetEnvironmentVariable("AuthContextConnectionString",
    //             BuildConstants.CreateConnectionString("AuthorizationContext"));
    //         Environment.SetEnvironmentVariable("BookcaseConnectionString",
    //             BuildConstants.CreateConnectionString("BookcaseContext"));
    //         Environment.SetEnvironmentVariable("BookcaseStoreConnectionString",
    //             BuildConstants.CreateConnectionString("BookcaseStoreContext"));
    //         Environment.SetEnvironmentVariable("PublicationsConnectionString",
    //             BuildConstants.CreateConnectionString("PublicationsContext"));
    //         Environment.SetEnvironmentVariable("PublicationsStoreConnectionString",
    //             BuildConstants.CreateConnectionString("PublicationsStoreContext"));
    //         Environment.SetEnvironmentVariable("LibrariansConnectionString",
    //             BuildConstants.CreateConnectionString("LibrariansContext"));
    //         Environment.SetEnvironmentVariable("RatingsConnectionString",
    //             BuildConstants.CreateConnectionString("RatingsContext"));
    //         Environment.SetEnvironmentVariable("ReadersConnectionString",
    //             BuildConstants.CreateConnectionString("ReadersContext"));
    //         Environment.SetEnvironmentVariable("ReadersStoreConnectionString",
    //             BuildConstants.CreateConnectionString("ReadersStoreContext"));
    //
    //         VSTestTasks.VSTest(cfg => cfg
    //             .AddTestCaseFilters("EndToEndTests")
    //             .SetTestAssemblies(projectDllPath)
    //         );
    //
    //         DockerTasks.DockerStop(s => s.SetContainers("/sql-server-db"));
    //     });

    // Target RunReadersIntegrationTests => _ => _
    //     .DependsOn(RunRatingsIntegrationTests)
    //     .Executes(() =>
    //     {
    //         var readersTestProject = Solution.GetProject("BookLovers.Readers.Tests");
    //         var projectDllPath = readersTestProject.GetProjectPath(Configuration);
    //
    //         Environment.SetEnvironmentVariable("ReadersConnectionString",
    //             BuildConstants.CreateConnectionString("ReadersContext"));
    //         Environment.SetEnvironmentVariable("ReadersStoreConnectionString",
    //             BuildConstants.CreateConnectionString("ReadersStoreContext"));
    //
    //         VSTestTasks.VSTest(cfg => cfg
    //             .AddTestCaseFilters("IntegrationTests")
    //             .SetTestAssemblies(projectDllPath));
    //     });
    

}