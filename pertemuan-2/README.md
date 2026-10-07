# PBKK Pertemuan 2 — Pengenalan .NET dan C#

Nama: Uwais Achmad  
NRP: 5025241103

## Ruang lingkup untuk ditinjau

1. Verifikasi .NET SDK.
2. Hello .NET dan eksperimen biodata (nama, NIM/NRP, prodi, semester, IPK).
3. Sistem data mahasiswa berbasis console: tambah, tampilkan, cari, dan hapus.

Pemetaan tugas ada di [assignment-confirmation.md](assignment-confirmation.md). Tulisan blog bahasa Indonesia ada di [blog-post.html](blog-post.html). Buka HTML di browser untuk melihat penjelasan dan screenshot. **Sudah dipublikasikan:** [PBKK Pertemuan 2 — Pengenalan .NET dan C#](https://wais2005.blogspot.com/2026/10/pbkk-pertemuan-2-pengenalan-net-dan-c.html).

## Menjalankan

Dengan .NET 10 SDK terpasang, dari folder ini:

```bash
dotnet run --project HelloDotNet
dotnet run --project DataMahasiswa
```

Di workspace ini juga tersedia SDK lokal:

```bash
bash assignments/pbkk/pertemuan-2/run.sh HelloDotNet
bash assignments/pbkk/pertemuan-2/run.sh DataMahasiswa
python3 assignments/pbkk/pertemuan-2/verify.py
```

`Program.cs` mengatur input, menu, validasi, dan operasi pada `List<Mahasiswa>`. `Mahasiswa.cs` mendefinisikan data NIM, nama, prodi, dan IPK. Pencarian menerima potongan NIM atau nama tanpa membedakan huruf besar/kecil. Penghapusan membutuhkan konfirmasi. Data hanya disimpan selama program berjalan.

## Dokumentasi

Screenshot dalam `screenshots/` diambil dari proses .NET yang berjalan di terminal Xterm pada Linux menggunakan layar virtual Xvfb. Input demonstrasi dimasukkan otomatis ke terminal aplikasi. Gambar bukan mockup. Nama Bima Pratama, NIM 5025241999, semester 5, dan IPK 3.75 adalah data contoh.

`verify.py` menguji alur normal serta input salah, NIM duplikat, pencarian, pembatalan hapus, penghapusan, dan akhir input. Tidak membutuhkan paket Python tambahan.

File ZIP berisi kode, dokumentasi, dan screenshot; tidak menyertakan SDK lokal atau artefak build. Artikel publik menggunakan gambar PNG tertanam dan kode lengkap dalam bagian Source Code. `blogger-publish.html` adalah isi yang dipublikasikan; `blogger-post.json` menyimpan metadata dan ID untuk pembaruan.
