using TaxiGomel.Models;
using System.Collections.Generic;

namespace TaxiGomel.Services
{
    public interface ICachedCarsService
    {
        public IEnumerable<Car> GetCars(int rowsNumber = 20);
        public void AddCars(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Car> GetCars(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Car> GetCarsByPriceAndModel(int price, int CarModelID);
        
    }
}
