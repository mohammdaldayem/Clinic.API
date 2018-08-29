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
    public class Prescription
    {
        private const string SP_LOAD_Prescription_PARENT = "SP_LoadPrescriptionParent";
        private const string SP_LOAD_DIAGNOSIS = "SP_LoadPrescription";

        public async Task<List<PrescriptionParentDto>> LoadPrescriptionParents (bool IsDeleted)
        {
            List<PrescriptionParentDto> _diagnosisParentList = new List<PrescriptionParentDto> (); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader (SP_LOAD_Prescription_PARENT, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue ("@IsDeleted", (object)IsDeleted ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _diagnosisParentList = objSqlDataReader.MapDataToBusinessEntityCollection<PrescriptionParentDto> ();
                });
                return _diagnosisParentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PrescriptionDto>> LoadPrescription (int DiagnosisParentID, bool IsDeleted)
        {
            List<PrescriptionDto> _treatmentList = new List<PrescriptionDto> (); ;
            try
            {
                await DatabaseExecuter.Instance.ExecuteReader (SP_LOAD_DIAGNOSIS, delegate (SqlCommand objSQLCommandGetAll)
                {
                    objSQLCommandGetAll.Parameters.AddWithValue ("@IsDeleted", (object)IsDeleted ?? DBNull.Value);
                    objSQLCommandGetAll.Parameters.AddWithValue ("@ParentPrescriptionID", (object)DiagnosisParentID ?? DBNull.Value);
                }, delegate (SqlDataReader objSqlDataReader)
                {
                    _treatmentList = objSqlDataReader.MapDataToBusinessEntityCollection<PrescriptionDto> ();
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
