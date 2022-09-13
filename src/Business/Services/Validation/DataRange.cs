using System.ComponentModel.DataAnnotations;

namespace Services.Validation
{
    public class DataRange : RangeAttribute
    {
        public DataRange(double minimum, double maximum) : base(minimum, maximum)
        {
        }
    }
}
