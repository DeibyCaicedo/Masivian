using Masivian.Interfaces;
using Masivian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masivian.interfaces
{
    public interface IRouletteService : IService
    {
        public Roulette create();

        public Roulette Find(string Id);

        public Roulette Open(string Id);
        public Roulette Close(string Id);

        public Roulette Bet(string Id, string UserId, int position, double money);

        public List<Roulette> GetAll();
    }
}
