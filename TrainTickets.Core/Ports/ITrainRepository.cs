using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Domain.Train;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Ports;

public interface ITrainRepository
{
    Task<ScheduleEntity> GetInfoTrainInScheduleAsync(InfoTrainRequest request);
}
