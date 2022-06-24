using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IReservationRepository
    {
        public Reservation GetReservationById(int id);
        public IEnumerable<Reservation> GetReservations();
        public IEnumerable<Reservation> GetReservationsByCustomerId(int customerId);
        public IEnumerable<Reservation> GetReservationsByDentistId(int dentistId);
        public IEnumerable<Reservation> GetReservationsByServiceId(int serviceId);
        public IEnumerable<Reservation> GetReservationsByDateTime(DateTime dateTime);
        public Reservation GetReservationByCustomerIdAndDateTime(int customerId, DateTime dateTime);
        public void AddNewReservation(Reservation reservation);
        public void UpdateReservation(Reservation reservation);
    }
}
