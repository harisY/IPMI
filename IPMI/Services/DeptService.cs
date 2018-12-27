using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPMI.Entity;
using IPMI.Repository;

namespace IPMI.Services
{
    public partial class DeptService
    {
        private GenericRepository<mDept> DeptRepository;

        public DeptService()
        {
            this.DeptRepository = new GenericRepository<mDept>(new IPMI_DBEntities());
        }

        public IEnumerable<mDept> GetAll(object[] parameters)
        {
            string spQuery = "[Get_Dept] {0}";
            return DeptRepository.ExecuteQuery(spQuery, parameters);
        }
        public IEnumerable<mDept> GetAll()
        {
            string spQuery = "[Get_Dept_All]";
            return DeptRepository.ExecuteQuery(spQuery);
        }

        public mDept GetbyID(object[] parameters)
        {
            string spQuery = "[Get_DeptbyID] {0}";
            return DeptRepository.ExecuteQuerySingle(spQuery, parameters);
        }
        public string GetDeptByID(object[] parameters)
        {
            string spQuery = "[Get_DeptbyID] {0}";
            return DeptRepository.ExecuteQuerySingle1(spQuery, parameters);
        }

        public string GetDeptByID_1(object[] parameters)
        {
            string spQuery = "[Get_DeptbyID_1] {0}";
            return DeptRepository.ExecuteQuerySingle1(spQuery, parameters);
        }
        public int Insert(object[] parameters)
        {
            string spQuery = "[Set_Dept] {0}, {1}";
            return DeptRepository.ExecuteCommand(spQuery, parameters);
        }

        public int Update(object[] parameters)
        {
            string spQuery = "[Update_Dept] {0}, {1}, {2}";
            return DeptRepository.ExecuteCommand(spQuery, parameters);
        }

        public int Delete(object[] parameters)
        {
            string spQuery = "[Delete_Dept] {0}";
            return DeptRepository.ExecuteCommand(spQuery, parameters);
        }
    }
}