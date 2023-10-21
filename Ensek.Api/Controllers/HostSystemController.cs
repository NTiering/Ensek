using Ensek.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HostSystemController : ControllerBase
{
    private readonly ISystemRepository _systemRepository;

    public HostSystemController(ISystemRepository systemRepository)
    {
        _systemRepository = systemRepository;
    }

    /// <summary>
    /// Returns a list of matching accounts
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(string search, int pageSize = 25, int pageCount = 1)
    {
        var data = await _systemRepository.Search(search, pageSize, pageCount);
        return Ok(data);
    }

    /// <summary>
    /// Returns a single account using the account id 
    /// </summary>
    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetAccount(int accountId)
    {
        var data = await _systemRepository.GetAccount(accountId);
        return data == null ? BadRequest("account not found") : Ok(data);
    }

    /// <summary>
    /// Returns meter reading for a single account the account id 
    /// </summary>
    [HttpGet("account/{accountId}/readings")]
    public async Task<IActionResult> GetReadings(int accountId)
    {
        var data = await _systemRepository.GetReadings(accountId);
        return Ok(data);
    }

}