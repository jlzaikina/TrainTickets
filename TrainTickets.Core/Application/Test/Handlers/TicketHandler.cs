
using Org.BouncyCastle.Asn1.Ocsp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

public class TicketHandler: ITicketHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITicketMapper _ticketMapper;

    public TicketHandler(ITicketRepository ticketRepository, ITicketMapper ticketMapper, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
        _ticketMapper = ticketMapper ?? throw new ArgumentNullException(nameof(ticketMapper));
        _userRepository = userRepository ?? throw new ArgumentNullException( nameof(userRepository));
    }

    public async Task<bool> DeleteTicketAsync(int id, string login)
    {
        await _ticketRepository.DeleteTicket(id, login);
        return true;
    }

    public byte[] GenerateTicketPdf(TicketDto ticket)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        string passportInfo = !string.IsNullOrEmpty(ticket.Passenger_passport)
            ? $"паспорт {ticket.Passenger_passport[..4]} {ticket.Passenger_passport[4..]}"
            : "ребёнок";
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A7);
                page.Margin(1, Unit.Centimetre);

                page.Content().Column(column =>
                {
                    // Заголовок
                    column.Item().Text($"Билет №{ticket.Id_ticket}")
                        .Bold().FontSize(10).AlignLeft();

                    // Основная информация
                    column.Item().PaddingVertical(8).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Поезд").Bold().FontSize(11);
                            col.Item().Text(text => text.Span(ticket.Train_number.ToString()).FontSize(11));
                            col.Item().Text(ticket.Train_name).FontSize(9);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Вагон").Bold().FontSize(11);
                            col.Item().Text(text => text.Span(ticket.Train_van.ToString()).FontSize(11));
                            col.Item().Text(ticket.Van_name).FontSize(9);
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Место").Bold().FontSize(11);
                            col.Item().Text(text => text.Span(ticket.Train_seat.ToString()).FontSize(11));
                            col.Item().Text(ticket.Seat_name).FontSize(9);
                        });
                    });

                    // Время отправления/прибытия
                    column.Item().PaddingVertical(8).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Отправление").Bold().FontSize(11);
                            col.Item().Text(ticket.Departure_time).FontSize(11);
                            col.Item().Text(ticket.Departure_city_name).FontSize(11);
                        });

                        row.RelativeItem().PaddingHorizontal(8).Column(col =>
                        {
                            col.Item().Text("Прибытие").Bold().FontSize(11);
                            col.Item().Text(ticket.Arrival_time).FontSize(11);   
                            col.Item().Text(ticket.Arrival_city_name).FontSize(11);
                        });
                    });

                    // Пассажир
                    column.Item().PaddingTop(20).Column(col =>
                    {
                        col.Item().Text("Пассажир").Bold().FontSize(11);
                        col.Item().Text(ticket.Passenger_name).FontSize(11);
                        col.Item().Text(ticket.Passenger_date_birth).FontSize(11);       
                        col.Item().Text(passportInfo).FontSize(11);
                    });
                });
            });
        }).GeneratePdf();
    }

    public async Task<IEnumerable<BookDto>> GetBookAsync(string login)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);
        var book = await _ticketRepository.GetBookWithDetailsAsync(user.Id);
        return book.Select(_ticketMapper.Map);
    }

    public async Task<TicketDto> GetTicketByIdAsync(int id)
    {
        var ticket = await _ticketRepository.GetTicketByIdAsync(id);
        return _ticketMapper.Map(ticket);   
    }
}
