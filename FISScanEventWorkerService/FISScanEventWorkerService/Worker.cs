using FISScanEventWorkerService.Entities;
using FISScanEventWorkerService.Validation;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FISScanEventWorkerService
{

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        //private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;
        private readonly ScanEventData scanEventData;
        private readonly JsonSerializerOptions jsonOptions;
        private readonly string apiLink;
        private readonly string latestEventDataFile;

        public Worker(ILogger<Worker> log, IHttpClientFactory httpClientFactory)
        {
            try
            {
                logger = log;
                httpClient = httpClientFactory.CreateClient();
                scanEventData = LoadScanEventData();

                jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                apiLink = "http://localhost/v1/scans/scanevents";

                latestEventDataFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\WorkerLogs.txt"));                

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error occurred in Worker");
            }            
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Worker app is started at: {time}", DateTimeOffset.Now);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Worker app is stopped at: {time}", DateTimeOffset.Now);
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Fetch scan events from API
                    var apiUrl = $"{apiLink}?FromEventId={scanEventData.LastEventId + 1}&Limit=100";
                    var response = await httpClient.GetAsync(apiUrl, stoppingToken);
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    var scanEvents = JsonSerializer.Deserialize<ScanEvent[]>(json, jsonOptions);

                    // For Fluent validation
                    ScanEventValidator validator = new ScanEventValidator();
                    IList<ValidationFailure> failures;
                    // Process scan events
                    if (scanEvents.Length > 0)
                    {
                        foreach (var scanEvent in scanEvents)
                        {
                            ValidationResult validationResult = validator.Validate(scanEvent);
                            failures = validationResult.Errors;
                            if (validationResult.IsValid)
                            {
                                // Update scan event data
                                scanEventData.LastEventId = scanEvent.EventId;
                                scanEventData.Events[scanEvent.EventId] = scanEvent;

                                // Check for pickup or delivery events
                                if (scanEvent.Type == "PICKUP")
                                {
                                    scanEventData.PickupEvents.Add(scanEvent.EventId);
                                }
                                else if (scanEvent.Type == "DELIVERY")
                                {
                                    scanEventData.DeliveryEvents.Add(scanEvent.EventId);
                                }
                            }
                            else
                            {
                                foreach (ValidationFailure failure in failures)
                                {
                                    // Write errors to the log
                                    logger.LogError(failure.ErrorMessage + "  -   " + DateTime.Now);
                                }
                            }
                        }


                        // Save updated scan event data
                        SaveScanEventData(scanEventData);
                    }

                    // Wait for next iteration
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while processing scan events");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
        }

        // Load scan event data from persistence (file)
        private ScanEventData LoadScanEventData()
        {
            try
            {
                // Read the contents of the input file
                string jsonString = File.ReadAllText(latestEventDataFile);
                // Deserialize the JSON string into a ScanEventData object
                ScanEventData scanEventData = JsonSerializer.Deserialize<ScanEventData>(jsonString);
                return scanEventData;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in loading scanning latest event data");
                return null;
            }

        }

        // Save most recent scan event data to persistence (file and database)
        private void SaveScanEventData(ScanEventData scanEventData)
        {   
            try
            {
                // Call for Save ScanEventData to Database which is to be created.
                // SaveScanEventDataDB();


                // Here Save latest event data in order to keep track of it, to build the URL most recent 100 records
                List<ScanEventData> scanEvents = new List<ScanEventData>();
                
                // Serialize the object to a JSON string
                string jsonData = JsonSerializer.Serialize(scanEvents.FirstOrDefault());
               
                // Write the JSON data to the output file
                using (StreamWriter outputFile = new StreamWriter(latestEventDataFile))
                {
                    outputFile.WriteLine(jsonData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while saving last event details to the file");
            }
        }

        /*
            * A sample instance of the ScanEventData class in order to show the structure
            
           */
        //var scanEventData = new ScanEventData
        //{
        //    LastEventId = 83269,
        //    Events = new Dictionary<int, ScanEvent>
        //{
        //    { 1, new ScanEvent {
        //        EventId = 1,
        //        ParcelId = 1001,
        //        Type = "PICKUP",
        //        CreatedDateTimeUtc = DateTime.UtcNow,
        //        StatusCode = "",
        //        Device = new Device {
        //            DeviceTransactionId = 83269,
        //            DeviceId = 103
        //        },
        //        User = new User {
        //            UserId = "NC1001",
        //            CarrierId = "NC",
        //            RunId = "100"
        //        }
        //    }},
        //    { 2, new ScanEvent {
        //        EventId = 2,
        //        ParcelId = 1002,
        //        Type = "DELIVERY",
        //        CreatedDateTimeUtc = DateTime.UtcNow,
        //        StatusCode = "",
        //        Device = new Device {
        //            DeviceTransactionId = 83270,
        //            DeviceId = 104
        //        },
        //        User = new User {
        //            UserId = "NC1002",
        //            CarrierId = "NC",
        //            RunId = "100"
        //        }
        //    }}
        //},
        //    PickupEvents = new HashSet<int> { 1 },
        //    DeliveryEvents = new HashSet<int> { 2 }
        //};

    }
}
