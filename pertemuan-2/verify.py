"""Uji alur console dengan input pengguna, tanpa dependensi tambahan."""
from pathlib import Path
import subprocess
root = Path(__file__).resolve().parent

def run(project, inputs):
    result = subprocess.run(['bash', str(root/'run.sh'), project], input='\n'.join(inputs)+'\n', text=True, capture_output=True, timeout=60)
    assert result.returncode == 0, result.stderr
    return result.stdout

hello = run('HelloDotNet', ['Uwais Achmad', '5025241103', 'Teknik Informatika', '0', '3', '5', '3,75'])
assert 'Semester : 3' in hello and 'IPK      : 3.75' in hello
result = run('DataMahasiswa', [
 '2', '1', 'abc', '1', '5025241103', '', 'Uwais Achmad', 'Teknik Informatika', 'abc', '4.1', '3,75',
 '1', '5025241103', '2', '3', 'uWAis', '3', 'tidakada',
 '4', '000', '4', '5025241103', 't', '2', '4', '5025241103', 'y', '2', '9', '0'])
for expected in ['NIM/NRP harus berupa angka.', 'Isian tidak boleh kosong.', 'IPK tidak valid.',
 'NIM/NRP sudah terdaftar.', 'Jumlah mahasiswa: 1', 'Mahasiswa tidak ditemukan.',
 'Penghapusan dibatalkan.', 'Data mahasiswa berhasil dihapus.', 'Menu tidak tersedia.']:
 assert expected in result, expected
assert result.count('Tidak ada data mahasiswa.') == 3
assert run('DataMahasiswa', []).endswith('Program selesai. Terima kasih!\n')
print('LULUS: biodata, IPK koma/titik, batas semester/IPK, isian kosong, NIM numerik/duplikat, daftar, pencarian, hapus/batal, menu salah, dan akhir input.')
