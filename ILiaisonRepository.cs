using System.Collections.Generic;
using System.Threading.Tasks;
using Desmos.Models;
using System.Linq;

namespace Desmos.Repositories
{
    public interface ILiaisonRepository
    {
        Task<Liaison> GetLiasonAsync(string liasonId);
        Task<IEnumerable<Liaison>> GetLiasonsFromEntityAsync(string entityId);

        Task<bool> AddLiasonAsync(Liaison liason);
        Task<bool> DeleteLiasonAsync(string liasonId);
    }

    public class LiasonRepository : ILiaisonRepository
    {
        private static List<Liaison> _liaisons = new List<Liaison>
        {

        };

        public Task<Liaison> GetLiasonAsync(string liasonId) => 
            Task.FromResult(_liaisons.SingleOrDefault(l => l.Id == liasonId));

        public Task<IEnumerable<Liaison>> GetLiasonsFromEntityAsync(string entityId) =>
            Task.FromResult(_liaisons.Where(l => l.FromId == entityId || l.ToId == entityId));

        public Task<bool> AddLiasonAsync(Liaison liason)
        {
            _liaisons.Add(liason);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteLiasonAsync(string liasonId)
        {
            _liaisons.RemoveAll(l => l.Id == liasonId);
            return Task.FromResult(true);
        }
    }
}
