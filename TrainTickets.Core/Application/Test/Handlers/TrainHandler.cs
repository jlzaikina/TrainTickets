using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

public class TrainHandler: ITrainHandler
{
    private readonly ITrainRepository _trainRepository;
    private readonly ITrainMapper _trainMapper;

    public TrainHandler(ITrainRepository trainRepository, ITrainMapper trainMapper)
    {
        _trainRepository = trainRepository ?? throw new ArgumentNullException(nameof(trainRepository));
        _trainMapper = trainMapper ?? throw new ArgumentNullException(nameof(trainMapper));
    }

    public async Task<TrainDto> GetInfoTrainInScheduleAsync(InfoTrainRequest request)
    {
        var sheduleEntity = await _trainRepository.GetInfoTrainInScheduleAsync(request);
        return _trainMapper.Map(sheduleEntity);
    }
}
