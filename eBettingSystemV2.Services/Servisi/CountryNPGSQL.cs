using eBettingSystemV2.Model.Models;
using eBettingSystemV2.Models;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eBettingSystemV2.Services.Extensions;

namespace eBettingSystemV2.Services.Servisi
{
    public class CountryNPGSQL:ICountryNPGSQL
    {
        public IConfiguration Configuration { get; }


        public CountryNPGSQL( IConfiguration Service1)
        {

            Configuration = Service1;

        
        }






        public async Task TestNPGSQL()
        {
            var connString = Configuration.GetSection("ConnectionString").GetSection("DefaultConnection").Value;

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();


            try
            {

                string Tablename = "Country";

                await using (var cmd = new NpgsqlCommand("INSERT INTO \"BettingSystem\".\"Country\" (\"CountryName\") VALUES ('test126');", conn))
                {
                    //parametri su varijable
                    var displayorder = "test125";

                    cmd.Parameters.Add(new NpgsqlParameter("@display_order", "test125"));
                    cmd.CommandText+=string.Format(@"Insert into ""BettingSystem"".""Country"" (""CountryName"")  VALUES(@display_order) ");




                    cmd.Parameters.AddWithValue("Hello world");
                    //await cmd.ExecuteNonQueryAsync();
                }


            }
            catch (Exception ex)
            { 
            
            
            
            
            
            
            }

            // Get  All Country
            //T test = default(T);
            //var test2 = test.GetType();
            //g.Field = "A string";
            ////...
            //Console.WriteLine("Generic.Field           = \"{0}\"", g.Field);
            //Console.WriteLine("Generic.Field.GetType() = {0}", g.Field.GetType().FullName);

            int i = 0;


            //-----
            //try
            //{
            //    await using (var cmd = new NpgsqlCommand("SELECT * FROM \"BettingSystem\".\"Country\" ", conn))
            //    await using (var reader = await cmd.ExecuteReaderAsync())
            //    {
            //        while (await reader.ReadAsync())
            //        {
            //            Console.WriteLine($"{reader.GetValue(0)} -- {reader.GetString(1)}");                                             
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{                      
            //}
            //dapper


            //try
            //{
            //    var conn2 = new NpgsqlConnection(connString);
            //    var cmd2 = new NpgsqlCommand("SELECT * FROM \"BettingSystem\".\"Country\" ", conn2);
            //    conn2.Open();
            //    var reader = cmd2.ExecuteReader();



            //    while (reader.Read())
            //    {
            //        CountryModel Country = new CountryModel();
            //        Country.CountryId = reader.GetRecord<int>("CountryId");
            //        Country.CountryName = reader.GetRecord<string>("CountryName");

            //        //Rasponse.Languages.Add(language);
            //        Console.WriteLine($"{Country.CountryId} -- {Country.CountryName}");

            //    }

            //    conn2.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}




            //parametri ukoliko ih ima  : 

            //try
            //{
            //    NpgsqlCommand command = conn.CreateCommand();
            //    command.Parameters.Add(new NpgsqlParameter("@CountryName", "Backlangalesh4"));
            //    //command.Parameters.Add(new NpgsqlParameter("@name", name));
            //    command.CommandText = string.Format(@"Insert into ""BettingSystem"".""Country""(""CountryName"")
            //                                      VALUES(@CountryName) returning ""CountryName"" , ""CountryId"";");
            //    var reader = command.ExecuteReader();


            //    while (reader.Read())
            //    {
            //        CountryModel Country = new CountryModel();
            //        Country.CountryId = reader.GetRecord<int>("CountryId");
            //        Country.CountryName = reader.GetRecord<string>("CountryName");

            //        //Rasponse.Languages.Add(language);
            //        Console.WriteLine($"{Country.CountryId} -- {Country.CountryName}");

            //    }
            //}
            //catch (Exception ex)
            //{ 




            //}

            NpgsqlCommand command = conn.CreateCommand();
            command.Parameters.Add(new NpgsqlParameter("@CountryName", "Backlangalesh9"));
            //command.Parameters.Add(new NpgsqlParameter("@name", name));
            command.CommandText = string.Format(@"Insert into ""BettingSystem"".""Country""(""CountryName"")
                                                  VALUES(@CountryName) returning ""CountryId"",""CountryName"";");
            try
            {
                int id = (int)command.ExecuteScalar();
                Rasponse.IsSuccess = id > 0;
                Rasponse.ReturningId = id;
            }
            catch (Exception ex)
            {
                Rasponse.IsSuccess = false;
                Rasponse.ErrorMessage = ex.Message.ToString();
            }







        }

    }
}


//NpgsqlCommand command = conn.CreateCommand();

//komanda za upit : command.CommandText = string.Format(@"select * from languages");


//za čitanje iz baze:
// while (reader.Read())
//{
//    Language language = new Language();
//    language.Id = reader.GetRecord<int>("language_id");
//    language.Name = reader.GetRecord<string>("name");
//    language.LanguageCulture = reader.GetRecord<string>("language_culture");
//    language.DisplayOrder = reader.GetRecord<int>("display_order");
//    language.IsDefault = reader.GetRecord<bool>("is_default");
//    language.ParentLanguageId = reader.GetRecord<int>("parent_language_id");

//    response.Languages.Add(language);

//}

//return response;

//za insert u bazu :


//parametri ukoliko ih ima  : 
//command.Parameters.Add(new NpgsqlParameter("@display_order", request.DisplayOrder));
//command.Parameters.Add(new NpgsqlParameter("@name", name));
//....command.CommandText = string.Format(@"Insert into languages.languages(name,language_culture,display_order,is_default,parent_language_id)  VALUES(@name,@language_culture,@display_order,@is_default,@parent_language_id) ");

//upis u bazu:
//  try
//{
//    int id = (int)command.ExecuteScalar();
//    response.IsSuccess = id > 0;
//    response.ReturningId = id;
//}
//catch (Exception ex)
//{
//    response.IsSuccess = false;
//    response.ErrorMessage = ex.Message.ToString();
//}