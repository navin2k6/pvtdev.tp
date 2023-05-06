using System;
using System.Data;
using AseAttributes;
using System.Reflection;
using System.Data.SqlClient;
using Debug = System.Diagnostics.Debug;
using StackTrace = System.Diagnostics.StackTrace;


// Author: Navin C. Pandit
// This class used to generate a sql command.
// Note: Please do not change.
namespace Arcmatics.TalentsProfile.DAL
{
    public sealed class AseCommandGenerator
    {
        private AseCommandGenerator()
        {
            throw new NotSupportedException();
        }

        public static readonly string ReturnValueParameterName = "RETURN_VALUE";

        public static readonly object[] NoValues = new object[] { };

        public static SqlCommand GenerateCommand(MethodInfo method, object[] values, String AseCommandText)
        {
            if (method == null)
                method = (MethodInfo)(new StackTrace().GetFrame(1).GetMethod());

            SqlCommand command = new SqlCommand();
            //command.Connection = connection;
            if (AseCommandText == string.Empty)
            {
                command.CommandText = "sp_" + method.Name;
                command.CommandType = CommandType.StoredProcedure;
                GenerateCommandParameters(command, method, values);
                command.Parameters.Add(ReturnValueParameterName, SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            }
            else
            {
                command.CommandType = CommandType.Text;
                command.CommandText = AseCommandText;
            }

            return command;
        }

        private static void GenerateCommandParameters(SqlCommand command, MethodInfo method, object[] values)
        {

            ParameterInfo[] methodParameters = method.GetParameters();

            int paramIndex = 0;


            foreach (ParameterInfo paramInfo in methodParameters)
            {
                if (Attribute.IsDefined(paramInfo, typeof(NonCommandParameterAttribute)))
                    continue;

                AseParameterAttribute paramAttribute = (AseParameterAttribute)Attribute.GetCustomAttribute(
                    paramInfo, typeof(AseParameterAttribute));

                if (paramAttribute == null)
                    paramAttribute = new AseParameterAttribute();

                SqlParameter AseParameter = new SqlParameter();

                if (paramAttribute.IsNameDefined)
                    AseParameter.ParameterName = paramAttribute.Name;
                else
                    AseParameter.ParameterName = paramInfo.Name;

                if (!AseParameter.ParameterName.StartsWith("@"))
                    AseParameter.ParameterName = "@" + AseParameter.ParameterName;

                if (paramAttribute.IsTypeDefined)
                    AseParameter.SqlDbType = paramAttribute.AseType;

                if (paramAttribute.IsSizeDefined)
                    AseParameter.Size = paramAttribute.Size;

                if (paramAttribute.IsScaleDefined)
                    AseParameter.Scale = paramAttribute.Scale;

                if (paramAttribute.IsPrecisionDefined)
                    AseParameter.Precision = paramAttribute.Precision;

                if (paramAttribute.IsDirectionDefined)
                {
                    AseParameter.Direction = paramAttribute.Direction;
                }
                else
                {
                    if (paramInfo.ParameterType.IsByRef)
                    {
                        AseParameter.Direction = paramInfo.IsOut ?
                            ParameterDirection.Output :
                            ParameterDirection.InputOutput;
                    }
                    else
                    {
                        AseParameter.Direction = ParameterDirection.Input;
                    }
                }

                AseParameter.Value = values[paramIndex];
                command.Parameters.Add(AseParameter);

                paramIndex++;
            }
        }
    }
}