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

        public Reservation GetReservationById(int id)
        {
            Reservation reservation;
            try
            {
                var dbContext = new DentistBookingContext();
                reservation = dbContext.Reservations.SingleOrDefault(r => r.Id == id);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservation;
        }

        public IEnumerable<Reservation> GetReservations()
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer)
                    .Include(r => r.Dentist)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public IEnumerable<Reservation> GetReservationsByCustomerId(int customerId) {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer)
                    .Include(r => r.Dentist)
                    .Where(r => r.CustomerId == customerId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public Reservation GetReservationByDateTimeAndCustomerId(int customerId, DateTime dateTime)
        {
            Reservation reservation;
            try
            {
                var dbContext = new DentistBookingContext();
                reservation = dbContext.Reservations.SingleOrDefault(r => r.CustomerId == customerId
                                                                            && r.ResevrationDate == dateTime);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservation;
        }

        public IEnumerable<Reservation> GetReservationsByDentistId(int dentistId)
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer)
                    .Include(r => r.Dentist)
                    .Where(r => r.DentistId == dentistId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public IEnumerable<Reservation> GetReservationsByServiceId(int serviceId)
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer)
                    .Include(r => r.Dentist)
                    .Where(r => r.ServiceId == serviceId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public IEnumerable<Reservation> GetReservationsByDate(DateTime dateTime)
        {
            IEnumerable<Reservation> reservations;
            try
            {
                var dbContext = new DentistBookingContext();
                reservations = dbContext.Reservations
                    .Include(r => r.Service)
                    .Include(r => r.Customer)
                    .Include(r => r.Dentist)
                    .Where(r => r.ResevrationDate == dateTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reservations;
        }

        public void AddNewReservation(Reservation reservation)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Reservations.Add(reservation);
                dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateReservation(Reservation reservation)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Entry<Reservation>(reservation).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
