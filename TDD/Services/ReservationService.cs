using System;
using Microsoft.EntityFrameworkCore;
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
        private readonly BuDbContext _db;

        public ReservationService(
            BuDbContext db
            )
        {
            _db = db;
        }
        public async Task<bool> Create(Reservation reservation)
        {
            _db.Reservations.Add(reservation);
            return await Save();

        }

        public async Task<bool> Delete(int id)
        {
            var res = await _db.Reservations.FirstOrDefaultAsync(a => a.Id == id);
            if (res != null)
            {

                _db.Reservations.Remove(res);
            }
            return await Save();
        }

        public async Task<List<Reservation>> GetCurrentReservationsByAdherentCode(string code)
        {
            return await _db.Reservations.Where(a => a.AdherentCode == code)
                .Where(r => r.DateFin > DateTime.Now.AddMonths(-4)).ToListAsync();
        }

        public async Task<List<Reservation>> GetHistoricalReservationsByAdherentCode(string code)
        {
            return await _db.Reservations.Where(a => a.AdherentCode == code).ToListAsync();

        }

        public async Task<Reservation> GetReservationById(int id)
        {
            return await _db.Reservations.FirstOrDefaultAsync(a=>a.Id==id);

        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public Task SendRecallMailToUser(string Code)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetEnd(int id)
        {
            var res =await _db.Reservations.FirstOrDefaultAsync(a => a.Id == id);
            res.DateFin = DateTime.Now;
            return await Save();
        }

        public async Task<bool> Update(int id, Reservation reservation)
        {
            var origine = await _db.Reservations.FirstOrDefaultAsync(a => a.Id == reservation.Id);
            if (origine != null)
            {

                _db.Reservations.Update(reservation);
            }
            return await Save();
        }
    }
}

