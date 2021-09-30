using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void ReadData()
        {
            //            SqlConnection conn = new SqlConnection();
            //            SqlDataReader reader = null;
            //            conn.ConnectionString = "Data Source=STHQ011A-01;Initial Catalog=Library;User ID=admin;Password=admin;";
            //            try
            //            {
            //                //open the connection
            //                conn.Open();
            //                SqlCommand command = new SqlCommand(@"
            //select * from Authors
            //",conn);

            //                reader = command.ExecuteReader();
            //                while (reader.Read())
            //                {
            //                    Console.WriteLine(reader[1]+"  "+reader[2]);
            //                }
            //            }
            //            finally
            //            {
            //                if (reader != null)
            //                {
            //                    reader.Close();
            //                }
            //                if (conn != null)
            //                {
            //                    conn.Close();
            //                }
            //            }

            using (SqlConnection conn = new SqlConnection())
            {
                // conn.ConnectionString = "Data Source=STHQ011A-01;Initial Catalog=Library;User ID=admin;Password=admin;";
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
                conn.Open();
                SqlCommand command = new SqlCommand(@"select * from Authors", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[1] + "  " + reader[2]);
                    }
                }
            }
        }


        static void ReadData2()
        {
            using (var conn=new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
                conn.Open();

                string query = @"select * from Authors;select * from Books";

                SqlCommand command = new SqlCommand(query, conn);
                
                using (var reader=command.ExecuteReader())
                {
                    bool hasShowed = false;
                    do
                    {

                    while (reader.Read())
                    {
                        if (!hasShowed)
                        {
                            hasShowed = true;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetName(i).ToString()+"\t");
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine(reader[0]+"\t"+reader[1]+"\t"+reader[2]);
                    }
                    Console.WriteLine("Total Records");
                    } while (reader.NextResult());
                }
            }
        }

        static void Insert()
        {
            using (SqlConnection conn=new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
                conn.Open();
                string query = @"insert into 
Authors(Id,Firstname,Lastname)
values(556,'Roger','Zelazny')";
                SqlCommand command = new SqlCommand(query,conn);
                var result=command.ExecuteNonQuery();
                Console.WriteLine(result);
            }
        }

        static void Main(string[] args)
        {
            // ReadData();
            //Insert();
            // ReadData2();
            // ReadDataWithParam();
            Call_SP_ByParam();
        }

        static void Call_SP_ByParam()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
                conn.Open();

                SqlCommand command = new SqlCommand("ShowStudentsByGroupId",conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter();
                param.Value = 3;
                param.ParameterName = "@GroupId";
                param.SqlDbType = SqlDbType.Int;
               ///// param.Direction = ParameterDirection.Output; if param is output
                /////Console.WriteLine(command.Parameters["@MyOutParam"].Value);
                command.Parameters.Add(param);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0] + "  " + reader[1]+" "+reader[2]);
                    }
                }



            }
        }

        static void ReadDataWithParam()
        {
            using (SqlConnection conn=new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
                conn.Open();
                string sql = @"select * from Books
                            where Pages>@MyCount";
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@MyCount";
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.Value = 400;

                var command = new SqlCommand(sql, conn);
                command.Parameters.Add(param);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[1] + "  " + reader[2]);
                    }
                }



            }
        }

    }
}
