using Ensek.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.Api.Controllers;


/// <summary>
/// Operation on type agnostic data
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ImportersController : ControllerBase
{
    private readonly IDataImpoterFactory _dataImpoterFactory;

    public ImportersController(IDataImpoterFactory dataImpoterFactory)
    {
        _dataImpoterFactory = dataImpoterFactory;
    }

    /// <summary>
    /// Load initial data sheet with account updates
    /// </summary>
    [HttpPost("Accounts")]
    public async Task<IActionResult> Load(IFormFile formFile)
    {
        if (formFile != null)
        {
            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            var importer = await _dataImpoterFactory.Build(DataImporterType.AccountUpdate);
            var (itemsRead, itemsAccepted) = await importer.Load(stream);
            return Ok(new { itemsAccepted, itemsRead, importer.Id });
        }

        return BadRequest("File not found");
    }

    /// <summary>
    /// Load initial data sheet with meter readings
    /// </summary>
    [HttpPost("MeterReadings")]
    public async Task<IActionResult> Post(IFormFile formFile)
    {
        if (formFile != null)
        {
            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            var importer = await _dataImpoterFactory.Build(DataImporterType.MeterUpdate);
            var (itemsRead, itemsAccepted) = await importer.Load(stream);
            return Ok(new { itemsAccepted, itemsRead, importer.Id });
        }

        return BadRequest("File not found");
    }

    /// <summary>
    /// Returns a list of data importers
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var rtn = new Dictionary<DataImporterStatus, IEnumerable<object>>();

        foreach (var status in Enum.GetValues(typeof(DataImporterStatus)).Cast<DataImporterStatus>())
        {
            rtn[status] = (await _dataImpoterFactory
                .BuildAll(status))
                .Select(x => new { x.Id, Type = x.ImporterType });
        }

        return Ok(rtn);
    }

    /// <summary>
    /// Returns details of a single data importer
    /// </summary>
    [HttpGet("{importerId}")]
    public async Task<IActionResult> GetImporter(Guid importerId)
    {
        var importer = await _dataImpoterFactory.Build(importerId);

        return Ok(new { 
            importer.Id, 
            importer.ImporterType, 
            importer.Status,
            Errors = importer.Errors.Count() });
    }

    /// <summary>
    /// Returns errors in a single data importer
    /// </summary>
    [HttpGet("Status/{importerId}/Errors")]
    public async Task<IActionResult> GetImporterErrors(Guid importerId)
    {
        var importer = await _dataImpoterFactory.Build(importerId);

        return Ok(importer.Errors);
    }



    /// <summary>
    /// Validate an importer loaded data 
    /// Used to simulate the azure function "validate" 
    /// </summary>    
    [HttpPost("Validate")]     
    public async Task<IActionResult> Validate()
    {
        var rtn = new List<string>();
        var dataImporters = await _dataImpoterFactory.BuildAll(DataImporterStatus.Loaded);

        foreach (var dataImporter in dataImporters)
        {
            var (ItemsRead, ItemsAccepted) = await dataImporter.Validate();
            rtn.Add($"dataImporter {dataImporter.Id} {ItemsRead}/{ItemsAccepted}");
        }

        return Ok(rtn.ToArray());
    }


    /// <summary>
    /// Move validated data to the host system
    /// Used to simulate the azure function "Import" 
    /// </summary>         
    [HttpPost("Import")]   
    public async Task<IActionResult> Import()
    {
        var rtn = new List<string>();
        var dataImporters = await _dataImpoterFactory.BuildAll(DataImporterStatus.Validated);

        foreach (var dataImporter in dataImporters)
        {
            var (ItemsRead, ItemsAccepted) = await dataImporter.Import();
            rtn.Add($"dataImporter {dataImporter.Id} {ItemsRead}/{ItemsAccepted}");
        }

        return Ok(rtn.ToArray());
    }
}