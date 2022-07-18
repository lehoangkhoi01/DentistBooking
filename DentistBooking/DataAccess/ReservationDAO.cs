using BusinessObject;
using BusinessObject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ReservationDAO
    {
        private static ReservationDAO instance = null;
        private static readonly object instanceLock = new object();

        private ReservationDAO() { }
        public static ReservationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ReservationDAO();
                    }
                }
                return instance;
            }
        }
        //-------------------------------------------------

        public async Task<Reservation> GetReservationById(int id)
        {
            Reservation reservation;
            try
            {
                var dbContext = new DentistBookingContext();
                reservation = await dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer.User)
                    .Include(r => r.Dentist.User)
                    .SingleOrDefaultAsync(r => r.Id == id);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetReservations()
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = await dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer)
                    .Include(r => r.Dentist)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByCustomerId(int customerId) {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = await dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer.User)
                    .Include(r => r.Dentist.User)
                    .Where(r => r.CustomerId == customerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByCustomerId(int page, int itemPerPage, int customerId)
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = await dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer.User)
                    .Include(r => r.Dentist.User)
                    .Where(r => r.CustomerId == customerId)
                    .Skip((page - 1) * itemPerPage)
                    .Take(itemPerPage)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public async Task<Reservation> GetReservationByDateTimeAndCustomerId(int customerId, DateTime dateTime)
        {
            Reservation reservation;
            try
            {
                var dbContext = new DentistBookingContext();
                reservation = await dbContext.Reservations.SingleOrDefaultAsync(r => r.CustomerId == customerId
                                                                            && r.ResevrationDate == dateTime);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByDentistId(int dentistId)
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = await dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer)
                    .Include(r => r.Dentist)
                    .Where(r => r.DentistId == dentistId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByServiceId(int serviceId)
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = await dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer.User)
                    .Include(r => r.Dentist.User)
                    .Where(r => r.ServiceId == serviceId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByDate(DateTime dateTime)
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = await dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer.User)
                    .Include(r => r.Dentist.User)
                    .Where(r => r.ResevrationDate == dateTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public async Task AddNewReservation(Reservation reservation)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                await dbContext.Reservations.AddAsync(reservation);
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateReservation(Reservation reservation)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Entry<Reservation>(reservation).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            try { 
                var dbContext = new DentistBookingContext();
                dbContext.Reservations.Remove(reservation);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
