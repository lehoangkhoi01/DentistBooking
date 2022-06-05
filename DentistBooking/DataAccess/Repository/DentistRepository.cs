using BusinessObject;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class DentistRepository : IDentistRepository
    {
        public void AddNewDentist(Dentist dentist) => DentistDAO.Instance.AddNewDentist(dentist);

        public Dentist GetDentistByDentistId(int dentistId) => DentistDAO.Instance.GetDentistByDentistId(dentistId);

        public Dentist GetDentistByUserId(int userId) => DentistDAO.Instance.GetDentistByUserId(userId);

        public IEnumerable<Dentist> GetDentistList() => DentistDAO.Instance.GetDentistList();

        public void UpdateDentist(Dentist dentist) => DentistDAO.Instance.UpdateDentist(dentist);
    }
}
