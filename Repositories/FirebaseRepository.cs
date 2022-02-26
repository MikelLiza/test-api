namespace APITest.Repositories;

using System.Security.Cryptography;
using Firebase.Database;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Models.DTOs;
using Newtonsoft.Json;
using Services;

public interface IFirebaseRepository
{
    Task<List<FirebaseUserDTO>> GetAllUsers();
    Task<FirebaseUserDTO> PostUser(FirebaseUserDTO request);
    Task<FirebaseUserDTO> Login(FirebaseUserDTO request);
}

public class FirebaseRepository : IFirebaseRepository
{
    private readonly FirebaseClient _firebaseClient;
    private readonly ISettingsService _settingsService;

    public FirebaseRepository(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _firebaseClient = new FirebaseClient(settingsService.GetFirebaseConfig().BasePath);
    }

    public async Task<List<FirebaseUserDTO>> GetAllUsers()
    {
        var query = await _firebaseClient.Child("users").OnceAsync<FirebaseUserDTO>();

        return query.Select(user => user.Object).ToList();
    }

    public async Task<FirebaseUserDTO> PostUser(FirebaseUserDTO request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("An email, name and password is required to sign up.");

            var allUsers = await GetAllUsers();
            if (allUsers.FirstOrDefault(u => u.Email == request.Email) != null)
                throw new Exception($"A user with email: {request.Email} already exists.");

            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                request.Password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100000,
                256 / 8
            ));

            request.Id = Guid.NewGuid();
            request.Password = hashed + Convert.ToBase64String(salt);

            var jsonToPost = JsonConvert.SerializeObject(request);
            var query = await _firebaseClient.Child("users").PostAsync(jsonToPost);

            return JsonConvert.DeserializeObject<FirebaseUserDTO>(query.Object)!;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<FirebaseUserDTO> Login(FirebaseUserDTO request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("An email and password is required to sign in.");

            var allUsers = await GetAllUsers();
            var existingUser = allUsers.FirstOrDefault(u => u.Email == request.Email);
            if (existingUser == null)
                throw new Exception($"User with {request.Email} does not exist.");

            var existingHashedPassword = existingUser.Password[..44];
            var salt = Convert.FromBase64String(existingUser.Password[44..]);
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                request.Password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100000,
                256 / 8
            ));

            if (hashed != existingHashedPassword)
                throw new Exception("Passwords do not match.");

            return existingUser;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}