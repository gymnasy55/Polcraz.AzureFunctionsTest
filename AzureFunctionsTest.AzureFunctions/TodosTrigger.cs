using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AzureFunctionsTest.AzureFunctions.HttpModels.Request.Todos;
using AzureFunctionsTest.AzureFunctions.HttpModels.Response.Message;
using AzureFunctionsTest.Domain.Models;
using AzureFunctionsTest.Domain.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AzureFunctionsTest.AzureFunctions
{
    public class TodosTrigger
    {
        private const string Route = "todos";

        private readonly ITodoRepository _todoRepository;

        public TodosTrigger(ITodoRepository todoRepository) => _todoRepository = todoRepository;

        [FunctionName("GetAllTodos")]
        [OpenApiOperation(operationId: "GetAllTodos", tags: new[] { "Todos" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Todo>), Description = "The OK response")]
        public IActionResult GetAllTodos(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Route)] 
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetAllTodos HTTP trigger function: getting all todos");

            var result = _todoRepository.GetAll();

            return new OkObjectResult(result);
        }

        [FunctionName("GetTodoById")]
        [OpenApiOperation(operationId: "GetTodoById", tags: new[] { "Todos" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "Id of todo item")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Todo), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NoContent, contentType: "text/plain", bodyType: typeof(string), Description = "The NoContent response")]
        public IActionResult GetTodoById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Route + "/{id}")]
            HttpRequest req,
            ILogger log,
            Guid id)
        {
            log.LogInformation("GetTodoById HTTP trigger function: getting todo by id");

            var result = _todoRepository.GetByKey(id);

            return new OkObjectResult(result);
        }

        [FunctionName("AddTodo")]
        [OpenApiOperation(operationId: "AddTodo", tags: new[] { "Todos" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(AddTodoDto), Required = true, Description = "Todo item")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Todo), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(MessageResponse), Description = "The BadRequest response", Summary = "Provide correct object")]
        public async Task<IActionResult> AddTodo(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Route)]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("AddTodo HTTP trigger function: adding todos");

            var stringBody = await new StreamReader(req.Body).ReadToEndAsync();

            AddTodoDto todo;
            try
            {
                todo = JsonConvert.DeserializeObject<AddTodoDto>(stringBody);
                if (todo == null)
                    throw new FormatException();
            }
            catch (Exception)
            {
                return new BadRequestObjectResult(new MessageResponse {Message = "Provide correct object"});
            }

            var result = _todoRepository.Add(new Todo
                {TaskDescription = todo.TaskDescription, IsCompleted = todo.IsCompleted});

            return new OkObjectResult(result);
        }
    }
}

