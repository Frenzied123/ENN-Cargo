using ENN_Cargo.DataAccess.Repository.IRepository; 
using ENN_Cargo.Models;                          
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class PendingRequestService : IPendingRequest
    {
        private readonly IRepository<PendingRequest> _pendingRequestRepository;
        public PendingRequestService(IRepository<PendingRequest> pendingRequestRepository)
        {
            _pendingRequestRepository = pendingRequestRepository;
        }

        public async Task AddPendingRequestAsync(PendingRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _pendingRequestRepository.AddAsync(request);
        }
    }
}