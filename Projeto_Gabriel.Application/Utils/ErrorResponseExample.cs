using Projeto_Gabriel.Application.Utils;
using Swashbuckle.AspNetCore.Filters;

public class ErrorResponseExample : IExamplesProvider<ErrorResponse>
{
    public ErrorResponse GetExamples()
    {
        return new ErrorResponse { Message = "string" };
    }
}