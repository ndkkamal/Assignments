using FISScanEventsMVC5.Data;
using FISScanEventsMVC5.Models;
using FISScanEventsMVC5.Validation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

/* 
    Created by : Indika Kamal
    Date       : 12th Feb 2023
    Purpose    : Reading scanned events data
 */

namespace FISScanEventsMVC5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("HomeController.Index method called!!!");

            // In order to show demo, reading from Json file and save in DB
            PostJsonDataFromFiletoDB();

            try
            {
                DBTransaction dbtr = new DBTransaction();
                DataTable dt = dbtr.ReadData();
                return View(dt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "  ");
                return null;
            }                       
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // Reading data from Json file
        public void PostJsonDataFromFiletoDB()
        {
            // To find the JSon file            
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string file = System.IO.Path.Combine(currentDirectory, @"..\..\..\Data\fisevents.json");
            string filePath = Path.GetFullPath(file);

            try
            {

                // Deleting records for testing purposes, because unable to insert duplicate records
                DBTransaction dbtr = new DBTransaction();
                dbtr.DeleteRecords();

                if (System.IO.File.Exists(filePath))
                {
                    var listScanEvents = new ScanEvents();
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string json = reader.ReadToEnd();
                        listScanEvents = JsonConvert.DeserializeObject<ScanEvents>(json);

                        ScanEventValidator validator = new ScanEventValidator();
                        IList<ValidationFailure> failures;
                        foreach (ScanEvent se in listScanEvents.scanevents)
                        {
                            ValidationResult validationResult = validator.Validate(se);
                            failures = validationResult.Errors;
                            if (!validationResult.IsValid)
                            {
                                foreach (ValidationFailure failure in failures)
                                {
                                    // Write errors to the log
                                    _logger.LogError(failure.ErrorMessage + "  -   " + DateTime.Now);
                                }
                            }
                            else
                            {
                                // Write data to database                            
                                dbtr.InsertData(se);
                                //DBTransaction.ReadData();
                                _logger.LogInformation("Inserted Event ID : " + se.EventId + " Date and Time : " + DateTime.Now);
                            }
                        }
                    }


                }
            }
            catch (Exception ex) { 
                _logger.LogError(ex.Message + " - PostJsonDataFromFiletoDB ");
            }
            
        }
    }
}
