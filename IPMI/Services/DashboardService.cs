using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPMI.Entity;
using IPMI.Repository;
using IPMI.Models;
namespace IPMI.Services
{
    public partial class DashboardService
    {
        private GenericRepository<DashboardModels> DashboardRepository;
        private GenericRepository<Pie> PieReporsitory;
        private GenericRepository<BarChart> BarChartReporsitory;
        public DashboardService()
        {
            this.DashboardRepository = new GenericRepository<DashboardModels>(new IPMI_DBEntities());
            this.PieReporsitory = new GenericRepository<Pie>(new IPMI_DBEntities());
            this.BarChartReporsitory = new GenericRepository<BarChart>(new IPMI_DBEntities());
        }

        public IEnumerable<Pie> GetAll()
        {
            string spQuery = "[Dasboard_Pie_1]";
            return PieReporsitory.ExecuteQuery(spQuery);
        }
        public IEnumerable<DashboardModels> GetAll(object[] parameters)
        {
            string spQuery = "[Dasboard] {0}";
            return DashboardRepository.ExecuteQuery(spQuery, parameters);
        }
        public DashboardModels GetbyID(object[] parameters)
        {
            string spQuery = "[Get_AnalisaByID] {0}";
            return DashboardRepository.ExecuteQuerySingle(spQuery, parameters);
        }
        public Pie GetPie(object[] parameters)
        {
            string spQuery = "[Dasboard_Pie_1] {0}";
            return PieReporsitory.ExecuteQuerySingle(spQuery, parameters);
        }
        public IEnumerable<BarChart> GetBarChart()
        {
            string spQuery = "[Dasboard_BarChart]";
            return BarChartReporsitory.ExecuteQuery(spQuery);
        }

    }
}