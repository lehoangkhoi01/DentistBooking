using BusinessObject;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        public Task AddNewReservation(Reservation reservation) => ReservationDAO.Instance.AddNewReservation(reservation);

        public Task DeleteReservation(Reservation reservation) => ReservationDAO.Instance.DeleteReservation(reservation);

        public Task<Reservation> GetReservationByCustomerIdAndDateTime(int customerId, DateTime dateTime) 
            => ReservationDAO.Instance.GetReservationByDateTimeAndCustomerId(customerId, dateTime);

        public Task<Reservation> GetReservationById(int id) => ReservationDAO.Instance.GetReservationById(id);

        public Task<IEnumerable<Reservation>> GetReservations() => ReservationDAO.Instance.GetReservations();

        public Task<IEnumerable<Reservation>> GetReservationsByCustomerId(int customerId) => ReservationDAO.Instance.GetReservationsByCustomerId(customerId);

        public Task<IEnumerable<Reservation>> GetReservationsByCustomerId(int page, int itemPerPage, int customerId)
            => ReservationDAO.Instance.GetReservationsByCustomerId(page, itemPerPage, customerId);

        public Task<IEnumerable<Reservation>> GetReservationsByDateTime(DateTime dateTime) => ReservationDAO.Instance.GetReservationsByDate(dateTime);

        public Task<IEnumerable<Reservation>> GetReservationsByDentistId(int dentistId) => ReservationDAO.Instance.GetReservationsByDentistId(dentistId);

        public Task<IEnumerable<Reservation>> GetReservationsByServiceId(int serviceId) => ReservationDAO.Instance.GetReservationsByServiceId(serviceId);

        public Task UpdateReservation(Reservation reservation) => ReservationDAO.Instance.UpdateReservation(reservation);
    }
}
