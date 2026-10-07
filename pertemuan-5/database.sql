-- Jalankan di SQL Server Management Studio atau sqlcmd sebelum membuka aplikasi.
IF DB_ID(N'PBKKStudents') IS NULL CREATE DATABASE PBKKStudents;
GO
USE PBKKStudents;
GO
IF OBJECT_ID('dbo.Students', 'U') IS NULL
CREATE TABLE dbo.Students (
    Id INT IDENTITY PRIMARY KEY,
    Nim NVARCHAR(12) NOT NULL UNIQUE,
    Nama NVARCHAR(100) NOT NULL,
    Jurusan NVARCHAR(50) NOT NULL,
    Gender NVARCHAR(20) NOT NULL,
    Email NVARCHAR(150) NOT NULL
);
GO
