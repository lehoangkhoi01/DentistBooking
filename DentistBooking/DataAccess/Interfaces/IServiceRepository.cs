using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IServiceRepository
    {
        public void AddNewService(Service service);
        public void UpdateService(Service service);
        public IEnumerable<Service> GetServiceList();
        public IEnumerable<Service> GetServiceListByPage(int page, int itemPerPage);
        public Service GetServiceById(int id);
        public Service GetServiceByName(string name);   

    }
}
