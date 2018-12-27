using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPMI.Models.IM
{
    public class IMRepos
    {
        public string AutoNo(String IdDept)
        {
            try
            {
                string sql =
                    @"DECLARE @Tahun varchar(4)
                            , @Bulan varchar(2) 
                            , @seq varchar(4)
                            , @ipmi char(4)
                    SET @ipmi ='IPMI'
                    SET @Tahun = datepart(year,getdate()) 
                    SET @Bulan = DATEPART(MONTH,getdate())
                    SET @dept = 'IT'
                    SET @seq = (SELECT RIGHT('0000'+ CAST(SUBSTRING(RTRIM(MAX(noipmi)),5,4) + 1 as Varchar),4) 
                                FROM tIpmi) 
                    SELECT @ipmi + '" + IdDept + "' + COALESCE(@seq, '0001') + @Bulan + @Tahun as AutoNo ";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}