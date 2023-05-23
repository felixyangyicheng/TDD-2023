using System;
using TDD.Data;

namespace TDD.Contracts
{
	public interface IReservationService
	{
		public Task<bool> Create(Reservation reservation);
		public Task<bool> Update(int id, Reservation reservation);
		public Task<bool> Delete(int id);
		public Task<Reservation> GetReservationById(int id);
		public Task<List<Reservation>> GetCurrentReservationsByAdherentCode(int code);
        public Task<List<Reservation>> GetHistoricalReservationsByAdherentCode(int code);
		public Task SendRecallMailToUser(string Code);

    }
}

