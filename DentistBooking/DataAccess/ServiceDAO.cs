using BusinessObject;
using BusinessObject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ServiceDAO
    {
        private static ServiceDAO instance = null;
        private static readonly object instanceLock = new object();

        private ServiceDAO() { }
        public static ServiceDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceDAO();
                    }
                }
                return instance;
            }
        }
        //-----------------------------------------------------------------

        public void AddNewService(Service service)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Services.AddAsync(service);
                dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Service GetServiceById(int id)
        {
            Service service;
            try
            {
                var dbContext = new DentistBookingContext();
                service = dbContext.Services.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return service;
        }

        public Service GetServiceByName(string name)
        {
            Service service;
            try
            {
                var dbContext = new DentistBookingContext();
                service = dbContext.Services.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(name.ToLower().Trim()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return service;
        }

        public void UpdateService(Service service)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Services.Update(service);
                dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Service> GetServiceList()
        {
            IEnumerable<Service> serviceList;
            try
            {
                var dbContext = new DentistBookingContext();
                serviceList = dbContext.Services.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return serviceList;
        }
    }
}
