using System.Data;
using Microsoft.Data.SqlClient;

namespace StudentManagement.Core;

public sealed class StudentRepository(string connectionString)
{
    private SqlConnection Open()
    {
        var connection = new SqlConnection(connectionString);
        try { connection.Open(); return connection; }
        catch { connection.Dispose(); throw; }
    }

    public void Initialize()
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            IF OBJECT_ID('dbo.Students', 'U') IS NULL
            CREATE TABLE dbo.Students (
                Id INT IDENTITY PRIMARY KEY,
                Nim NVARCHAR(12) NOT NULL UNIQUE,
                Nama NVARCHAR(100) NOT NULL,
                Jurusan NVARCHAR(50) NOT NULL,
                Gender NVARCHAR(20) NOT NULL,
                Email NVARCHAR(150) NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }

    public List<Student> Search(string search)
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, Nim, Nama, Jurusan, Gender, Email FROM dbo.Students
            WHERE Nim LIKE @search ESCAPE '~' OR Nama LIKE @search ESCAPE '~'
               OR Jurusan LIKE @search ESCAPE '~' ORDER BY Nama, Id;
            """;
        var pattern = search.Replace("~", "~~").Replace("%", "~%").Replace("_", "~_").Replace("[", "~[");
        command.Parameters.Add("@search", SqlDbType.NVarChar, 210).Value = "%" + pattern + "%";
        using var reader = command.ExecuteReader();
        var result = new List<Student>();
        while (reader.Read())
            result.Add(new Student(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)));
        return result;
    }

    public void Save(Student student)
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText = student.Id == 0
            ? "INSERT INTO dbo.Students (Nim,Nama,Jurusan,Gender,Email) VALUES (@nim,@nama,@jurusan,@gender,@email);"
            : "UPDATE dbo.Students SET Nim=@nim,Nama=@nama,Jurusan=@jurusan,Gender=@gender,Email=@email WHERE Id=@id;";
        command.Parameters.Add("@id", SqlDbType.Int).Value = student.Id;
        command.Parameters.Add("@nim", SqlDbType.NVarChar, 12).Value = student.Nim;
        command.Parameters.Add("@nama", SqlDbType.NVarChar, 100).Value = student.Nama;
        command.Parameters.Add("@jurusan", SqlDbType.NVarChar, 50).Value = student.Jurusan;
        command.Parameters.Add("@gender", SqlDbType.NVarChar, 20).Value = student.Gender;
        command.Parameters.Add("@email", SqlDbType.NVarChar, 150).Value = student.Email;
        if (command.ExecuteNonQuery() != 1) throw new InvalidOperationException("Data tidak ditemukan. Muat ulang daftar.");
    }

    public void Delete(int id)
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM dbo.Students WHERE Id=@id;";
        command.Parameters.Add("@id", SqlDbType.Int).Value = id;
        if (command.ExecuteNonQuery() != 1) throw new InvalidOperationException("Data tidak ditemukan. Muat ulang daftar.");
    }

    public StudentStats GetStats()
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT COUNT(*),
                COALESCE(SUM(CASE WHEN Jurusan=N'Informatika' THEN 1 ELSE 0 END),0),
                COALESCE(SUM(CASE WHEN Jurusan=N'Sistem Informasi' THEN 1 ELSE 0 END),0),
                COALESCE(SUM(CASE WHEN Gender=N'Laki-laki' THEN 1 ELSE 0 END),0),
                COALESCE(SUM(CASE WHEN Gender=N'Perempuan' THEN 1 ELSE 0 END),0)
            FROM dbo.Students;
            """;
        using var reader = command.ExecuteReader();
        reader.Read();
        return new StudentStats(reader.GetInt32(0),reader.GetInt32(1),reader.GetInt32(2),reader.GetInt32(3),reader.GetInt32(4));
    }
}
