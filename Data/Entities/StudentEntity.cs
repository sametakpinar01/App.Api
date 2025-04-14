using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Api.Data.Entities
{
    public class StudentEntity
    {
        // Öğrenci kimlik numarası (birincil anahtar)
        // Veritabanında otomatik olarak artan bir değer olarak tanımlanır
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Öğrencinin adı (zorunlu alan, maksimum 50 karakter)
        // Varsayılan değer olarak boş bir string atanır
        [Required, StringLength(50, MinimumLength = 1, ErrorMessage = "Ad 1 ile 50 karakter arasında olmalıdır.")]
        public string FirstName { get; set; } = string.Empty;

        // Öğrencinin soyadı (zorunlu alan, maksimum 50 karakter)
        // Varsayılan değer olarak boş bir string atanır
        [Required, StringLength(50, MinimumLength = 1, ErrorMessage = "Soyad 1 ile 50 karakter arasında olmalıdır.")]
        public string LastName { get; set; } = string.Empty;

        // Öğrenci numarası (pozitif bir değer olmalı)
        // Negatif veya sıfır değerlerin girilmesini engellemek için doğrulama eklenmiştir
        [Range(1, int.MaxValue, ErrorMessage = "Öğrenci numarası pozitif bir değer olmalıdır.")]
        public int No { get; set; }

        // Öğrencinin sınıfı (zorunlu alan, maksimum 10 karakter)
        // Maksimum uzunluk sınırı eklenmiştir
        [Required, StringLength(10, ErrorMessage = "Sınıf bilgisi en fazla 10 karakter olabilir.")]
        public string Class { get; set; } = string.Empty;
    }
}
