using FISScanEventsMVC5.Controllers;
using FISScanEventsMVC5.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;

namespace FISScanEventsMVC5.Data
{
    public class DBTransaction
    {
        public SQLiteConnection sqlite_conn;
        private readonly ILogger<HomeController> _logger;
        public SQLiteConnection GetConnection()
        {            
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string file = System.IO.Path.Combine(currentDirectory, @"..\..\..\Data\FISScanEventsDB.db");
                string filePath = Path.GetFullPath(file);

                // Create a new database connection:
                sqlite_conn = new SQLiteConnection("URI=file:" + filePath);
                return sqlite_conn;            
        }


        // To retrieve records from database
        public DataTable ReadData()
        {
            try
            {
                sqlite_conn = GetConnection();

                sqlite_conn.Open();

                string stm = "SELECT * FROM ScanEvents ";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(stm, sqlite_conn);

                using var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " - Function: Readata() -");
                return null;
            }
            finally { 
                sqlite_conn.Close();
            }         

        }



        // To insert records
        public void InsertData(ScanEvent se)
        {            
            try
            {
                sqlite_conn = GetConnection();
                sqlite_conn.Open();

                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "INSERT INTO ScanEvents (EventId, ParcelId, Type, CreatedDateTimeUtc, StatusCode, DeviceTransactionId, DeviceId, UserId, CarrierId, RunId) " +
                    " VALUES(" + se.EventId + ",'" + se.ParcelId + "','" + se.Type + "','" + se.CreatedDateTimeUtc + "','" + se.StatusCode + "'," + se.Device.DeviceTransactionId + "," + se.Device.DeviceId + ",'" + se.User.UserId + "','" + se.User.CarrierId + "','" + se.User.RunId + "'); ";

               
                sqlite_cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + " - Function: InsertData");
            }
        }

        // Delete record for testing purposes, which calls every execution
        public void DeleteRecords()
        {
            try
            {
                DataTable dt = ReadData();
                if (dt.Rows.Count > 0)
                {
                    sqlite_conn = GetConnection();

                    sqlite_conn.Open();

                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM ScanEvents", sqlite_conn);
                    cmd.ExecuteNonQuery();
                    sqlite_conn.Close();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " Function: DeleteRecords ");
            }
        }
    }
}
