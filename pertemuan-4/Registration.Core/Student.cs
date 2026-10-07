namespace Registration.Core;

public record Student(string Nim, string Nama, string Prodi, string Gender)
{
    public override string ToString() => $"{Nim} | {Nama} | {Prodi} | {Gender}";
}
