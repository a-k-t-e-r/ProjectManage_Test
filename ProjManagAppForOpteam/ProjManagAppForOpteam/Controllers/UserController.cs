using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjManagAppForOpteam.Auth;
using ProjManagAppForOpteam.Models;
using ProjManagAppForOpteam.Repositories;

namespace ProjManagAppForOpteam.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ProjectManagementContext context, AuthService authService) : ControllerBase
{
    private readonly ProjectManagementContext _context = context;
    private readonly AuthService _authService = authService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
                u => u.Username == request.Username
                && u.Password == request.Password
            );

        if (user is null)
            return Unauthorized("Invalid username or password");

        // Generate the JWT token using AuthService
        var token = _authService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        try
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }
        catch
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return NotFound();

            return Ok(user);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }
        catch
        {
            throw;
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.UserId)
            return BadRequest();

        try
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
                return NotFound();
            else
                throw;
        }
        catch
        {
            throw;
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.UserId == id);
    }
}