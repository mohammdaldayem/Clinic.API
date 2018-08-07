using DSC.DataExecutor.DBExecutor;
using DSC.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using DSC.Entity.Reflectives;

namespace DSC.Datalayer
{
    public class Financial
    {
        private const string SP_INSEART_PATIENT_FINANCIAL = "SP_InseartPatientFinancial";
        private const string SP_LOAD_PATIENT_FINANCIAL = "SP_LoadPatientFinancial";
        private const string SP_UPDATE_PATIENT_FINANCIAL = "SP_UpdatePatientFinancial";

        public async Task<List<FinancialDto>> GetPatientFinancial(int PatientID,int EmployeeID)
        {
            List<FinancialDto> _FinancialList = new List<FinancialDto>(); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader(SP_LOAD_PATIENT_FINANCIAL, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientID", (object)PatientID ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@EmployeeID", (object)EmployeeID ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _FinancialList = objSqlDataReader.MapDataToBusinessEntityCollection<FinancialDto>();
                });
                return _FinancialList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdatePatientFinancial(int FinancialID, int TreatmentCost, int PatientCredit,int PatientDebit, DateTime FinancialDate)
        {
            try
            {
                return await DatabaseExecuter.Instance.ExecuteNonQuery(SP_UPDATE_PATIENT_FINANCIAL, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@FinancialID", (object)FinancialID ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@TreatmentCost", (object)TreatmentCost ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientCredit", (object)PatientCredit ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientDebit", (object)PatientDebit ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@FinancialDate", (object)FinancialDate ?? DBNull.Value);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> InseartPatientFinancial(int TreatmentCost, int PatientCredit, int PatientDebit, DateTime FinancialDate,string Note,int VisitID,int EmployeeID,int PatientID,int CreatedBy,DateTime CreationDate,int PaymentTypeID)
        {
            try
            {
                return await DatabaseExecuter.Instance.ExecuteNonQuery(SP_INSEART_PATIENT_FINANCIAL, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@TreatmentCost", (object)TreatmentCost ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientCredit", (object)PatientCredit ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientDebit", (object)PatientDebit ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@FinancialDate", (object)FinancialDate ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@Note", (object)Note ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@VisitID", (object)VisitID ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@EmployeeID", (object)EmployeeID ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientID", (object)PatientID ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@CreatedBy", (object)CreatedBy ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@CreationDate", (object)CreationDate ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@PaymentTypeID", (object)PaymentTypeID ?? DBNull.Value);
                });

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
