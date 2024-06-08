using API.Context;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UserController : Controller
{
    private readonly DBContext _context;
    private readonly UserService _userService;

    public UserController(DBContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> SetAccountStatus(int id, [FromBody] UserStatus model)
    {
        if (!Enum.TryParse<Status>(model.Status, true, out var status))
        {
            return BadRequest("Invalid status value");
        }
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.Status = status;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}/selection")]
    public async Task<IActionResult> SetUserSelection(int id, [FromBody] UserSelection model)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.isSelected = model.IsSelected ?? user.isSelected;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("delete-users")]
    public async Task<IActionResult> DeleteUsers([FromBody] List<int> userIds)
    {
        foreach (var id in userIds)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
        }
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
