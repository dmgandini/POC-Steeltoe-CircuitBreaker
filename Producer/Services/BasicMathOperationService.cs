using Producer.Models;

namespace Producer.Services
{
    public class BasicMathOperationService : IBasicMathOperationService
    {
        public OperationModel Exec(OperationModel input) =>
                new OperationModel
                {
                    FirstTerm = input.FirstTerm,
                    SecondTerm = input.SecondTerm,
                    Type = input.Type,
                    Result = Resolve(input)
                };

        private decimal Resolve(OperationModel input)
        {
            switch (input.Type)
            {
                case OperationType.Addition:
                    return input.FirstTerm + input.SecondTerm;

                case OperationType.Subtraction:
                    return input.FirstTerm - input.SecondTerm;

                case OperationType.Multiplication:
                    return input.FirstTerm * input.SecondTerm;

                case OperationType.Division:
                    return input.SecondTerm == 0 ? 
                        0 : 
                        input.FirstTerm / input.SecondTerm;
            }
            return 0;
        }
    }
}
