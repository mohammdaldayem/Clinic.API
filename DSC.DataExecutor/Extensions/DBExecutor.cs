using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSC.DataExecutor.Extensions
{
  public  class DBExecutor
    {
         void AttachParameters(ref DBCommand cmd, object obj)
        {
            object FieldValue = null;
            foreach (FieldInfo Field in obj.GetType().GetFields())
            {
                FieldValue = Field.GetValue(obj);
                if (FieldValue.ToString().IndexOf("SpParamerters") != -1)
                {
                    ((SpParamerters)FieldValue).Parameter.ParameterName = "@" + Field.Name;
                    cmd.command.Parameters.Add(((SpParamerters)FieldValue).Parameter);
                }
                else if (((System.Type)Field.FieldType).Name == "String" && Field.Name == "SPName")
                {
                    cmd.command.CommandText = Convert.ToString(FieldValue);
                }
            }
        }
        public  List<object> OneResultReader(object senderobj, Type classType)
        {
            List<object> DataList = null;
            DBCommand cmd = new DBCommand();
            try
            {

                AttachParameters(ref cmd, senderobj);
                cmd.command.Connection.Open();
                SqlDataReader dr = cmd.command.ExecuteReader(CommandBehavior.KeyInfo);
                DataTable dt = dr.GetSchemaTable();
                DataList = new List<object>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        object ClassInstanse = Activator.CreateInstance(classType);
                        int index = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            classType.GetProperty(row[0].ToString()).SetValue(ClassInstanse, dr.GetValue(index++), null);
                        }
                        DataList.Add(ClassInstanse);
                    }
                }
                dr.Close();
                cmd.command.Connection.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (cmd.command.Connection.State == ConnectionState.Open)
                    cmd.command.Connection.Close();
                //dr.Close();
            }
            return DataList;
        }
        public  object MultiResultReader(object senderobj, Type classType)
        {
            var DataList = new List<object>();
            DBCommand cmd = new DBCommand();
            try
            {
                AttachParameters(ref cmd, senderobj);
                cmd.command.Connection.Open();
                SqlDataReader dr = cmd.command.ExecuteReader(CommandBehavior.KeyInfo);
                DataTable dt = new DataTable();
                DataList = new List<object>();
                object mainClassInstanse = Activator.CreateInstance(classType);
                PropertyInfo[] pi = classType.GetProperties();
                FieldInfo[] fields = classType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.Static | BindingFlags.Instance |
                                                         BindingFlags.DeclaredOnly);
                //t.GetFields(flags).Concat(GetAllFields(t.BaseType));
                int numberOfResult = pi.Length;
                int counter = 0;
                do
                {
                    if (counter != 0)
                        dr.NextResult();
                    if (dr.HasRows)
                    {
                        if (pi[counter].PropertyType.FullName == PropertyType.StringType)
                        {
                            dr.Read();
                            if (dr[0] != DBNull.Value)
                                pi[counter].SetValue(mainClassInstanse, dr[0].ToString(), null);
                            counter++;
                        }
                        else if (pi[counter].PropertyType.FullName == PropertyType.IntType)
                        {
                            dr.Read();
                            if (dr[0] != DBNull.Value)
                                pi[counter].SetValue(mainClassInstanse, Convert.ToInt32(dr[0].ToString()), null);
                            counter++;
                        }
                        else
                        {
                            string typeString = ((SubPropertyName)(pi[counter].GetCustomAttributes(false)[0])).Name;
                            Type t = Type.GetType(typeString);
                            dt = dr.GetSchemaTable();
                            while (dr.Read())
                            {

                                int index = 0;
                                object ClassInstanse = Activator.CreateInstance(t);
                                //Parallel.ForEach(dt.AsEnumerable(), row =>
                                //{
                                //    if (dr.GetValue(index) != System.DBNull.Value)
                                //    {
                                //        t.GetProperty(row[0].ToString()).SetValue(ClassInstanse, dr.GetValue(index++), null);
                                //    }
                                //    else
                                //    {
                                //        index++;
                                //    }


                                //});
                                //Parallel.ForEach(dt.Rows.OfType<System.Data.DataRow>(), (DataRow row, ParallelLoopState loopState, long i) =>
                                //{
                                //    if (dr.GetValue((int) i) != System.DBNull.Value)
                                //    {
                                //        t.GetProperty(row[0].ToString()).SetValue(ClassInstanse, dr.GetValue((int)i), null);
                                //    }
                                //    else
                                //    {
                                //        index++;
                                //    }
                                //});

                                foreach (DataRow row in dt.Rows)
                                {
                                    if (dr.GetValue(index) != System.DBNull.Value)
                                    {
                                        t.GetProperty(row[0].ToString()).SetValue(ClassInstanse, dr.GetValue(index++), null);
                                    }
                                    else
                                    {
                                        index++;
                                    }
                                }
                                DataList.Add(ClassInstanse);
                            }
                            var result = ((IEnumerable)DataList).Cast<object>().ToList();
                            MethodInfo castMethod = this.GetType().GetMethod("Cast").MakeGenericMethod(t);
                            object castedObject = castMethod.Invoke(null, new object[] { DataList });
                            pi[counter].SetValue(mainClassInstanse, castedObject, null);
                            //pi[counter].SetValue(mainClassInstanse, Convert.ChangeType(castedObject, pi[counter].PropertyType), null);

                            counter++;
                        }
                    }
                    else
                    {
                        counter++;
                    }
                } while (counter < numberOfResult);
                dr.Close();
                cmd.command.Connection.Close();
                return mainClassInstanse;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

                if (cmd.command.Connection.State == ConnectionState.Open)
                    cmd.command.Connection.Close();
            }
            //return DataList;
        }
        public  void ExecuteNoQuery(object senderobj)
        {
            DBCommand cmd = new DBCommand();

            try
            {
                AttachParameters(ref cmd, senderobj);
                cmd.command.Connection.Open();
                int rows = cmd.command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                cmd.command.Connection.Close();
                throw (ex);
            }
            finally
            {
                cmd.command.Connection.Close();
            }

        } 
         List<T> Cast<T>(List<object> o)
        {
            return o.Cast<T>().ToList();
        }
    }
}
