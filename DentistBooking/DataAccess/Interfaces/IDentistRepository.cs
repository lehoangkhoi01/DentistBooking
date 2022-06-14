using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IDentistRepository
    {
        public void AddNewDentist(Dentist dentist);
        public Dentist GetDentistByDentistId(int dentistId);
        public Dentist GetDentistByPhone(string phone);
        public Dentist GetDentistByUserId(int userId);
        public void UpdateDentist(Dentist dentist);
        public IEnumerable<Dentist> GetDentistList();
    }
}
