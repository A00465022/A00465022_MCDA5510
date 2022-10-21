Assignment #1
-------------------------------------------------------------------
Project Structure:

Assignment 1
 |
 |- Assignment 1
      |
      |- ProgAssign1
            |
            |- Outputs
            |- Logs
            |- Model
            |- AddDateField.cs
            |- DirWalker.cs
            
Program Working:
* Create Model similar to schema in CSV
* Read the CSV Data using CSVHealper library and insert data into the model
* Check whether row is valid or invalid
* If valid, increament validRows else increament badRows
* Write the valid row to csv file appending date column
* Log Valid rows, Bad rows.
