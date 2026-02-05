using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers;

public class BookingsController : Controller
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: My Bookings
    public async Task<IActionResult> MyBookings()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");

        var userId = int.Parse(userIdStr);
        var bookings = await _context.Bookings
            .Include(b => b.Car)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.StartDate)
            .ToListAsync();

        return View(bookings);
    }

    // GET: Create Booking
    public async Task<IActionResult> Create(int carId)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"))) 
            return RedirectToAction("Login", "Account");

        var car = await _context.Cars.FindAsync(carId);
        if (car == null || !car.IsAvailable) return RedirectToAction("Index", "Cars");

        ViewBag.Car = car;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking booking)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");

        var car = await _context.Cars.FindAsync(booking.CarId);
        if (car == null) return NotFound();

        if (booking.EndDate <= booking.StartDate)
        {
            ModelState.AddModelError("", "End date must be after start date.");
            ViewBag.Car = car;
            return View(booking);
        }

        int days = (booking.EndDate - booking.StartDate).Days;
        if (days == 0) days = 1;

        booking.UserId = int.Parse(userIdStr);
        booking.TotalAmount = days * car.PricePerDay;
        booking.Status = "Pending";

        _context.Add(booking);
        
        // Mark car as unavailable
        car.IsAvailable = false;
        _context.Update(car);

        await _context.SaveChangesAsync();

        return RedirectToAction("Confirmation", new { id = booking.Id });
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Car)
            .Include(b => b.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (booking == null) return NotFound();
        return View(booking);
    }
}
