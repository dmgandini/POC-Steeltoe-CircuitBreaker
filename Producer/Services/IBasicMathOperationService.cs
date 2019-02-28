using Producer.Models;

namespace Producer.Services
{
    public interface IBasicMathOperationService
    {
        OperationModel Exec(OperationModel input);
    }
}