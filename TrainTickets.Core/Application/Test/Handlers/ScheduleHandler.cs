using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.Schedules;
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
}
