using App.Api.Data.Entities;
using Bogus;

namespace App.Api.Data
{
    public class BogusFakeData
    {
        // Öğrenci verilerini sahte olarak oluşturup veritabanına ekleyen metot
        public static void SeedStudents(AppDbContext context, int count = 20)
        {
            // Veritabanı bağlantısının kontrol edilmesi
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Veritabanı bağlantısı sağlanamadı.");
            }

            // Faker kütüphanesi kullanılarak sahte öğrenci verileri oluşturulur
            var faker = new Faker<StudentEntity>()
                .RuleFor(s => s.FirstName, f => f.Name.FirstName()) // Öğrencinin adı
                .RuleFor(s => s.LastName, f => f.Name.LastName()) // Öğrencinin soyadı
                .RuleFor(s => s.No, f => f.Random.Int(1000, 9999)) // Öğrencinin numarası
                .RuleFor(s => s.Class, f => $"{f.Random.ArrayElement(new[] { "A", "B", "C", "D", "E" })}{f.Random.Int(1, 12)}"); // Öğrencinin sınıfı

            // Öğrenci numaralarının benzersiz olmasını sağlamak için bir set kullanılır
            var uniqueNumbers = new HashSet<int>();
            var students = new List<StudentEntity>();

            for (int i = 0; i < count; i++)
            {
                var student = faker.Generate();

                // Benzersiz öğrenci numarası kontrolü
                while (!uniqueNumbers.Add(student.No))
                {
                    student.No = new Random().Next(1000, 9999);
                }

                // Sınıf bilgisinin doğrulanması
                if (student.Class.Length > 10)
                {
                    student.Class = student.Class.Substring(0, 10); // Maksimum 10 karaktere kesilir
                }

                students.Add(student);
            }

            // Oluşturulan öğrenciler veritabanına eklenir
            context.Students.AddRange(students);
            context.SaveChanges(); // Değişiklikler veritabanına kaydedilir
        }
    }
}
