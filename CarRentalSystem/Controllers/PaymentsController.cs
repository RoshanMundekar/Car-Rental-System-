using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers;

public class PaymentsController : Controller
{
    private readonly AppDbContext _context;

    public PaymentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Payments/Process?bookingId=5
    public async Task<IActionResult> Process(int bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Car)
            .FirstOrDefaultAsync(m => m.Id == bookingId);

        if (booking == null) return NotFound();
        if (booking.Status != "Pending") return RedirectToAction("MyBookings", "Bookings");

        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessPost(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return NotFound();

        // Create Payment record
        var payment = new Payment
        {
            BookingId = bookingId,
            Amount = booking.TotalAmount,
            PaymentDate = DateTime.Now,
            Status = "Paid" // Mocking successful payment
        };

        _context.Payments.Add(payment);

        // Update Booking status
        booking.Status = "Confirmed";
        _context.Update(booking);

        await _context.SaveChangesAsync();

        TempData["Success"] = "Payment processed successfully!";
        return RedirectToAction("MyBookings", "Bookings");
    }
}
