using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contestService
{
    public class ContestException: Exception
    {
        public ContestException() : base() { }

        public ContestException(String msg) : base(msg) { }

        public ContestException(String msg, Exception ex) : base(msg, ex) { }
    }
}
