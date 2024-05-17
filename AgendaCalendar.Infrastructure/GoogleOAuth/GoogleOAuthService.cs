using AgendaCalendar.Application.Common.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;

namespace AgendaCalendar.Infrastructure.Authorization
{
    public class GoogleOAuthService : IGoogleOAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public GoogleOAuthService(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GoogleSignIn(string tokenId)
        {
            var payload = GoogleJsonWebSignature.ValidateAsync(tokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                var userToAdd = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email
                };
                var identityResult = await _userManager.CreateAsync(userToAdd);
                if (identityResult.Succeeded)
                {
                    var newUser = await _userManager.FindByEmailAsync(payload.Email);
                    var userCalendar = new Calendar
                    {
                        Title = payload.Email,
                        CalendarDescription = $"Basic calendar, which belongs to {payload.Email}",
                        AuthorId = newUser.Id,
                        CalendarColor = "green"
                    };
                    await _unitOfWork.CalendarRepository.AddAsync(userCalendar);
                    await _unitOfWork.SaveAllAsync();
                    return newUser;
                }
                else
                {
                    return null;
                }
            }
            return user;
        }

        private string SanitizeUserName(string userName)
        {
            return new string(userName.Where(char.IsLetterOrDigit).ToArray());
        }
    }
}
