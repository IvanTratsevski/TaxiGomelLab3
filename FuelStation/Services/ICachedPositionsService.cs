using TaxiGomel.Models;
using System.Collections.Generic;

namespace TaxiGomel.Services
{
    public interface ICachedPositionsService
    {
        public IEnumerable<Position> GetPositions(int rowsNumber = 20);
        public void AddPositions(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Position> GetPositions(string cacheKey, int rowsNumber = 20);
    }
}
