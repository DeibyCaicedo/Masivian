using Masivian.Exceptions;
using Masivian.interfaces;
using Masivian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masivian.Repo
{
    public class RouletteService : IRouletteService
    {
        private IRouletteRepo rouletteRepo;

        public RouletteService(IRouletteRepo rouletteRepo)
        {
            this.rouletteRepo = rouletteRepo;
        }

        public Roulette create()
        {
            Roulette roulette = new Roulette()
            {
                Id = Guid.NewGuid().ToString(),
                IsOpen = false,
                OpenedAt = null,
                ClosedAt = null
            };
            rouletteRepo.Save(roulette);
            return roulette;
        }

        public Roulette Find(string Id)
        {
            return rouletteRepo.GetById(Id);
        }

        public Roulette Open(string Id)
        {
            Roulette roulette = rouletteRepo.GetById(Id);
            if (roulette == null)
            {
                throw new RouletteNotFound();
            }

            if (roulette.OpenedAt != null)
            {
                throw new NotAllowedOpenException();
            }
            roulette.OpenedAt = DateTime.Now;
            roulette.IsOpen = true;
            return rouletteRepo.Update(Id, roulette);
        }

        public Roulette Close(string Id)
        {
            Roulette roulette = rouletteRepo.GetById(Id);
            if (roulette == null)
            {
                throw new RouletteNotFound();
            }
            if (roulette.ClosedAt != null)
            {
                throw new NotAllowedClosedException();
            }
            roulette.ClosedAt = DateTime.Now;
            roulette.IsOpen = false;
            return rouletteRepo.Update(Id, roulette);
        }

        public Roulette Bet(string Id, string UserId, int position, double moneyBet)
        {
            if (moneyBet > 10000 || moneyBet < 1)
            {
                throw new CashOutRangeException();
            }
            Roulette roulette = rouletteRepo.GetById(Id);
            if (roulette == null)
            {
                throw new RouletteNotFound();
            }

            if (roulette.IsOpen == false)
            {
                throw new RouletteClosedException();
            }

            double value = 0d;
            roulette.board[position].TryGetValue(UserId, out value);
            roulette.board[position].Remove(UserId + "");
            roulette.board[position].TryAdd(UserId + "", value + moneyBet);

            return rouletteRepo.Update(roulette.Id, roulette);
        }

        public List<Roulette> GetAll()
        {
            return rouletteRepo.GetAll();
        }

        Roulette IRouletteService.create()
        {
            throw new NotImplementedException();
        }

        Roulette IRouletteService.Find(string Id)
        {
            throw new NotImplementedException();
        }

        Roulette IRouletteService.Open(string Id)
        {
            throw new NotImplementedException();
        }

        Roulette IRouletteService.Close(string Id)
        {
            throw new NotImplementedException();
        }

        Roulette IRouletteService.Bet(string Id, string UserId, int position, double money)
        {
            throw new NotImplementedException();
        }

        List<Roulette> IRouletteService.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}