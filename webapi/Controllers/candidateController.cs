using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class candidateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public candidateController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            String query = @"dbo.Getcandidates";//@"SELECT * FROM dbo.dbcandidates";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DevConnection");
            SqlDataReader myreader;
            using(SqlConnection mycon=new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand mycommand=new SqlCommand(query, mycon))
                {
                    myreader = mycommand.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(dbcandidates dbc)
        {
            String query = @"dbo.Insecandi";
            
            /*@"INSERT INTO dbo.dbcandidates(fullname,age,mobile,email,bloodgroup,address) 
                            VALUES ('"+dbc.fullname+@"','"+dbc.age+ @"','" + dbc.mobile + @"','" + dbc.email + @"','" + dbc.bloodgroup + @"','" + dbc.address + @"')";*/
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DevConnection");
           SqlDataReader myreader;
            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.CommandType = CommandType.StoredProcedure;
                    mycommand.Parameters.Add("@fullname", SqlDbType.NVarChar).Value = dbc.fullname;
                    mycommand.Parameters.Add("@email", SqlDbType.NVarChar).Value = dbc.email;
                    mycommand.Parameters.Add("@mobile", SqlDbType.NVarChar).Value = dbc.mobile;
                    mycommand.Parameters.Add("@age", SqlDbType.Int).Value = dbc.age;
                    mycommand.Parameters.Add("@bloodgroup", SqlDbType.NVarChar).Value = dbc.bloodgroup;
                    mycommand.Parameters.Add("@address", SqlDbType.NVarChar).Value = dbc.address;
                    mycommand.Parameters.Add("@sex", SqlDbType.NVarChar).Value = dbc.sex;


                    myreader = mycommand.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("insert sucess");//table);//
        }

        [HttpPut]
        public JsonResult Put(dbcandidates dbc)
        {
            String query = @"dbo.upcandi";
                /* @" UPDATE dbo.dbcandidates SET age= '" + dbc.age + @"'
                           WHERE id=" + dbc.id + @"*/
                          //  ";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DevConnection");
            SqlDataReader myreader;
            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.CommandType = CommandType.StoredProcedure;
                    mycommand.Parameters.Add("@age", SqlDbType.Int).Value = dbc.age;
                    mycommand.Parameters.Add("@id", SqlDbType.Int).Value = dbc.id;
                    myreader = mycommand.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("update sucess");//table);//"
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            String query = @"dbo.del";
                /* @" DELETE FROM dbo.dbcandidates 
                           WHERE id=" + id + @"
                            ";*/
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("DevConnection");
            SqlDataReader myreader;
            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.CommandType = CommandType.StoredProcedure;
                    mycommand.Parameters.Add("@id", SqlDbType.Int).Value =id;                   
                    myreader = mycommand.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("delete sucess");//(table);// 
        }

        [Route("Savefile")]
        [HttpPost]
        public JsonResult Savefile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/photoes/" + filename;

                using (var stream=new FileStream(physicalPath,FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch(Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}