using Newtonsoft.Json;
using Users.Persistence;

namespace Users.Application.Services.StreamValidator
{
    /// <summary>
    /// Class to validate and extract valid user profiles from a StreamReader instance.
    /// </summary>
    public class StreamValidator : IStreamValidator
    {
        private readonly int userNameMaxLength = 100;

        /// <summary>
        /// Asynchronously reads through the Stream and returns a list of valid <see cref="UserProfile"/>.
        /// </summary>
        /// <param name="stream">The StreamReader instance which could contain serialized UserProfile data.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task result contains a list of valid UserProfiles.</returns>
        public async Task<List<UserProfile>> GetValidUserProfiles(StreamReader stream)
        {
            string line;
            var validUsers = new List<UserProfile>();
            var emptyList = new List<UserProfile>();

            while ((line = await stream.ReadLineAsync()) != null)
            {
                try
                {
                    var userUpdate = JsonConvert.DeserializeObject<UserProfile>(line);

                    if (userUpdate == null || !IsValidUserProfile(userUpdate))
                    {
                        Console.WriteLine("Invalid user data.");
                        return emptyList;
                    }

                    validUsers.Add(userUpdate);
                }
                catch (JsonSerializationException ex)
                {
                    Console.WriteLine($"An error occurred while deserializing the user data: {ex.Message}");
                    return emptyList;
                }
            }

            return validUsers;
        }

        /// <summary>
        /// Checks if a <see cref="UserProfile"/> instance is valid i.e. it has a non-null/non-empty email and first name within character limit.
        /// </summary>
        /// <param name="userUpdate">The UserProfile instance to validate</param>
        /// <returns>A boolean representing whether the UserProfile instance is valid</returns>
        private bool IsValidUserProfile(UserProfile userUpdate)
        {
            if (string.IsNullOrEmpty(userUpdate.Email))
            {
                Console.WriteLine($"User id: {userUpdate.Id}, e-mail is null or empty!");
                return false;
            }

            if (string.IsNullOrEmpty(userUpdate.FirstName))
            {
                Console.WriteLine($"User id: {userUpdate.Id}, first name e-mail is null or empty!");
                return false;
            }

            if (userUpdate.FirstName.Length > userNameMaxLength)
            {
                Console.WriteLine($"User id: {userUpdate.Id}, first name e-mail is more than 100 characters long!");
                return false;
            }

            return true;
        }
    }
}
