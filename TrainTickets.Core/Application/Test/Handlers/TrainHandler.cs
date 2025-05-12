using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.Ticket;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

public class TrainHandler : ITrainHandler
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
            Id_user = userId,
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

    public async Task<IEnumerable<int>> GetVanNumberAsync()
    {
        return await _trainRepository.GetVanNumberAsync();
    }

    public async Task<IEnumerable<SchemaDto>> GetAllSchemaAsync()
    {
        var shema = await _trainRepository.GetAllSchemaAsync();
        return shema.Select(_trainMapper.Map);
    }
    public async Task<SchemaDto> GetSchemaAsync(int id)
    {
        var shema = await _trainRepository.GetSchemaByIdAsync(id);
        return _trainMapper.Map(shema);
    }
    public async Task<bool> SaveSchemaAsync(SaveSchemaRequest request)
    {
        string newSchemaName;

        request.JsonSchema.TryGetProperty("schemaName", out var name);
        newSchemaName = name.GetString();

        var existingSchemasName = await _trainRepository.GetSchemaNameAsync();
        if (existingSchemasName.Any(s => s == newSchemaName))
        {
            throw new ApplicationException("Схема с таким названием уже существует");
        }
        var entity = new SchemaEntity
        {
            Schema = request.JsonSchema.GetRawText(),
        };
        await _trainRepository.AddSchema(entity);
        return true;
    }

    public async Task<bool> UpdateSchemaAsync(int id, SaveSchemaRequest request)
    {
        string newSchemaName;

        request.JsonSchema.TryGetProperty("schemaName", out var name);
        newSchemaName = name.GetString();

        var oldSchemaName = await _trainRepository.GetSchemaNameByIdAsync(id);
        if (oldSchemaName != newSchemaName)
        {
            var existingSchemasName = await _trainRepository.GetSchemaNameAsync();

            if (existingSchemasName.Any(s => s == newSchemaName))
            {
                throw new ApplicationException("Схема с таким названием уже существует");
            }
        }

        var schema = await _trainRepository.GetSchemaByIdAsync(id);
        schema.Schema = request.JsonSchema.GetRawText();

        var activeTrains = await _trainRepository.GetActiveTrainsAsync();

        var unactiveVans = await _trainRepository.GetUnactiveVansAsync(activeTrains, id);

        foreach (var van in unactiveVans)
        {
            van.Copy_schema = request.JsonSchema.GetRawText();
        }
        
        await _trainRepository.UpdateSchema(schema);
        return true;
    }

    public async Task<bool> DeleteSchemaAsync(int id)
    {
        var schema = await _trainRepository.GetSchemaByIdAsync(id);
        await _trainRepository.DeleteSchema(schema);
        return true;
    }

    public async Task<bool> CreateTrainAsync(CreateTrainRequest request)
    {
        int idTrainType = await _trainRepository.GetTypeTrainIdAsync(request.TrainType);
        var trainEntity = new TrainEntity
        {
            Number_train = request.TrainNumber,
            Id_type_train = idTrainType,
            Name = request.TrainName,
            Vans = new List<VanEntity>()
        };

        foreach (var c in request.Vans)
        {
            var schema = await _trainRepository.GetSchemaByIdAsync(c.SchemaId);
            var json = JsonDocument.Parse(schema.Schema);
            json.RootElement.TryGetProperty("schemaType", out var typeProp);
            var typeVan = typeProp.GetString();
            var idVanType = await _trainRepository.GetTypeVanIdAsync(typeVan);
            var vanEntity = new VanEntity
            {
                Number_van = c.VanNumber,
                Id_type_van = idVanType,
                Copy_schema = schema.Schema,
                Id_schema = c.SchemaId,
                Seats = new List<SeatEntity>()
            };

            await GenerateSeats(vanEntity, json, typeVan);
            trainEntity.Vans.Add(vanEntity);
        }
        await _trainRepository.AddTrain(trainEntity);
        return true;
    }

    public async Task<bool> DeleteTrainAsync(int number)
    {
        var train = await _trainRepository.GetTrainByNumberAsync(number);
        await _trainRepository.DeleteTrain(train);
        return true;
    }

    public async Task<IEnumerable<TrainDetailsDto>> GetTrainsAsync()
    {
        var train = await _trainRepository.GetAllTrainsAsync();
        return train.Select(_trainMapper.Map);
    }

    private async Task GenerateSeats(VanEntity vanEntity, JsonDocument json, string typeVan)
    {
        var root = json.RootElement;

        if (typeVan == "Купе" || typeVan == "Плацкарт" || typeVan == "СВ")
        {
            if (json.RootElement.TryGetProperty("compartments", out var compartments))
            {
                foreach (var comp in compartments.EnumerateArray())
                {
                    var seats = comp.GetProperty("seats").EnumerateArray().ToList();
                    for (int i = 0; i < seats.Count; i++)
                    {
                        var seatNum = seats[i].GetInt32();
                        var seatType = seats.Count == 2 ? "нижнее" : (i % 2 == 0 ? "нижнее" : "верхнее");
                        vanEntity.Seats.Add(new SeatEntity
                        {
                            Number_seat = seatNum,
                            Id_type_seat = await _trainRepository.GetTypeSeatIdAsync(seatType)
                        });
                    }
                }
            }
            if (typeVan == "Плацкарт" && json.RootElement.TryGetProperty("sideSeats", out var sideSeats))
            {
                for (int i = 0; i < sideSeats.GetArrayLength(); i++)
                {
                    var seatNum = sideSeats[i].GetInt32();
                    var seatType = i % 2 == 0 ? "нижнее боковое" : "верхнее боковое";
                    vanEntity.Seats.Add(new SeatEntity
                    {
                        Number_seat = seatNum,
                        Id_type_seat = await _trainRepository.GetTypeSeatIdAsync(seatType)
                    });
                }
            }
        }
        else if (typeVan == "Сидячий")
        {
            if (json.RootElement.TryGetProperty("rows", out var rows))
            {
                foreach (var row in rows.EnumerateArray())
                {
                    foreach (var side in new[] { "leftSeats", "rightSeats" })
                    {
                        if (row.TryGetProperty(side, out var seatList))
                        {
                            foreach (var seat in seatList.EnumerateArray())
                            {
                                vanEntity.Seats.Add(new SeatEntity
                                {
                                    Number_seat = seat.GetInt32(),
                                    Id_type_seat = 5
                                });
                            }
                        }
                    }
                }
            }
        }
    }

    public async Task<bool> UpdateTrainAsync(TrainDetailsDto request)
    {
        var train = await _trainRepository.GetTrainByNumberAsync(request.TrainNumber);
        int idTrainType = await _trainRepository.GetTypeTrainIdAsync(request.TrainType);

        train.Id_type_train = idTrainType;
        train.Name = request.TrainName;

        await _trainRepository.DeleteVans(train.Vans);
        train.Vans.Clear();
        foreach (var c in request.Vans)
        {
            var schema = await _trainRepository.GetSchemaByIdAsync(c.SchemaId);
            var json = JsonDocument.Parse(schema.Schema);
            json.RootElement.TryGetProperty("schemaType", out var typeProp);
            var typeVan = typeProp.GetString();
            var idVanType = await _trainRepository.GetTypeVanIdAsync(typeVan);
            var vanEntity = new VanEntity
            {
                Number_van = c.VanNumber,
                Id_type_van = idVanType,
                Copy_schema = schema.Schema,
                Id_schema = c.SchemaId,
                Seats = new List<SeatEntity>()
            };

            await GenerateSeats(vanEntity, json, typeVan);

            train.Vans.Add(vanEntity);
        }
        await _trainRepository.UpdateTrain(train);
        return true;
    }

    public async Task<IEnumerable<string>> GetTypeTrainsAsync()
    {
        return await _trainRepository.GetTypeTrainsAsync();
    }

    public async Task<bool> CheckUpdateTrainAsync(int numberTrain)
    {
        return await _trainRepository.CheckUpdateTrainAsync(numberTrain);
    }
}
