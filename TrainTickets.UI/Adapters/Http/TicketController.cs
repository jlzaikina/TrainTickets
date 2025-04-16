using Microsoft.AspNetCore.Mvc;
using TrainTickets.UI.Application.Test.Handlers;
namespace TrainTickets.UI.Adapters.Http;

using Org.BouncyCastle.Asn1.Ocsp;
using TrainTickets.UI.Domain.Ticket;

public class TicketController : ControllerBase
{
    private readonly ITicketHandler _ticketHandler;

    public TicketController(ITicketHandler ticketHandler)
    {
        _ticketHandler = ticketHandler ?? throw new ArgumentNullException(nameof(ticketHandler));
    }

    /// <summary>
    /// Получить расписание
    /// </summary>
    /// <returns><see cref="BookDto"/></returns>
    [HttpGet]
    [Route("/api/v1/ticket/get-all/{login}")]
    public async Task<IEnumerable<BookDto>> GetBook(string login)
    {
        return await _ticketHandler.GetBookAsync(login);
    }
    [HttpGet]
    [Route("/api/v1/ticket/get-ticket/{id}/pdf")]
    public async Task<IActionResult> GetTicketPdf(int id)
    {
        var ticket = await _ticketHandler.GetTicketByIdAsync(id);
        if (ticket == null) return NotFound();

        var pdfBytes = _ticketHandler.GenerateTicketPdf(ticket);
        return File(pdfBytes, "application/pdf", $"{ticket.Passenger_name.Split(' ')[0]}{id}.pdf");
    }
    [HttpPost]
    [Route("/api/v1/ticket/delete-ticket/{id}/{login}")]
    public async Task<ActionResult<bool>> DeleteTicket(int id, string login)
    {
        try
        {
            var result = await _ticketHandler.DeleteTicketAsync(id, login);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ошибка удаления");
        }
    }
}
