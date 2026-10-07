namespace StudentManagement.Core;

public sealed record Student(int Id, string Nim, string Nama, string Jurusan, string Gender, string Email);
public sealed record StudentStats(int Total, int Informatika, int SistemInformasi, int LakiLaki, int Perempuan);
