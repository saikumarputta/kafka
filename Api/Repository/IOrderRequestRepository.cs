using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Repository
{
    public interface IOrderRequestRepository
    {
        void Create(OrderRequest request);
    }
}