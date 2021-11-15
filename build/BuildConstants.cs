using Nuke.Common.IO;

public class BuildConstants : Build
{
    public static AbsolutePath WorkingDirectory => RootDirectory / ".nuke-working-directory";

    public static AbsolutePath InputFilesDirectory => WorkingDirectory / "input-files";

    public static AbsolutePath AuthPath => RootDirectory / "BookLovers.Auth.Infrastructure";
    public static AbsolutePath BookcasesPath => RootDirectory / "BookLovers.Bookcases.Infrastructure";
    public static AbsolutePath PublicationsPath => RootDirectory / "BookLovers.Publication.Infrastructure";
    public static AbsolutePath LibrariansPath => RootDirectory / "BookLovers.Librarians.Infrastructure";
    public static AbsolutePath RatingsPath => RootDirectory / "BookLovers.Ratings.Infrastructure";
    public static AbsolutePath ReadersPath => RootDirectory / "BookLovers.Readers.Infrastructure";
    
    public static AbsolutePath CreateAuthDatabasePath =>
        AuthPath / "Persistence" / "Scripts" / CreateAuthDatabaseScriptName;

    public static AbsolutePath CreateAuthStructurePath =>
        AuthPath / "Persistence" / "Scripts" / CreateAuthStructureScriptName;

    public static AbsolutePath CreateBookcaseDatabasePath =>
        BookcasesPath / "Persistence" / "Scripts" / CreateBookcaseDatabaseScriptName;

    public static AbsolutePath CreateBookcaseStructurePath =>
        BookcasesPath / "Persistence" / "Scripts" / CreateBookcaseStructureScriptName;

    public static AbsolutePath CreateBookcaseStorePath =>
        BookcasesPath / "Persistence" / "Scripts" / CreateBookcaseStoreScriptName;

    public static AbsolutePath CreateBookcaseStoreStructurePath =>
        BookcasesPath / "Persistence" / "Scripts" / CreateBookcaseStoreStructureScriptName;

    public static AbsolutePath CreatePublicationsDatabasePath =>
        PublicationsPath / "Persistence" / "Scripts" / CreatePublicationsDatabaseScriptName;

    public static AbsolutePath CreatePublicationsStructurePath =>
        PublicationsPath / "Persistence" / "Scripts" / CreatePublicationsStructureScriptName;

    public static AbsolutePath CreatePublicationsStorePath =>
        PublicationsPath / "Persistence" / "Scripts" / CreatePublicationsStoreScriptName;

    public static AbsolutePath CreatePublicationsStoreStructurePath =>
        PublicationsPath / "Persistence" / "Scripts" / CreatePublicationsStoreStructureScriptName;

    public static AbsolutePath CreateLibrariansDatabasePath =>
        LibrariansPath / "Persistence" / "Scripts" / CreateLibrariansDatabaseScriptName;

    public static AbsolutePath CreateLibrariansStructurePath =>
        LibrariansPath / "Persistence" / "Scripts" / CreateLibrariansStructureScriptName;

    public static AbsolutePath CreateRatingsDatabasePath =>
        RatingsPath / "Persistence" / "Scripts" / CreateRatingsDatabaseScriptName;

    public static AbsolutePath CreateRatingsStructurePath =>
        RatingsPath / "Persistence" / "Scripts" / CreateRatingsStructureScriptName;

    public static AbsolutePath CreateReadersDatabasePath =>
        ReadersPath / "Persistence" / "Scripts" / CreateReadersDatabaseScriptName;

    public static AbsolutePath CreateReadersStructurePath =>
        ReadersPath / "Persistence" / "Scripts" / CreateReadersStructureScriptName;

    public static AbsolutePath CreateReadersStorePath =>
        ReadersPath / "Persistence" / "Scripts" / CreateReadersStoreScriptName;

    public static AbsolutePath CreateReadersStoreStructurePath =>
        ReadersPath / "Persistence" / "Scripts" / CreateReadersStoreStructureScriptName;

    public const string CreateAuthDatabaseScriptName = "CreateAuthDatabase.sql";
    public const string CreateAuthStructureScriptName = "CreateAuthStructure.sql";
    public const string CreateBookcaseDatabaseScriptName = "CreateBookcaseDatabase.sql";
    public const string CreateBookcaseStructureScriptName = "CreateBookcaseStructure.sql";
    public const string CreateBookcaseStoreScriptName = "CreateBookcaseStore.sql";
    public const string CreateBookcaseStoreStructureScriptName = "CreateBookcaseStoreStructure.sql";
    public const string CreatePublicationsDatabaseScriptName = "CreatePublicationsDatabase.sql";
    public const string CreatePublicationsStructureScriptName = "CreatePublicationsStructure.sql";
    public const string CreatePublicationsStoreScriptName = "CreatePublicationsStore.sql";
    public const string CreatePublicationsStoreStructureScriptName = "CreatePublicationsStoreStructure.sql";
    public const string CreateLibrariansDatabaseScriptName = "CreateLibrariansDatabase.sql";
    public const string CreateLibrariansStructureScriptName = "CreateLibrariansStructure.sql";
    public const string CreateRatingsDatabaseScriptName = "CreateRatingsDatabase.sql";
    public const string CreateRatingsStructureScriptName = "CreateRatingsStructure.sql";
    public const string CreateReadersDatabaseScriptName = "CreateReadersDatabase.sql";
    public const string CreateReadersStructureScriptName = "CreateReadersStructure.sql";
    public const string CreateReadersStoreScriptName = "CreateReadersStore.sql";
    public const string CreateReadersStoreStructureScriptName = "CreateReadersStoreStructure.sql";
    
    public const string InputFilesDirectoryName = "input-files";

    public const string SqlServerPassword = "123qwe!@#QWE";

    public const string SqlServerUser = "sa";

    public const string SqlServerPort = "1401";

    public static string CreateConnectionString(string databaseName)
    {
        return $@"Server=localhost,1401; Database={databaseName}; User=sa; Password ={BuildConstants.SqlServerPassword};";
    }


}