using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENN_Cargo.Models;

namespace ENN_Cargo.Core
{
   public interface IPendingRequest
    {
        Task AddPendingRequestAsync(PendingRequest request);
    }
}
