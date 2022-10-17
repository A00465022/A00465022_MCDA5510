using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.VisualBasic;

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

        List<NewCustomerModel> CSVReaderTrans(List<string> fileList)
        {
            try
            {
                List<NewCustomerModel> writeRecords = new List<NewCustomerModel>();
                int validRecords = 0;
                int badRecords = 0;
                foreach (string file in fileList)
                {

                    string[] splitString = file.Split("\\");
                    string date = splitString[8] + "/" + splitString[9] + "/" + splitString[10];
                    string fileName = splitString[11];
                    Console.WriteLine("Started Reading {0} from {1}", fileName, date);

                    using (var reader = new StreamReader(fileList[0]))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
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
                Console.WriteLine("Valid Records : {0}", validRecords);
                Console.WriteLine("Skipped Records: {0}", badRecords);
                return writeRecords;
            } catch (Exception err)
            {
                throw;
            }
        }
        public static void Main(String[] args)
        {
            try
            {
                DirWalker fw = new DirWalker();
                AddDateField dtField = new AddDateField();
                List<string> fileList = new List<string>();

                string writePath = @"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\result.csv";

                fileList = fw.walk(@"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\Sample Data\Sample Data\", fileList);

                var writeRecords = dtField.CSVReaderTrans(fileList);

                dtField.CsvWriter(writePath, writeRecords);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}
