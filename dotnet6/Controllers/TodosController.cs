using Microsoft.AspNetCore.Mvc;
using dotnet6.Repositories;
using dotnet6.Models;
using System.Text.Json;

namespace dotnet6.Controllers;

class CreateRequestBody {
    public String content { get; set; } = "";
}

[ApiController]
[Route("[controller]")]
public class TodosController : ControllerBase
{
    private TodosRepository _todoRepo;
    private readonly ILogger<TodosController> _logger;

    public TodosController(ILogger<TodosController> logger)
    {
        _logger = logger;
        _todoRepo = new TodosRepository();
    }

    [HttpGet]
    [Route("")]
    public IEnumerable<TodoModel> GetAll()
    {
        try {
            return this._todoRepo.fetchAll();
        }catch(Exception e) {
            throw new HttpRequestException(e.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public TodoModel FindOne(Guid id)
    {
        try {
            return this._todoRepo.findOne(id);
        }catch(Exception e) {
            throw new HttpRequestException(e.Message);
        }
    }

    [HttpPost]
    [Route("")]
    public async Task<TodoModel> CreateAsync()
    {
        try {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                var deserialized = JsonSerializer.Deserialize<CreateRequestBody>(body);
                return this._todoRepo.create(deserialized?.content ?? "empty");
            }
        }catch(Exception e) {
            throw new HttpRequestException(e.Message);
        }
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<TodoModel> Update(Guid id)
    {
        try {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                var deserialized = JsonSerializer.Deserialize<CreateRequestBody>(body);
                return this._todoRepo.update(id, deserialized?.content ?? "empty");
            }
        }catch(Exception e) {
            throw new HttpRequestException(e.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public TodoModel Delete(Guid id)
    {
        try {
            return this._todoRepo.delete(id);
        }catch(Exception e) {
            throw new HttpRequestException(e.Message);
        }
    }
}
