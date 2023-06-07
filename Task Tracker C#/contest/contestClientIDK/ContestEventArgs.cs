using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contestClientIDK
{

    public enum ContestClientEvent
    {
        Update
    };

    public class ContestEventArgs : EventArgs
    {
        private readonly ContestClientEvent clientEvent;
        private readonly Object data;

        public ContestEventArgs(ContestClientEvent clientEvent, object data)
        {
            this.clientEvent = clientEvent;
            this.data = data;
        }

        public ContestClientEvent ClientEventType
        {
            get { return clientEvent; }
        }

        public object Data
        {
            get { return data; }
        }
    }
}
