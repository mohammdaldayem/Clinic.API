using DSC.DataExecutor.DBExecutor;
using DSC.Entity;
using DSC.Entity.Reflectives;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DSC.Datalayer
{
   public class Diagnosis
    {
        private const string SP_LOAD_DIAGNOSIS_PARENT = "SP_LoadDiagnosisParent";
        private const string SP_LOAD_DIAGNOSIS = "SP_LoadDiagnosis";

        public async Task<List<DiagnosisParentDto>> LoadDiagnosisParents(bool IsDeleted)
        {
            List<DiagnosisParentDto> _diagnosisParentList = new List<DiagnosisParentDto> (); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader(SP_LOAD_DIAGNOSIS_PARENT, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@IsDeleted", (object)IsDeleted ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _diagnosisParentList = objSqlDataReader.MapDataToBusinessEntityCollection<DiagnosisParentDto>();
                });
                return _diagnosisParentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DiagnosisDto>> LoadDiagnosis(int DiagnosisParentID, bool IsDeleted)
        {
            List<DiagnosisDto> _treatmentList = new List<DiagnosisDto> (); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader(SP_LOAD_DIAGNOSIS, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@IsDeleted", (object)IsDeleted ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@ParentDiagnosisID", (object)DiagnosisParentID ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _treatmentList = objSqlDataReader.MapDataToBusinessEntityCollection<DiagnosisDto> ();
                });
                return _treatmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
