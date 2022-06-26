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
        public void AddNewReservation(Reservation reservation) => ReservationDAO.Instance.AddNewReservation(reservation);

        public Reservation GetReservationByCustomerIdAndDateTime(int customerId, DateTime dateTime) 
            => ReservationDAO.Instance.GetReservationByDateTimeAndCustomerId(customerId, dateTime);

        public Reservation GetReservationById(int id) => ReservationDAO.Instance.GetReservationById(id);

        public IEnumerable<Reservation> GetReservations() => ReservationDAO.Instance.GetReservations();

        public IEnumerable<Reservation> GetReservationsByCustomerId(int customerId) => ReservationDAO.Instance.GetReservationsByCustomerId(customerId);

        public IEnumerable<Reservation> GetReservationsByCustomerId(int page, int itemPerPage, int customerId)
            => ReservationDAO.Instance.GetReservationsByCustomerId(page, itemPerPage, customerId);

        public IEnumerable<Reservation> GetReservationsByDateTime(DateTime dateTime) => ReservationDAO.Instance.GetReservationsByDate(dateTime);

        public IEnumerable<Reservation> GetReservationsByDentistId(int dentistId) => ReservationDAO.Instance.GetReservationsByDentistId(dentistId);

        public IEnumerable<Reservation> GetReservationsByServiceId(int serviceId) => ReservationDAO.Instance.GetReservationsByServiceId(serviceId);

        public void UpdateReservation(Reservation reservation) => ReservationDAO.Instance.UpdateReservation(reservation);
    }
}
