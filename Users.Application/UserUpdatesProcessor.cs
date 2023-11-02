using Microsoft.EntityFrameworkCore;
using Users.Application.Services.StreamValidator;
using Users.Persistence;

namespace Users.Application;

public class UserUpdatesProcessor
{
    private readonly UserContext _context;
    private readonly IStreamValidator _streamValidator;

    public UserUpdatesProcessor(UserContext context, IStreamValidator streamValidator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _streamValidator = streamValidator ?? throw new ArgumentNullException(nameof(streamValidator));
    }

    public async Task Process(StreamReader stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        var validUserProfiles = await _streamValidator.GetValidUserProfiles(stream);

        if (validUserProfiles == null || validUserProfiles.Count == 0)
        {
            throw new ArgumentException("Stream data is not valid!");
        }

        var existingUserIds = await _context.UserProfiles.AsNoTracking().Select(user => user.Id).ToListAsync();
        var newUsersToAdd = validUserProfiles.Where(user => !existingUserIds.Contains(user.Id)).ToList();
        var existingUsersToUpdate = validUserProfiles.Where(user => existingUserIds.Contains(user.Id)).ToList();

        try
        {
            await _context.AddRangeAsync(newUsersToAdd);
            _context.UpdateRange(existingUsersToUpdate);
            await _context.SaveChangesAsync();
            Console.WriteLine("All data processed succesfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured while performing actions in database: {ex.Message}");
        }
    }
}