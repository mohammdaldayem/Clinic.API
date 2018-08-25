using DSC.DataExecutor.DBExecutor;
using DSC.Entity;
using DSC.Entity.Reflectives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DSC.Datalayer
{
    public class Patient
    {
        private const string SP_GET_PATIENT_INFO= "GetPatientInfo";
        private const string SP_GET_PATIENT_VISITS = "SP_GetPatientVisits";
        private const string SP_GET_PATIENT_IMAGES = "SP_GetPatientImages";
        private const string SP_INSEART_PATIENT_IMAGE = "SP_InseartPatientImage";
        #region TreatmentSP
        private const string SP_GET_PATIENT_TREATMENT = "SP_GetPatientTreatment";
        private const string SP_UPDATE_PATIENT_TREATMENT = "SP_UpdatePatientTreatment";
        #endregion

        public async Task<List<PatientDto>> GetPatientInfo(DateTime? FromDate, DateTime? ToDate,int? EmployeeID,string PatientName,int? PatientID)
        {
            List<PatientDto> _PatientList = new List<PatientDto>(); ;
            try
            {
                 await DatabaseExecuter.Instance.ExecuteReader(SP_GET_PATIENT_INFO,delegate(SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@FromDate", (object)FromDate ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@ToDate", (object)ToDate ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@EmployeeID", (object)EmployeeID ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientName", (object)PatientName ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientID", (object)PatientID ?? DBNull.Value);
                },delegate(SqlDataReader objSqlDataReader)
                {
                    _PatientList = objSqlDataReader.MapDataToBusinessEntityCollection<PatientDto>();
                });
                return _PatientList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<VisitDto>> GetPatientVisits(int? PatientID)
        {
            List<VisitDto> _VisitList = new List<VisitDto>(); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader(SP_GET_PATIENT_VISITS, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientID", (object)PatientID ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _VisitList = objSqlDataReader.MapDataToBusinessEntityCollection<VisitDto>();
                });
                return _VisitList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PatientImageDto>> GetPatientImages(int PatientID)
        {
            List<PatientImageDto> _ImageList = new List<PatientImageDto>(); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader(SP_GET_PATIENT_IMAGES, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientID", (object)PatientID ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _ImageList = objSqlDataReader.MapDataToBusinessEntityCollection<PatientImageDto>();
                });
                return _ImageList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> InseartPatientImage(int PatientID,Byte[] ImageData,string ImageName,string Note)
        {
            try
            {
                return await DatabaseExecuter.Instance.ExecuteNonQuery(SP_INSEART_PATIENT_IMAGE, delegate (SqlCommand objSQLCommandGetAll)
                 {
                     objSQLCommandGetAll.Parameters.AddWithValue("@PatientID", (object)PatientID ?? DBNull.Value);
                     objSQLCommandGetAll.Parameters.AddWithValue("@ImageData", (object)ImageData ?? DBNull.Value);
                     objSQLCommandGetAll.Parameters.AddWithValue("@ImageName", (object)ImageName ?? DBNull.Value);
                     objSQLCommandGetAll.Parameters.AddWithValue("@Note", (object)Note ?? DBNull.Value);
                 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Treatment
        public async Task<string> GetPatientTreatment(int PatientID,int VisiteID)
        {
            string Treatment = string.Empty;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader(SP_GET_PATIENT_TREATMENT, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@PatientID", (object)PatientID ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@VisitID", (object)VisiteID ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    while (objSqlDataReader.Read())
                    {
                        Treatment = objSqlDataReader["Treatment"].ToString();
                    }
                });
                return Treatment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdatePatientTreatment(int PatientID, int Visitid, string Treatment, string TreatmentText)
        {
            try
            {
                return await DatabaseExecuter.Instance.ExecuteNonQuery(SP_UPDATE_PATIENT_TREATMENT, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue("@patientid", (object)PatientID ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@visitid", (object)Visitid ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@Treatment", (object)Treatment ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue("@TreatmentText", value: (object)TreatmentText ?? DBNull.Value);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
