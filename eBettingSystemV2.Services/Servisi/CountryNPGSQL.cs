using eBettingSystemV2.Services.Interface;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    await cmd.ExecuteNonQueryAsync();
                }


            }
            catch (Exception ex)
            { 
            
            
            
            
            
            
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