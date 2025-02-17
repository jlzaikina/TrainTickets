using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainTickets.UI.Entities;

namespace TrainTickets.UI.Ports
{
    public interface ISessionRepository
    {
        SessionEntity AddSession(SessionEntity entity);
    }
}
