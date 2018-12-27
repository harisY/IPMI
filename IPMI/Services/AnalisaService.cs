using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPMI.Entity;
using IPMI.Repository;
namespace IPMI.Services
{
    public partial class AnalisaService
    {
        private GenericRepository<tIpmiAnalisa> AnalisaRepository;
        private GenericRepository<tIpmi> IpmiRepository;

        public AnalisaService()
        {
            this.AnalisaRepository = new GenericRepository<tIpmiAnalisa>(new IPMI_DBEntities());
            this.IpmiRepository = new GenericRepository<tIpmi>(new IPMI_DBEntities());
        }

        public IEnumerable<tIpmiAnalisa> GetAll()
        {
            string spQuery = "[Get_Analisa_Global]";
            return AnalisaRepository.ExecuteQuery(spQuery);
        }
        public IEnumerable<tIpmi> GetAll1()
        {
            string spQuery = "[Get_Analisa_Global]";
            return IpmiRepository.ExecuteQuery(spQuery);
        }
        public IEnumerable<tIpmi> GetAll(object[] parameters)
        {
            string spQuery = "[Get_Analisa] {0}";
            return IpmiRepository.ExecuteQuery(spQuery, parameters);
        }

        public IEnumerable<tIpmi> GetAll_OnProgress(object[] parameters)
        {
            string spQuery = "[Get_Analisa_OnProgress] {0}";
            return IpmiRepository.ExecuteQuery(spQuery, parameters);
        }
        public IEnumerable<tIpmi> GetAll_Closed(object[] parameters)
        {
            string spQuery = "[Get_Analisa_Closed] {0}";
            return IpmiRepository.ExecuteQuery(spQuery, parameters);
        }
        public IEnumerable<tIpmiAnalisa> GetIpmiClosed(object[] parameters)
        {
            string spQuery = "[Get_Analisa_Closed] {0}";
            return AnalisaRepository.ExecuteQuery(spQuery, parameters);
        }
        public tIpmiAnalisa GetbyID(object[] parameters)
        {
            string spQuery = "[Get_AnalisaByID] {0}";
            return AnalisaRepository.ExecuteQuerySingle(spQuery, parameters);
        }

        public int Insert(object[] parameters)
        {
            string spQuery = "[Set_Analisa] {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return AnalisaRepository.ExecuteCommand(spQuery, parameters);
        }
        public int Update(object[] parameters)
        {
            string spQuery = "[Update_Analisa]{0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return AnalisaRepository.ExecuteCommand(spQuery, parameters);
        }
        public int UpdateStatusIpmi(object[] parameters)
        {
            string spQuery = "[Update_Status_Analisa]{0},{1}";
            return AnalisaRepository.ExecuteCommand(spQuery, parameters);
        }

        public int Delete(object[] parameters)
        {
            string spQuery = "[Delete_Analisa] {0}";
            return AnalisaRepository.ExecuteCommand(spQuery, parameters);
        }
        public string AutoNo(object[] parameters)
        {
            string spQuery = "[Get_AutoNo] {0}";
            return AnalisaRepository.ExecuteQuerySingle1(spQuery, parameters);
        }
    }
}