﻿using BusinessObject;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        public void AddNewService(Service service) => ServiceDAO.Instance.AddNewService(service);

        public Service GetServiceById(int id) => ServiceDAO.Instance.GetServiceById(id);

        public Service GetServiceByName(string name) => ServiceDAO.Instance.GetServiceByName(name);

        public IEnumerable<Service> GetServiceList() => ServiceDAO.Instance.GetServiceList();

        public void UpdateService(Service service) => ServiceDAO.Instance.UpdateService(service);
    }
}
