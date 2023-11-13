using TaxiGomel.Data;
using TaxiGomel.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace TaxiGomel.Services
{
    public class CachedPositionsService : ICachedPositionsService
    {
        private readonly TaxiGomelContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedPositionsService(TaxiGomelContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        // получение списка емкостей из базы
        public IEnumerable<Position> GetPositions(int rowsNumber = 20)
        {
            return _dbContext.Positions.Take(rowsNumber).ToList();
        }

        // добавление списка емкостей в кэш
        public void AddPositions(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Position> Positions = _dbContext.Positions.Take(rowsNumber).ToList();
            if (Positions != null)
            {
                _memoryCache.Set(cacheKey, Positions, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            }

        }
        // получение списка емкостей из кэша или из базы, если нет в кэше
        public IEnumerable<Position> GetPositions(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Position> Positions;
            if (!_memoryCache.TryGetValue(cacheKey, out Positions))
            {
                Positions = _dbContext.Positions.Take(rowsNumber).ToList();
                if (Positions != null)
                {
                    _memoryCache.Set(cacheKey, Positions,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return Positions;
        }

    }
}

