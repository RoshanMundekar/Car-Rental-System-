using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Dashboard()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

        var bookings = await _context.Bookings
            .Include(b => b.Car)
            .Include(b => b.User)
            .OrderByDescending(b => b.Id)
            .ToListAsync();

        var payments = await _context.Payments
            .Include(p => p.Booking!)
            .ThenInclude(b => b.Car)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        ViewBag.TotalBookings = bookings.Count;
        ViewBag.TotalRevenue = payments.Sum(p => p.Amount);
        ViewBag.RecentPayments = payments.Take(5).ToList();

        return View(bookings);
    }
}
