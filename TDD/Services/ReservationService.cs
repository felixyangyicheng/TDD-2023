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
            await Save();
            var livre=await _db.Livres.FirstOrDefaultAsync(a => a.Isbn == reservation.Isbn);

            try
            {
                livre.Disponible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
          

            _db.Livres.Update(livre);


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

            try
            {
            var changes = await _db.SaveChangesAsync();

            return changes > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

                var rm= await  _db.Reservations.FirstOrDefaultAsync(a => a.Id == id);
                _db.Reservations.Remove(rm);
                _db.Reservations.Add(reservation);
     
                         
            
            return await Save();
        }
    }
}

