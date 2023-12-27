using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipManager.Engine
{
    public interface ISql
    {
        public ISql? Get(object pk);
    }
}
