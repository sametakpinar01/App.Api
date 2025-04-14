using App.Api.Data;
using App.Api.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor üzerinden AppDbContext bağımlılığı (dependency injection) yapılır
        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Student
        // Tüm öğrencileri getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentEntity>>> GetAll()
        {
            try
            {
                // Performans için AsNoTracking kullanılır
                return await _context.Students.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                // Hata durumunda 500 döner
                return StatusCode(StatusCodes.Status500InternalServerError, $"Bir hata oluştu: {ex.Message}");
            }
        }

        // GET: api/Student/5
        // Belirli bir ID'ye sahip öğrenciyi getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentEntity>> GetById(int id)
        {
            try
            {
                var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                if (student == null)
                {
                    return NotFound("Öğrenci bulunamadı."); // Öğrenci bulunamazsa 404 döner
                }
                return student;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Bir hata oluştu: {ex.Message}");
            }
        }

        // POST: api/Student
        // Yeni bir öğrenci oluşturur
        [HttpPost]
        public async Task<ActionResult<StudentEntity>> Create([FromBody] StudentEntity student)
        {
            try
            {
                // Model doğrulama
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Students.Add(student); // Yeni öğrenci eklenir
                await _context.SaveChangesAsync(); // Değişiklikler veritabanına kaydedilir

                // Oluşturulan öğrenci bilgisiyle birlikte 201 döner
                return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Bir hata oluştu: {ex.Message}");
            }
        }

        // PUT: api/Student/5
        // Belirli bir ID'ye sahip öğrenciyi günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentEntity student)
        {
            try
            {
                // Model doğrulama
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != student.Id)
                {
                    return BadRequest("ID'ler eşleşmiyor."); // ID'ler eşleşmezse 400 döner
                }

                _context.Entry(student).State = EntityState.Modified; // Öğrenci bilgisi güncellenir

                await _context.SaveChangesAsync(); // Değişiklikler veritabanına kaydedilir
                return NoContent(); // Başarılı güncelleme sonrası 204 döner
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExists(id))
                {
                    return NotFound("Öğrenci bulunamadı."); // Öğrenci bulunamazsa 404 döner
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Bir hata oluştu: {ex.Message}");
            }
        }

        // DELETE: api/Student/5
        // Belirli bir ID'ye sahip öğrenciyi siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound("Öğrenci bulunamadı."); // Öğrenci bulunamazsa 404 döner
                }

                _context.Students.Remove(student); // Öğrenci silinir
                await _context.SaveChangesAsync(); // Değişiklikler veritabanına kaydedilir

                return NoContent(); // Başarılı silme sonrası 204 döner
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Bir hata oluştu: {ex.Message}");
            }
        }

        // Belirli bir ID'ye sahip öğrencinin var olup olmadığını kontrol eder
        private async Task<bool> StudentExists(int id)
        {
            return await _context.Students.AnyAsync(e => e.Id == id);
        }
    }
}
