using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace Assignment1
{
    public class AddDateField
    {
        void CsvWriter(string path, List<NewCustomerModel> resultData)
        {
            try
            {
                using (var writer = new StreamWriter(path))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(resultData);
                }
            } catch(Exception err)
            {
                throw;
            }
        }

        List<NewCustomerModel> CSVReaderTrans(List<string> fileList, log4net.ILog logger)
        {
            try
            {
                List<NewCustomerModel> writeRecords = new List<NewCustomerModel>();
                int validRecords = 0;
                int badRecords = 0;
                int missingField = 0;
                foreach (string file in fileList)
                {

                    string[] splitString = file.Split("\\");
                    string date = splitString[8] + "/" + splitString[9] + "/" + splitString[10];
                    string fileName = splitString[11];
                    logger.InfoFormat("Started Reading {0} from {1}", fileName, date);
                    var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);

                    config.MissingFieldFound = (msg) =>
                    {
                        missingField++;
                        logger.Warn(msg);
                    }; ;

                    using (var reader = new StreamReader(file))
                    using (var csv = new CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<CustomerDataModel>();

                        foreach (var customer in records)
                        {
                            if (customer.firstName != "" && customer.lastName != "" && customer.streetNumber != "" && customer.street != "" && customer.city != "" &&
                                customer.province != "" && customer.postalCode != "" && customer.country != "" && customer.phoneNumber != "" &&
                                customer.emailAddress != "")
                            {
                                validRecords++;
                                NewCustomerModel newModel = new NewCustomerModel();
                                newModel.firstName = customer.firstName;
                                newModel.lastName = customer.lastName;
                                newModel.streetNumber = customer.streetNumber;
                                newModel.street = customer.street;
                                newModel.city = customer.city;
                                newModel.province = customer.province;
                                newModel.postalCode = customer.postalCode;
                                newModel.country = customer.country;
                                newModel.phoneNumber = customer.phoneNumber;
                                newModel.emailAddress = customer.emailAddress;
                                newModel.date = date;

                                writeRecords.Add(newModel);
                            }
                            else
                            {
                                badRecords++;
                            }
                        }
                    }
                }
                logger.InfoFormat("Valid Records : {0}", validRecords);
                logger.InfoFormat("Skipped Records: {0}", badRecords);
                logger.InfoFormat("Missing Fields: {0}", missingField);
                return writeRecords;
            } catch (Exception err)
            {
                throw;
            }
        }
        public static void Main(String[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(AddDateField));
            Stopwatch stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();

                DirWalker fw = new DirWalker();
                AddDateField dtField = new AddDateField();
                List<string> fileList = new List<string>();

                string writePath = @"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\result.csv";

                fileList = fw.walk(@"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\Sample Data\Sample Data\", fileList);

                var writeRecords = dtField.CSVReaderTrans(fileList, log);

                dtField.CsvWriter(writePath, writeRecords);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                log.InfoFormat("Total Runtime: {0}", ts);
            }
            catch (Exception err)
            {
                log.Error(err.Message);
            }
        }
    }
}
