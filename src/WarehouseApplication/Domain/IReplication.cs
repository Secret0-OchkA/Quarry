using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IReplication
    {
        long Version { get; }
        DateTimeOffset timestamp { get; }

        public void NewVersion();
    }
}
