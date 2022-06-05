using BusinessObject;
using BusinessObject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DentistDAO
    {
        private static DentistDAO instance = null;
        private static readonly object instanceLock = new object();

        private DentistDAO() { }
        public static DentistDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DentistDAO();
                    }
                }
                return instance;
            }
        }
        //---------------------------------------------------------
        public void AddNewDentist(Dentist dentist)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Customers.AddAsync(dentist);
                dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Dentist GetDentistByUserId(int id)
        {
            Dentist dentist;
            try
            {
                var dbContext = new DentistBookingContext();
                dentist = dbContext.Dentists.FirstOrDefault(x => x.UserId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dentist;
        }

        public Dentist GetDentistByDentistId(int id)
        {
            Dentist dentist;
            try
            {
                var dbContext = new DentistBookingContext();
                dentist = dbContext.Dentists.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dentist;
        }

        public void UpdateDentist(Dentist dentist)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Dentists.Update(dentist);
                dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Dentist> GetDentistList()
        {
            IEnumerable<Dentist> dentistList;
            try
            {
                var dbContext = new DentistBookingContext();
                dentistList = dbContext.Dentists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dentistList;
        }

    }
}
