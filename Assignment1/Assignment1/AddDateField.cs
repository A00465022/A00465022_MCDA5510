using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using LoggingAdvanced.Console;
using Microsoft.VisualBasic;

namespace Assignment1
{
    public class AddDateField
    {
        public static void Main(String[] args)
        {
            DirWalker fw = new DirWalker();
            //string[] fileList = fw.walk(@"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\Sample Data\Sample Data\");
            string[] fileList = new string[1];
            int badRecord = 0;
            int validRecord = 0;
            string writePath = @"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\result.csv";

            fileList[0] = @"C:\Users\Aravind Lakshmanan\Documents\GitHub\MCDA5510_Assignments\Sample Data\Sample Data\2017\11\8\CustomerData0.csv";
            foreach (string file in fileList) {

                string[] splitString = file.Split("\\");
                string year = splitString[8];
                string month = splitString[9];
                string day = splitString[10];
                string date = year + "/" + month + "/" + day;
                string fileName = splitString[11];
                Console.WriteLine("Started Reading {0} from {1}/{2}/{3}", fileName, year, month, day);
                List<NewCustomerModel> writeRecords = new List<NewCustomerModel>();

                using (var reader = new StreamReader(fileList[0]))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<CustomerDataModel>();
                    foreach (var customer in records)
                    {
                        if (customer.firstName != "" && customer.lastName != "" && customer.streetNumber != "" && customer.street != "" && customer.city != "" && 
                            customer.province != ""  && customer.postalCode != "" && customer.country != "" && customer.phoneNumber != "" &&
                            customer.emailAddress != "") {
                            validRecord++;
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
                            badRecord++;
                        }
                    }
                }
                using (var writer = new StreamWriter(writePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(writeRecords);
                }
            }
            Console.WriteLine("Valid Records : {0}", validRecord);
            Console.WriteLine("Skipped Records: {0}", badRecord);
        }
    }
}
