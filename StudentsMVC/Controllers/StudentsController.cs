using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsMVC.Models;

namespace StudentsMVC.Controllers
{
    // Використання Primary Constructor (C# 12)
    public class StudentsController(StudentContext context) : Controller
    {
        private readonly StudentContext _context = context;

        // GET: Students
        // AsNoTracking() використовується для оптимізації читання (Read-Only)
        public async Task<IActionResult> Index() =>
            View(await _context.Students.AsNoTracking().ToListAsync());

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            // AsNoTracking() значно зменшує навантаження на пам'ять,
            // оскільки EF Core не відстежує сутності
            var student = await _context.Students
                .AsNoTracking() 
                .FirstOrDefaultAsync(m => m.Id == id);

            return student is null ? NotFound() : View(student);
        }

        // GET: Students/Create
        public IActionResult Create() => View();

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Age,GPA")] Student student)
        {
            if (!ModelState.IsValid) return View(student);

            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            return student is null ? NotFound() : View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Age,GPA")] Student student)
        {
            if (id != student.Id) return NotFound();
            if (!ModelState.IsValid) return View(student);

            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                /*
                 * DbUpdateConcurrencyException — це механізм Entity Framework Core для відстеження 
                 * та обробки проблем паралелізму. Він викидається під час виклику SaveChanges(), 
                 * коли фреймворк виявляє, що запис у базі даних, який  треба оновити або видалити, 
                 * змінився іншим процесом з моменту його первинного завантаження.
                 * При розробці багатопотокових додатків або серверних архітектур із великою кількістю 
                 * одночасних запитів цей виняток є основою оптимістичного паралелізму (Optimistic Concurrency). 
                 * Замість жорсткого блокування таблиць під час читання, система дозволяє кільком потокам 
                 * або користувачам читати дані одночасно, але запобігає випадковому перезапису, 
                 * якщо початковий стан уже неактуальний.
                 */
                if (!StudentExists(student.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return student is null ? NotFound() : View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student is not null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id) => _context.Students.Any(e => e.Id == id);
    }
}