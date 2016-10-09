using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample_leap
{
    
    class Map_Value
    {
        public decimal MapValue(decimal val, decimal input_min, decimal input_max, decimal output_min, decimal output_max)
        {
            decimal result = (val - input_min) * (output_max - output_min) / (input_max - input_min) + output_min;

            
            if (result.CompareTo(output_min) < 0)
            {
                return output_min;
            }
            else if (result.CompareTo(output_max) > 0)
            {
                return output_max;
            }
            else
            { return result; }
          
        }

      
  }
}
