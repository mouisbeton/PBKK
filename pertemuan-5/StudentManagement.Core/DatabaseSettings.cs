namespace StudentManagement.Core;

public static class DatabaseSettings
{
    public static string ConnectionString => Environment.GetEnvironmentVariable("PBKK_SQL_CONNECTION")
        ?? @"Server=(localdb)\MSSQLLocalDB;Database=PBKKStudents;Integrated Security=true;TrustServerCertificate=true;Connect Timeout=5;";
}
