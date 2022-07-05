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
        public Task<Reservation> GetReservationById(int id);
        public Task<IEnumerable<Reservation>> GetReservations();
        public Task<IEnumerable<Reservation>> GetReservationsByCustomerId(int customerId);
        public Task<IEnumerable<Reservation>> GetReservationsByCustomerId(int page, int itemPerPage, int customerId);
        public Task<IEnumerable<Reservation>> GetReservationsByDentistId(int dentistId);
        public Task<IEnumerable<Reservation>> GetReservationsByServiceId(int serviceId);
        public Task<IEnumerable<Reservation>> GetReservationsByDateTime(DateTime dateTime);
        public Task<Reservation> GetReservationByCustomerIdAndDateTime(int customerId, DateTime dateTime);
        public Task AddNewReservation(Reservation reservation);
        public Task UpdateReservation(Reservation reservation);
        public Task DeleteReservation(Reservation reservation);
    }
}
