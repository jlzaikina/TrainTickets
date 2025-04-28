using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

public class TrainHandler: ITrainHandler
{
    private readonly ITrainRepository _trainRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ITrainMapper _trainMapper;
    private readonly ITicketMapper _ticketMapper;
    private readonly IUserRepository _userRepository;

    public TrainHandler(ITrainRepository trainRepository, IUserRepository userRepository, ITicketRepository ticketRepository, ITrainMapper trainMapper, ITicketMapper ticketMapper)
    {
        _trainRepository = trainRepository ?? throw new ArgumentNullException(nameof(trainRepository));
        _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _trainMapper = trainMapper ?? throw new ArgumentNullException(nameof(trainMapper));
        _ticketMapper = ticketMapper ?? throw new ArgumentNullException(nameof(ticketMapper));
    }

    public async Task<TicketDto> CreateBookAsync(BookRequest request)
    {
        InfoTrainRequest infoTrainRequest = new InfoTrainRequest
        {
            DateDeparture = request.DateDeparture,
            Number_train = request.Number_train
        };
        // Полчение пользователя
        var user = await _userRepository.GetUserByLoginAsync(request.Login);

        // Получение расписания
        var schedule = await _trainRepository.GetInfoTrainInScheduleAsync(infoTrainRequest);

        // Получение места и проверка занятости места на конкретное расписание
        var seat = await _trainRepository.GetByNumberAsync(request.Number_seat, request.Number_van, request.Number_train);


        // Проверяем, что у пассажира нет билета на этот рейс
        if (await _trainRepository.HasTicketForScheduleAsync(request.Pass_id, schedule.Id_schedule))
        {
            throw new ApplicationException("У пассажира уже есть билет на этот рейс");
        }

        if (!await _trainRepository.IsSeatAvailableAsync(seat.Id_seat, schedule.Id_schedule))
            throw new ApplicationException("Место уже занято");


        var existingBooking = await _trainRepository.GetActiveBookingForScheduleAsync(user.Id, schedule.Id_schedule);

        // Если бронь существует и в ней меньше 4 пассажиров - добавляем в неё
        if (existingBooking != null && existingBooking.Tickets.Count < 4)
        {
            return await AddTicketToBooking(existingBooking.Id_book, request.Pass_id, seat.Id_seat, request.Number_train, request.DateDeparture, request.type_van, request.type_seat);
        }
        // Если бронь существует, но в ней уже 4 пассажира
        else if (existingBooking?.Tickets.Count >= 4)
        {
            throw new ApplicationException("В брони уже максимальное количество пассажиров (4)");
        }
        // Если брони нет или она неактивна - создаём новую
        else
        {
            // Проверяем лимит броней пользователя
            if (await _trainRepository.GetActiveBookingsCountAsync(user.Id) >= 5)
            {
                throw new ApplicationException("Превышен лимит броней (максимум 5)");
            }

            return await AddNewBooking(user.Id, schedule.Id_schedule, request.Pass_id, seat.Id_seat, request.Number_train, request.DateDeparture, request.type_van, request.type_seat);
        }
    }
    private async Task<TicketDto> AddNewBooking(long userId, int scheduleId, long passengerId, int seatId, int trainId, DateTime dateDeparture, string typeVan, string typeSeat)
    {
        // Создание новой брони
        var booking = new BookEntity
        {
            Date_create = DateTime.UtcNow,
            Id_schedule = scheduleId,
            Id_user= userId,
        };

        await _trainRepository.AddBook(booking);

        CheckRequest checkRequest = new CheckRequest
        {
            Number_train = trainId,
            DateDeparture = dateDeparture,
            type_seat = typeSeat,
            type_van = typeVan
        };

        // Расчёт цены билета
        var price = await GetPriceAsync(checkRequest);

        // Создание билета
        var ticket = new TicketEntity
        {
            Price = price,
            Id_book = booking.Id_book,
            Id_seat = seatId,
            Id_passenger = passengerId,
        };

        await _ticketRepository.AddTicket(ticket);

        ticket = await _ticketRepository.GetTicketByIdAsync(ticket.Id_ticket);

        return _ticketMapper.Map(ticket);
    }
    private async Task<TicketDto> AddTicketToBooking(int bookId, long passengerId, int seatId, int trainId, DateTime dateDeparture, string typeVan, string typeSeat)
    {
        CheckRequest checkRequest = new CheckRequest
        {
            Number_train = trainId,
            DateDeparture = dateDeparture,
            type_seat = typeSeat,
            type_van = typeVan
        };

        // Расчёт цены билета
        var price = await GetPriceAsync(checkRequest);

        // Создание билета
        var ticket = new TicketEntity
        {
            Price = price,
            Id_book = bookId,
            Id_seat = seatId,
            Id_passenger = passengerId,
        };

        await _ticketRepository.AddTicket(ticket);

        ticket = await _ticketRepository.GetTicketByIdAsync(ticket.Id_ticket);

        return _ticketMapper.Map(ticket);
    }
    public async Task<TrainDto> GetInfoTrainInScheduleAsync(InfoTrainRequest request)
    {
        var sheduleEntity = await _trainRepository.GetInfoTrainInScheduleAsync(request);
        return _trainMapper.Map(sheduleEntity);
    }

    public async Task<double> GetPriceAsync(CheckRequest request)
    {
        InfoTrainRequest request1 = new InfoTrainRequest();
        request1.Number_train = request.Number_train;   
        request1.DateDeparture = request.DateDeparture;
        double price = 2.5;
        var train = await _trainRepository.GetInfoTrainInScheduleAsync(request1);
        double km = train.Route.Distance;
        double koef_train = train.Train.Type_train.Route;
        var van = train.Train.Vans.FirstOrDefault(v => v.Type_van.Name == request.type_van);
        double koef_van = van.Type_van.Route;
        var seat = van.Seats.FirstOrDefault(v => v.Type_seat.Name == request.type_seat);
        double koef_seat = seat.Type_seat.Route;
        price = km * price * koef_train * koef_van * koef_seat;
        price = Math.Round(price, 0);
        return price;
    }

    public async Task<VanDto> GetShemaVanAsync(InfoVanRequest request)
    {
        var shema = await _trainRepository.GetShemaVanAsync(request);
        var seats = await _trainRepository.GetOccupiedSeatAsync(shema.Id_van);
        return _trainMapper.Map(shema, seats);
    }
}
