using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contestService
{
    public interface IContestObserver
    {
        void updateTaskList();
    }
}
