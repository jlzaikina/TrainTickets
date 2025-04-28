using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.Schedules;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

/// <inheritdoc/>
public class ScheduleHandler: IScheduleHandler
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IScheduleMapper _scheduleMapper;

    public ScheduleHandler(IScheduleRepository scheduleRepository, IScheduleMapper scheduleMapper)
    {
        _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
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
        var sheduleEntity = await _scheduleRepository.GetScheduleAsync();
        return sheduleEntity.Select(_scheduleMapper.Map);
    }

    public async Task<bool> DeleteTripAsync(InfoTrainRequest request)
    {
        await _scheduleRepository.DeleteTrip(request);
        return true;
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
