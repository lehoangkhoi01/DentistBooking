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
                dbContext.Services.Add(service);
                dbContext.SaveChanges();
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
                service = dbContext.Services
                                    .Include(s => s.Admin)
                                    .FirstOrDefault(x => x.Id == id);
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
                service = dbContext.Services
                    .Include(s => s.Admin)
                    .FirstOrDefault(x => x.Name.ToLower().Trim().Equals(name.ToLower().Trim()));
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
                if(!CheckDuplicateServiceName(service))
                {
                    throw new Exception("This service is already exsited");
                }
                else
                {
                    dbContext.Entry<Service>(service).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Service> GetActiveServiceList()
        {
            IEnumerable<Service> serviceList;
            try
            {
                var dbContext = new DentistBookingContext();
                serviceList = dbContext.Services
                    .Include(s => s.Admin)
                    .Where(s => s.Status == "Active")
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return serviceList;
        }

        public IEnumerable<Service> GetServiceList()
        {
            IEnumerable<Service> serviceList;
            try
            {
                var dbContext = new DentistBookingContext();
                serviceList = dbContext.Services
                    .Include(s => s.Admin)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return serviceList;
        }

        public IEnumerable<Service> GetServiceListByPage(int page, int itemPerPage)
        {
            IEnumerable<Service> serviceList;
            try
            {
                var dbContext = new DentistBookingContext();
                serviceList = dbContext.Services
                    .Include(s => s.Admin)
                    .Skip((page - 1) * itemPerPage)
                    .Take(itemPerPage)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return serviceList;
        }

        private bool CheckDuplicateServiceName(Service service)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                List<Service> serviceList = dbContext.Services.ToList();
                serviceList.RemoveAll(s => s.Id == service.Id);
                if(serviceList.Exists(s => s.Name.ToLower().Trim() == service.Name.ToLower().Trim()))
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
    }
}
