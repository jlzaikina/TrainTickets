using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Domain;
using TrainTickets.UI.Ports;

namespace TrainTickets.UI.Application.Test.Handlers;

// тут пишется логика
public class UserHandler : IUserHandler
{
    private readonly IUserRepository _testRepository;
    private readonly IUserMapper _testMapper;

    public UserHandler(IUserRepository testRepository, IUserMapper testMapper)
    {
        _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
        _testMapper = testMapper ?? throw new ArgumentNullException(nameof(testMapper));
    }

    public async Task<IEnumerable<UserDto>> GetAllUserAsync()
    {
        var testEntity = await _testRepository.GetAllUserAsync();

        //List<TestDto> list = new List<TestDto>();
        //foreach (var i in testEntity)
        //{
        //    list.Add(new TestDto { Id = i.Id});
        //}

        return testEntity.Select(_testMapper.Map);
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var testEntity = await _testRepository.GetUserByIdAsync(id);

        //List<TestDto> list = new List<TestDto>();
        //foreach (var i in testEntity)
        //{
        //    list.Add(new TestDto { Id = i.Id});
        //}

        return _testMapper.Map(testEntity);
    }
}