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
   public class Treatment
    {
        private const string SP_LOAD_TREATMENT_PARENT = "SP_LoadTreatmentParent";
        private const string SP_LOAD_TREATMENT = "SP_LoadTreatment";

        public async Task<List<TreatmentParentDto>> LoadTreatmentParents(bool IsDeleted)
        {
            List<TreatmentParentDto> _treatmentParentList = new List<TreatmentParentDto>(); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader(SP_LOAD_TREATMENT_PARENT, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@IsDeleted", (object)IsDeleted ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _treatmentParentList = objSqlDataReader.MapDataToBusinessEntityCollection<TreatmentParentDto>();
                });
                return _treatmentParentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TreatmentDto>> LoadTreatments(int TreatmentParentID,bool IsDeleted)
        {
            List<TreatmentDto> _treatmentList = new List<TreatmentDto>(); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader(SP_LOAD_TREATMENT, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@IsDeleted", (object)IsDeleted ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@TreatmentParentID", (object)TreatmentParentID ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _treatmentList = objSqlDataReader.MapDataToBusinessEntityCollection<TreatmentDto>();
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
