using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers;

public class CarsController : Controller
{
    private readonly AppDbContext _context;

    public CarsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Cars
    public async Task<IActionResult> Index()
    {
        return View(await _context.Cars.ToListAsync());
    }

    // Admin Only - Managing Cars
    public async Task<IActionResult> Manage()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");
        return View(await _context.Cars.ToListAsync());
    }

    // GET: Cars/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
        if (car == null) return NotFound();
        return View(car);
    }

    // GET: Cars/Create
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Car car)
    {
        if (ModelState.IsValid)
        {
            _context.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }
        return View(car);
    }

    // GET: Cars/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");
        if (id == null) return NotFound();

        var car = await _context.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Car car)
    {
        if (id != car.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(car.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Manage));
        }
        return View(car);
    }

    // GET: Cars/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");
        if (id == null) return NotFound();

        var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
        if (car == null) return NotFound();

        return View(car);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null) _context.Cars.Remove(car);
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Manage));
    }

    private bool CarExists(int id)
    {
        return _context.Cars.Any(e => e.Id == id);
    }
}
