using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDiff.Utilities.Exceptions
{
    public class LogicalException : Exception
    {
        public LogicalException(string message, Exception innerexception) : base(message, innerexception)
        {

        }
    }
}
