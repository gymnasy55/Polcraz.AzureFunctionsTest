namespace AzureFunctionsTest.AzureFunctions.HttpModels.Request.Todos
{
    public class AddTodoDto
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
