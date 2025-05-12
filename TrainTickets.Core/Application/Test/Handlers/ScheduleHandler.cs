using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

/// <inheritdoc/>
public class ScheduleHandler: IScheduleHandler
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ITrainRepository _trainRepository;
    private readonly IScheduleMapper _scheduleMapper;

    public ScheduleHandler(IScheduleRepository scheduleRepository, IScheduleMapper scheduleMapper, ITrainRepository trainRepository)
    {
        _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
        _trainRepository = trainRepository ?? throw new ArgumentNullException(nameof(trainRepository));
        _scheduleMapper = scheduleMapper ?? throw new ArgumentNullException(nameof(scheduleMapper));
    }

    public async Task<IEnumerable<CityDto>> GetCitiesAsync()
    {
        var cityEntity = await _scheduleRepository.GetCitiesAsync();
        return cityEntity.Select(_scheduleMapper.Map);
    }

    public async Task<IEnumerable<ScheduleDto>> GetFilterScheduleAsync(FilterRequest request)
    {
        var sheduleEntity = await _scheduleRepository.GetFilterScheduleAsync(request);
        return sheduleEntity.Select(_scheduleMapper.Map);
    }

    public async Task<IEnumerable<ScheduleDto>> GetScheduleAsync()
    {
        var sheduleEntity = await _scheduleRepository.GetSchedulesAsync();
        return sheduleEntity.Select(_scheduleMapper.Map);
    }

    public async Task<bool> DeleteTripAsync(InfoTrainRequest request)
    {
        var schedule = await _scheduleRepository.GetScheduleAsync(request);

        var vans = await _trainRepository.GetVansAsync(request.Number_train);

        foreach (var van in vans)
        {
            van.Copy_schema = van.Schema.Schema;
            await _trainRepository.DeleteSeats(van.Seats);
            van.Seats.Clear();
            var json = JsonDocument.Parse(van.Copy_schema);
            json.RootElement.TryGetProperty("schemaType", out var typeProp);
            var typeVan = typeProp.GetString();
            await GenerateSeats(van, json, typeVan);
        }
        await _scheduleRepository.DeleteTrip(request);
        return true;
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
    public async Task<bool> UpdateTripAsync(UpdateScheduleRequest request)
    {
        var schedule = await _scheduleRepository.GetOneScheduleAsync(request);

        var departureCity = await _scheduleRepository.GetCityByNameAsync(request.DepartureCityNameNew);
        var arrivalCity = await _scheduleRepository.GetCityByNameAsync(request.ArrivalCityNameNew);

        if (departureCity == null)
            throw new ApplicationException("Город отправления не найден");

        if (arrivalCity == null)
            throw new ApplicationException("Город прибытия не найден");

        var route = await _scheduleRepository.FindRouteAsync(departureCity.Code_city, arrivalCity.Code_city);

        if (route == null)
            throw new ApplicationException("Маршрут не найден");

        if (schedule.Number_train != request.NumberTrain ||
                schedule.Date_departure.Date != request.DepartureTimeNew.Date)
        {
            if (await _scheduleRepository.ExistsScheduleAsync(request.NumberTrain, request.DepartureTime))
                throw new ApplicationException("Поезд уже назначен на рейс в этот день");
        }


        schedule.Date_departure = request.DepartureTimeNew;
        schedule.Date_arrival = request.ArrivalTimeNew;
        schedule.Id_route = route.Id_route;
        _scheduleRepository.UpdateTrip(schedule);

        return true;
    }

    public async Task<bool> CreateTripAsync(CreateScheduleRequest request)
    {
        var departureCity = await _scheduleRepository.GetCityByNameAsync(request.DepartureCityNameNew);
        var arrivalCity = await _scheduleRepository.GetCityByNameAsync(request.ArrivalCityNameNew);

        if (departureCity == null)
            throw new ApplicationException("Город отправления не найден");

        if (arrivalCity == null)
            throw new ApplicationException("Город прибытия не найден");

        var route = await _scheduleRepository.FindRouteAsync(departureCity.Code_city, arrivalCity.Code_city);

        if (route == null)
            throw new ApplicationException("Маршрут не найден");
        
        if (await _scheduleRepository.ExistsScheduleAsync(request.NumberTrainNew, request.DepartureTimeNew))
                throw new ApplicationException("Поезд уже назначен на рейс в этот день");

        _scheduleRepository.AddTrip(_scheduleMapper.Map(request, route.Id_route));

        return true;
    }

    public async Task<IEnumerable<RouteDto>> GetRoutesAsync()
    {
        var cityEntity = await _scheduleRepository.GetRoutesAsync();
        return cityEntity.Select(_scheduleMapper.Map);
    }
}
