using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Assignment1.ProgAssign1.Model;
using CsvHelper;
using log4net;

namespace Assignment1.ProgAssign1
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
            }
            catch (Exception err)
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
                    var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture);

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
                WriteLogFile.WriteLog(String.Format("Valid Records :{0} @ {1}", validRecords, DateTime.Now));
                WriteLogFile.WriteLog(String.Format("Skipped Records: {0} @ {1}", badRecords, DateTime.Now));
                WriteLogFile.WriteLog(String.Format("Missing Fields: {0} @ {1}", missingField, DateTime.Now));

                return writeRecords;
            }
            catch (Exception err)
            {
                throw;
            }
        }
        public static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(AddDateField));
            Stopwatch stopWatch = new Stopwatch();
            WriteLogFile.WriteLog(String.Format("{0} @ {1}", "Program Started", DateTime.Now));
            try
            {
                stopWatch.Start();

                DirWalker fw = new DirWalker();
                AddDateField dtField = new AddDateField();
                List<string> fileList = new List<string>();

                string writePath = @"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\Assignment1\Assignment1\ProgAssign1\Output\result.csv";

                fileList = fw.walk(@"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\Sample Data\Sample Data\", fileList);
                WriteLogFile.WriteLog(String.Format("{0} @ {1}", "Started Reading CSV Data", DateTime.Now));

                var writeRecords = dtField.CSVReaderTrans(fileList, log);

                WriteLogFile.WriteLog(String.Format("{0} @ {1}", "Finished Reading CSV Data", DateTime.Now));

                WriteLogFile.WriteLog(String.Format("{0} @ {1}", "Started Writing Data", DateTime.Now));

                dtField.CsvWriter(writePath, writeRecords);

                WriteLogFile.WriteLog(String.Format("{0} @ {1}", "Finished Writing Data", DateTime.Now));

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                WriteLogFile.WriteLog(String.Format("{0}{1} @ {2} \n-------END-------\n", "Total Runtime: ", ts, DateTime.Now));

            }
            catch (Exception err)
            {
                log.Error(err.Message);
                WriteLogFile.WriteLog(String.Format("Error: {0} @ {1}", err.Message, DateTime.Now));
            }
        }
    }

    class WriteLogFile
    {

        public static bool WriteLog(string strMessage)
        {
            try
            {
                string Logpath = @"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\Assignment1\Assignment1\ProgAssign1\Logs";
                string fileName = "log.txt";
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}",Logpath, fileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
