﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieList.DTO;
using MovieList.Models;

namespace MovieList.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApiUser> _userManager;
    private readonly SignInManager<ApiUser> _signInManager;

    public AccountController(
        ApplicationDbContext context,
        ILogger<AccountController> logger,
        IConfiguration configuration,
        UserManager<ApiUser> userManager,
        SignInManager<ApiUser> signInManager)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<ActionResult> Register(RegisterDTO input)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApiUser()
                {
                    UserName = input.UserName,
                    Email = input.Email
                };
                var result = await _userManager.CreateAsync(
                    newUser, input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation(
                        "User {userName} ({email}) has been created.",
                        newUser.UserName, newUser.Email);
                    return StatusCode(201,
                        $"User '{newUser.UserName}' has been created.");
                }
                else
                    throw new Exception(
                        string.Format("Error: {0}", string.Join(" ",
                            result.Errors.Select(e => e.Description))));
            }
            else
            {
                var details = new ValidationProblemDetails(ModelState);
                details.Type =
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                details.Status = StatusCodes.Status400BadRequest;
                return new BadRequestObjectResult(details);
            }
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails();
            exceptionDetails.Detail = e.Message;
            exceptionDetails.Status =
                StatusCodes.Status500InternalServerError;
            exceptionDetails.Type =
                "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                exceptionDetails);
        }
    }

    [HttpPost]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<ActionResult> Login()
    {
        throw new NotImplementedException();
    }
}