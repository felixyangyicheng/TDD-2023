using System;
using TDD.Contracts;
using TDD.Data;

namespace TDD.Services
{
    public class ReservationService : IReservationService
    {
        private IReservationService _rese;
        public ReservationService(IReservationService rese)
        {
            this._rese = rese;
        }
        public Task<bool> Create(Reservation reservation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reservation>> GetCurrentReservationsByAdherentCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reservation>> GetHistoricalReservationsByAdherentCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<Reservation> GetReservationById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public Task SendRecallMailToUser(string Code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetEnd(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, Reservation reservation)
        {
            throw new NotImplementedException();
        }
    }
}

