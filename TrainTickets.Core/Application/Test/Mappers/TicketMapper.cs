using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.User;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Application.Test.Mappers;

public class TicketMapper: ITicketMapper
{
    public BookDto Map(BookEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return new BookDto()
        {
            Id_book = entity.Id_book,
            Tickets = entity.Tickets.Select(t => new TicketDto
            {
                Id_ticket = t.Id_ticket,
                Passenger_name = $"{t.Passenger.Surname} {t.Passenger.Name} {t.Passenger.Midname}",
                Passenger_date_birth = t.Passenger.Date_birth?.ToString("dd.MM.yyyy"),
                Passenger_passport = t.Passenger.Passport,
                Train_number = t.Seat.Van.Train.Number_train,
                Train_name = t.Seat.Van.Train.Name,
                Train_van = t.Seat.Van.Number_van,
                Van_name = t.Seat.Van.Type_van.Name,
                Train_seat = t.Seat.Number_seat,
                Departure_time = t.Book.Schedule.Date_departure.ToString("dd.MM.yyyy HH:mm"),
                Arrival_time = t.Book.Schedule.Date_arrival.ToString("dd.MM.yyyy HH:mm"),
                Arrival_city_name = t.Book.Schedule.Route.ArrivalCity.Name,
                Departure_city_name = t.Book.Schedule.Route.DepartureCity.Name,
                Seat_name = t.Seat.Type_seat.Name,

                PdfUrl = $"/api/tickets/{t.Id_ticket}/pdf"
            }).ToList()
        };
    }

    public TicketDto Map(TicketEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return new TicketDto()
        {
                Id_ticket = entity.Id_ticket,
                Passenger_name = $"{entity.Passenger.Surname} {entity.Passenger.Name} {entity.Passenger.Midname}",
                Passenger_date_birth = entity.Passenger.Date_birth?.ToString("dd.MM.yyyy"),
                Passenger_passport = entity.Passenger.Passport,
                Train_number = entity.Seat.Van.Train.Number_train,
                Train_name = entity.Seat.Van.Train.Name,
                Train_van = entity.Seat.Van.Number_van,
                Van_name = entity.Seat.Van.Type_van.Name,
                Train_seat = entity.Seat.Number_seat,
                Departure_time = entity.Book.Schedule.Date_departure.ToString("dd-MM-yyyy HH:mm"),
                Arrival_time = entity.Book.Schedule.Date_arrival.ToString("dd-MM-yyyy HH:mm"),
                Arrival_city_name = entity.Book.Schedule.Route.ArrivalCity.Name,
                Departure_city_name = entity.Book.Schedule.Route.DepartureCity.Name,
                Seat_name = entity.Seat.Type_seat.Name,

                PdfUrl = $"/api/tickets/{entity.Id_ticket}/pdf"
        };
    }
}
