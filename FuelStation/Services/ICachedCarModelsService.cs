using TaxiGomel.Models;
using System.Collections.Generic;

namespace TaxiGomel.Services
{
    public interface ICachedCarModelsService
    {
        public IEnumerable<CarModel> GetCarModels(int rowsNumber = 20);
        public void AddCarModels(string cacheKey, int rowsNumber = 20);
        public IEnumerable<CarModel> GetCarModels(string cacheKey, int rowsNumber = 20);
    }
}
