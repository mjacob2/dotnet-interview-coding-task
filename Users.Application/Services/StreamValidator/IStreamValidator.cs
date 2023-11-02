using Users.Persistence;

namespace Users.Application.Services.StreamValidator
{
    public interface IStreamValidator
    {
        Task<List<UserProfile>> GetValidUserProfiles(StreamReader stream);
    }
}
