using System.ComponentModel.DataAnnotations;

namespace Producer.Models
{
    public class OperationModel
    {
        [Required]
        [Range(0, 999.99)]
        public decimal FirstTerm { get; set; }

        [Required]
        [Range(0, 999.99)]
        public decimal SecondTerm { get; set; }

        [Required]
        public OperationType Type { get; set; }

        public decimal Result { get; set; }
    }

    public enum OperationType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
}
