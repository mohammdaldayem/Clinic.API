// ***********************************************************************
// Assembly         : Akeed.DatabaseExecuter
// Author           : Anas
// Created          : 09-22-2015
//
// Last Modified By : Anas
// Last Modified On : 10-05-2015
// ***********************************************************************
// <copyright file="DatabaseExecuter.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using DSC.DataExecutor;

/// <summary>
/// The DatabaseManager namespace.
/// </summary>
namespace DSC.DataExecutor.DBExecutor
{

    /// <summary>
    /// Class ErrorResponse.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Class OutputParametersResponse.
    /// </summary>
    public class OutputParametersResponse : ErrorResponse
    {
        public OutputParametersResponse()
        {
            ParameterValues = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the parameter values.
        /// </summary>
        /// <value>The parameter values.</value>
        public Dictionary<string, object> ParameterValues { get; set; }

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        public object GetParameterValue(string key)
        {
            object paramValue;
            ParameterValues.TryGetValue(key, out paramValue);
            return paramValue;
        }
    }

    /// <summary>
    /// Class DatabaseExecuter.
    /// </summary>
    public class DatabaseExecuter
    {
        #region Variables

        /// <summary>
        /// The connectio n_ string
        /// </summary>
        readonly protected string CONNECTION_STRING = new AppConfiguration().SqlDataConnection;
        //readonly protected string CONNECTION_STRING=
        //    "Data Source=LENOVO-PC\\SQLEXPRESS;Initial Catalog=AkeedDevelopment;"
        //   + "Integrated Security=true";
        /// <summary>
        /// The _instance
        /// </summary>
        private static DatabaseExecuter _instance;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static DatabaseExecuter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseExecuter();



                return _instance;
            }

        }

        #endregion

        #region Actions

        /// <summary>
        /// Excecutes the non query.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="FillCommandParams">The fill command parameters.</param>
        /// <returns>Task.</returns>
        /// <exception cref="WaselniException">
        /// </exception>
        public async Task<int> ExecuteNonQuery(string storedProcedureName, Action<SqlCommand> FillCommandParams)
        {
            int _rowEffected = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    FillCommandParams(cmd);
                    await conn.OpenAsync();
                    _rowEffected = await cmd.ExecuteNonQueryAsync();
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
               
            }
            return _rowEffected;
        }

        /// <summary>
        /// This function to excute commands with output error message.
        /// </summary>
        /// <param name="storedProcedureName">stored procedure name.</param>
        /// <param name="FillCommandParam">command parameters.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="OutputParametersResponse">The output parameters response.</param>
        /// <returns>response with error message</returns>
        /// <exception cref="WaselniException">
        /// </exception>
        public async Task ExecuteNonQueryWithOutputParameters(string storedProcedureName, Action<SqlCommand> FillCommandParam, string[] parameters, Action<OutputParametersResponse> OutputParametersResponse)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    SqlParameter errorMessage = new SqlParameter();
                    errorMessage.ParameterName = "@ErrorMessage";
                    errorMessage.DbType = DbType.String;
                    errorMessage.Direction = ParameterDirection.Output;
                    errorMessage.Size = 250;
                    cmd.Parameters.Add(errorMessage);

                    SqlParameter errorCode = new SqlParameter();
                    errorCode.ParameterName = "@ErrorCode";
                    errorCode.DbType = DbType.String;
                    errorCode.Size = 100;
                    errorCode.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(errorCode);

                    FillCommandParam(cmd);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    OutputParametersResponse response = new OutputParametersResponse();
                    foreach (var item in parameters)
                    {
                        response.ParameterValues.Add(item, cmd.Parameters["@" + item].Value);
                    }
                    if (cmd.Parameters["@ErrorCode"].Value.ToString() != "0" && cmd.Parameters["@ErrorCode"].Value.ToString() != "")
                    {
                        response.ErrorMessage = cmd.Parameters["@ErrorMessage"].Value.ToString();
                        response.ErrorCode = cmd.Parameters["@ErrorCode"].Value.ToString();
                    }
                    else
                    {
                        response.ErrorCode = "0";
                        response.ErrorMessage = null;
                    }
                    OutputParametersResponse(response);
                    conn.Close();
                }
            }
            //catch (WaselniException)
            //{
            //    throw;
            //}
            //catch (SqlException ex)
            //{
            //    throw new WaselniException()
            //    {
            //        OriginalErrorCode = ErrorCode.DatabaseError.ToString(),
            //        ErrorCode = ErrorCode.GeneralError.ToString("D"),
            //        OriginalMessage = string.Format("Message:{0}\tErrorCode:{1}", ex.Message, ex.ErrorCode),
            //        StackTrace = ex.StackTrace
            //    };
            //}
            catch (Exception ex)
            {
                throw ex;
                //throw new WaselniException()
                //{
                //    OriginalErrorCode = ErrorCode.GeneralError.ToString("D").ToString(),
                //    ErrorCode = ErrorCode.GeneralError.ToString("D"),
                //    OriginalMessage = ex.Message,
                //    StackTrace = ex.StackTrace
                //};
            }
        }


        public async Task ExecuteNonQueryWithOutputParameters(string storedProcedureName, Action<SqlCommand> FillCommandParam, Action<OutputParametersResponse> OutputParametersResponse)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    SqlParameter errorMessage = new SqlParameter();
                    errorMessage.ParameterName = "@ErrorMessage";
                    errorMessage.DbType = DbType.String;
                    errorMessage.Direction = ParameterDirection.Output;
                    errorMessage.Size = 250;
                    cmd.Parameters.Add(errorMessage);

                    SqlParameter errorCode = new SqlParameter();
                    errorCode.ParameterName = "@ErrorCode";
                    errorCode.DbType = DbType.String;
                    errorCode.Size = 100;
                    errorCode.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(errorCode);

                    FillCommandParam(cmd);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    OutputParametersResponse response = new OutputParametersResponse();
                    if (cmd.Parameters["@ErrorCode"].Value.ToString() != "0" && cmd.Parameters["@ErrorCode"].Value.ToString() != "")
                    {
                        response.ErrorMessage = cmd.Parameters["@ErrorMessage"].Value.ToString();
                        response.ErrorCode = cmd.Parameters["@ErrorCode"].Value.ToString();
                    }
                    else
                    {
                        response.ErrorCode = "0";
                        response.ErrorMessage = null;
                    }
                    OutputParametersResponse(response);
                    conn.Close();
                }
            }
            //catch (WaselniException)
            //{
            //    throw;
            //}
            //catch (SqlException ex)
            //{
            //    throw new WaselniException()
            //    {
            //        OriginalErrorCode = ErrorCode.DatabaseError.ToString(),
            //        ErrorCode = ErrorCode.GeneralError.ToString("D"),
            //        OriginalMessage = string.Format("Message:{0}\tErrorCode:{1}", ex.Message, ex.ErrorCode),
            //        StackTrace = ex.StackTrace
            //    };
            //}
            catch (Exception ex)
            {
                throw ex;
                //throw new WaselniException()
                //{
                //    OriginalErrorCode = ErrorCode.GeneralError.ToString("D").ToString(),
                //    ErrorCode = ErrorCode.GeneralError.ToString("D"),
                //    OriginalMessage = ex.Message,
                //    StackTrace = ex.StackTrace
                //};
            }
        }

        /// <summary>
        /// Excecutes the scaler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="FillCommandParams">The fill command parameters.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        /// <exception cref="WaselniException">
        /// </exception>
        public async Task<T> ExcecuteScaler<T>(string storedProcedureName, Action<SqlCommand> FillCommandParams)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    FillCommandParams(cmd);
                    await conn.OpenAsync();
                    var result = (T)Convert.ChangeType(await cmd.ExecuteScalarAsync(), typeof(T));
                    conn.Close();

                    return result;

                }
            }
            //catch (WaselniException)
            //{
            //    throw;
            //}
            //catch (SqlException ex)
            //{
            //    throw new WaselniException()
            //    {
            //        OriginalErrorCode = ErrorCode.DatabaseError.ToString(),
            //        ErrorCode = ErrorCode.GeneralError.ToString("D"),
            //        OriginalMessage = string.Format("Message:{0}\tErrorCode:{1}", ex.Message, ex.ErrorCode),
            //        StackTrace = ex.StackTrace
            //    };
            //}
            catch (Exception ex)
            {
                throw ex;
                //throw new WaselniException()
                //{
                //    OriginalErrorCode = ErrorCode.GeneralError.ToString("D").ToString(),
                //    ErrorCode = ErrorCode.GeneralError.ToString("D"),
                //    OriginalMessage = ex.Message,
                //    StackTrace = ex.StackTrace
                //};
            }
        }


        /// <summary>
        /// Excecutes the reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="FillCommandParam">The fill command parameters.</param>
        /// <param name="FetchReader">The fetch reader.</param>
        /// <returns>Task.</returns>
        /// <exception cref="WaselniException">
        /// </exception>
        public async Task ExecuteReaderWithOutputParameters(string storedProcedureName, Action<SqlCommand> FillCommandParam
            , Action<SqlDataReader> FetchReader, string[] parameters, Action<OutputParametersResponse> OutputParametersResponse)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    SqlParameter errorMessage = new SqlParameter();
                    errorMessage.ParameterName = "@ErrorMessage";
                    errorMessage.DbType = DbType.String;
                    errorMessage.Direction = ParameterDirection.Output;
                    errorMessage.Size = 250;
                    cmd.Parameters.Add(errorMessage);

                    SqlParameter errorCode = new SqlParameter();
                    errorCode.ParameterName = "@ErrorCode";
                    errorCode.DbType = DbType.String;
                    errorCode.Size = 100;
                    errorCode.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(errorCode);
                    FillCommandParam(cmd);
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        FetchReader(reader);
                    }

                    conn.Close();
                    OutputParametersResponse response = new OutputParametersResponse();
                    foreach (var item in parameters)
                    {
                        response.ParameterValues.Add(item, cmd.Parameters["@" + item].Value);
                    }

                    if (cmd.Parameters["@ErrorCode"].Value.ToString() != "0" && cmd.Parameters["@ErrorCode"].Value.ToString() != "")
                    {
                        response.ErrorMessage = cmd.Parameters["@ErrorMessage"].Value.ToString();
                        response.ErrorCode = cmd.Parameters["@ErrorCode"].Value.ToString();
                    }
                    else
                    {
                        response.ErrorCode = "0";
                        response.ErrorMessage = null;
                    }
                    OutputParametersResponse(response);
                }
            }
            //catch (WaselniException)
            //{
            //    throw;
            //}
            //catch (SqlException ex)
            //{
            //    throw new WaselniException()
            //    {
            //        OriginalErrorCode = ErrorCode.DatabaseError.ToString(),
            //        ErrorCode = ErrorCode.GeneralError.ToString("D"),
            //        OriginalMessage = string.Format("Message:{0}\tErrorCode:{1}", ex.Message, ex.ErrorCode),
            //        StackTrace = ex.StackTrace
            //    };
            //}
            catch (Exception ex)
            {
                throw ex;
                //throw new WaselniException()
                //{
                //    OriginalErrorCode = ErrorCode.GeneralError.ToString("D").ToString(),
                //    ErrorCode = ErrorCode.GeneralError.ToString("D"),
                //    OriginalMessage = ex.Message,
                //    StackTrace = ex.StackTrace
                //};
            }
        }

        public async Task ExecuteReaderWithOutputParameters(string storedProcedureName, Action<SqlCommand> FillCommandParam
          , Action<SqlDataReader> FetchReader, Action<OutputParametersResponse> OutputParametersResponse)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;

                    SqlParameter errorMessage = new SqlParameter();
                    errorMessage.ParameterName = "@ErrorMessage";
                    errorMessage.DbType = DbType.String;
                    errorMessage.Direction = ParameterDirection.Output;
                    errorMessage.Size = 250;
                    cmd.Parameters.Add(errorMessage);

                    SqlParameter errorCode = new SqlParameter();
                    errorCode.ParameterName = "@ErrorCode";
                    errorCode.DbType = DbType.String;
                    errorCode.Size = 100;
                    errorCode.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(errorCode);
                    FillCommandParam(cmd);
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        FetchReader(reader);
                    }

                    conn.Close();
                    OutputParametersResponse response = new OutputParametersResponse();

                    if (cmd.Parameters["@ErrorCode"].Value.ToString() != "0" && cmd.Parameters["@ErrorCode"].Value.ToString() != "")
                    {
                        response.ErrorMessage = cmd.Parameters["@ErrorMessage"].Value.ToString();
                        response.ErrorCode = cmd.Parameters["@ErrorCode"].Value.ToString();
                    }
                    else
                    {
                        response.ErrorCode = "0";
                        response.ErrorMessage = null;
                    }
                    OutputParametersResponse(response);
                }
            }
            //catch (WaselniException)
            //{
            //    throw;
            //}
            //catch (SqlException ex)
            //{
            //    throw new WaselniException()
            //    {
            //        OriginalErrorCode = ErrorCode.DatabaseError.ToString(),
            //        ErrorCode = ErrorCode.GeneralError.ToString("D"),
            //        OriginalMessage = string.Format("Message:{0}\tErrorCode:{1}", ex.Message, ex.ErrorCode),
            //        StackTrace = ex.StackTrace
            //    };
            //}
            catch (Exception ex)
            {
                throw ex;
                //throw new WaselniException()
                //{
                //    OriginalErrorCode = ErrorCode.GeneralError.ToString("D").ToString(),
                //    ErrorCode = ErrorCode.GeneralError.ToString("D"),
                //    OriginalMessage = ex.Message,
                //    StackTrace = ex.StackTrace
                //};
            }
        }

        /// <summary>
        /// Excecutes the reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="FillCommandParams">The fill command parameters.</param>
        /// <param name="FetchReader">The fetch reader.</param>
        /// <returns>Task.</returns>
        /// <exception cref="WaselniException">
        /// </exception>
        public async Task ExecuteReader(string storedProcedureName, Action<SqlCommand> FillCommandParams
            , Action<SqlDataReader> FetchReader)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    FillCommandParams(cmd);
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        FetchReader(reader);
                    }


                    //    OutputParametersResponse(response);
                    conn.Close();
                }
            }
            //catch (WaselniException)
            //{
            //    throw;
            //}
            //catch (SqlException ex)
            //{
            //    throw new WaselniException()
            //    {
            //        OriginalErrorCode = ErrorCode.DatabaseError.ToString(),
            //        ErrorCode = ErrorCode.GeneralError.ToString("D"),
            //        OriginalMessage = string.Format("Message:{0}\tErrorCode:{1}", ex.Message, ex.ErrorCode),
            //        StackTrace = ex.StackTrace
            //    };
            //}
            catch (Exception ex)
            {
                throw ex;
                //throw new WaselniException()
                //{
                //    OriginalErrorCode = ErrorCode.GeneralError.ToString("D").ToString(),
                //    ErrorCode = ErrorCode.GeneralError.ToString("D"),
                //    OriginalMessage = ex.Message,
                //    StackTrace = ex.StackTrace
                //};
            }
        }

        /// <summary>
        /// Excecutes the specified process command.
        /// </summary>
        /// <param name="ProcessCommand">The process command.</param>
        /// <returns>Task.</returns>
        /// <exception cref="WaselniException">
        /// </exception>
        public async Task Excecute(Func<SqlCommand, Task> ProcessCommand)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    await conn.OpenAsync();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await ProcessCommand(cmd);
                    conn.Close();
                }
            }
            //catch (WaselniException)
            //{
            //    throw;
            //}
            //catch (SqlException ex)
            //{
            //    throw new WaselniException()
            //    {
            //        OriginalErrorCode = ErrorCode.DatabaseError.ToString(),
            //        ErrorCode = ErrorCode.GeneralError.ToString("D"),
            //        OriginalMessage = string.Format("Message:{0}\tErrorCode:{1}", ex.Message, ex.ErrorCode),
            //        StackTrace = ex.StackTrace
            //    };
            //}
            catch (Exception ex)
            {
                throw ex;
                //throw new WaselniException()
                //{
                //    OriginalErrorCode = ErrorCode.GeneralError.ToString("D").ToString(),
                //    ErrorCode = ErrorCode.GeneralError.ToString("D"),
                //    OriginalMessage = ex.Message,
                //    StackTrace = ex.StackTrace
                //};
            }
        }


        /// <summary>
        /// Excecutes the transaction.
        /// </summary>
        /// <param name="ProcessCommand">The process command.</param>
        /// <returns>Task.</returns>
        /// <exception cref="WaselniException">
        /// </exception>
        public async Task ExcecuteTransaction(Func<SqlCommand, Task> ProcessCommand)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    var transaction = conn.BeginTransaction();
                    cmd.Transaction = transaction;

                    try
                    {
                        await ProcessCommand(cmd);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (SqlException ex)
                        {

                        }
                        catch (Exception ex)
                        {

                        }

                        throw;
                    }

                    transaction.Commit();
                    conn.Close();
                }
            }
            //catch (WaselniException)
            //{
            //    throw;
            //}
            //catch (SqlException ex)
            //{
            //    throw new WaselniException()
            //    {
            //        OriginalErrorCode = ErrorCode.DatabaseError.ToString(),
            //        ErrorCode = ErrorCode.GeneralError.ToString("D"),
            //        OriginalMessage = string.Format("Message:{0}\tErrorCode:{1}", ex.Message, ex.ErrorCode),
            //        StackTrace = ex.StackTrace
            //    };
            //}
            catch (Exception ex)
            {
                throw ex;
                //throw new WaselniException()
                //{
                //    OriginalErrorCode = ErrorCode.GeneralError.ToString("D").ToString(),
                //    ErrorCode = ErrorCode.GeneralError.ToString("D"),
                //    OriginalMessage = ex.Message,
                //    StackTrace = ex.StackTrace
                //};
            }
        }

        public async Task<DataSet> ExcecuteDataSet(Action<SqlCommand> FillCommand)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        using (SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            FillCommand(cmd);
                            DataSet objDataSet = new DataSet();
                            cmd.Connection = conn;
                            await conn.OpenAsync();
                            objSqlDataAdapter.Fill(objDataSet);
                            return objDataSet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion
    }
}
