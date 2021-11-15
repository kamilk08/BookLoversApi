using System.Collections.Generic;
using Nuke.Common.ProjectModel;

public static class BuildExtensions
{
    public static IEnumerable<Project> GetProjectsWithTests(this Solution solution)
    {
        return solution.GetProjects("*Tests*");
    }

    public static string GetProjectPath(this Project project, Configuration configuration)
    {
        var outDir = project.GetMSBuildProject(configuration)
            .GetPropertyValue("OutputPath");

        return @$"{project.Directory}\{outDir}{project.Name}.dll";
    }

    public static IEnumerable<string> GetTestProjectPaths(this IEnumerable<Project> projects,
        Configuration configuration)
    {
        var assemblyNames = new List<string>();

        foreach (var item in projects)
        {
            var outDir = item.GetMSBuildProject(configuration)
                .GetPropertyValue("OutputPath");
            var assembly = @$"{item.Directory}\{outDir}{item.Name}.dll";
            assemblyNames.Add(assembly);
        }

        return assemblyNames;
    }
}