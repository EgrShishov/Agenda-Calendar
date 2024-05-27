using AgendaCalendar.Application;
using AgendaCalendar.Application.Authentication.Common;
using AgendaCalendar.WEB_API.Contracts.Authentication;
using MapsterMapper;
using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using AgendaCalendar.Domain.Common.Errors;
using AgendaCalendar.Application.Authentication.Queries;
using AgendaCalendar.Application.Users.Queries.AgendaCalendar.Application.Users.Queries;

namespace AgendaCalendar.WEB_API.Controllers
{
    [Route("api/account")]
    public class AccountController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AccountController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<SignUpCommand>(request);
            ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

            return authResult.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

            if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResult.FirstError.Description);
            }

            return authResult.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors));
        }

        [HttpGet("google-signin")]
        public async Task<IActionResult> GoogleSignIn(string token)
        {
            var userSignInResult = await _mediator.Send(new GoogleSigninQuery(token));
            return userSignInResult.Match(
                userSignInResult => Ok(_mapper.Map<AuthenticationResponse>(userSignInResult)),
                errors => Problem(errors));
        }
    }
}
