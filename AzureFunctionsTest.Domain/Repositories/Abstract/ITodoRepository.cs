using System;
using AzureFunctionsTest.Domain.Models;

namespace AzureFunctionsTest.Domain.Repositories.Abstract
{
    public interface ITodoRepository : IRepository<Todo, Guid>
    { }
}
