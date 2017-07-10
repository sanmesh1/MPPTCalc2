using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoRental2.Models;
using VideoRental2.ViewModels;
using System.Data.Entity;//needed for Include() func to include object members of database objects
using System.Diagnostics;
using System.Web.Helpers;
using System.ComponentModel;
using System.Web.Hosting;
using System.IO;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;


namespace VideoRental2.Controllers
{
    public class CircuitCalculatorController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MPPTCalculator() //take in circuit data and output ViewModel with circuit plot data and variables to choose plots
        {
            GenerateAsciiOutputFromNetlist(Server.MapPath(@"\Content\OtherFile\RC2.cir"));
            List<ChartPoints> dataSetsMember = ConvertCircuitAsciiOutputToDataList(Server.MapPath(@"\Content\OtherFile\RC2.raw"));
            MPPTCalculatorViewModel ViewModel = new MPPTCalculatorViewModel();
            ViewModel.dataSets = dataSetsMember;
            return View(ViewModel);
        }

        public JsonResult GetJsonPlotData(int? xData, int? yData)
        {
            //get data
            GenerateAsciiOutputFromNetlist(Server.MapPath(@"\Content\OtherFile\RC2.cir"));
            List<ChartPoints> dataSetsMember = ConvertCircuitAsciiOutputToDataList(Server.MapPath(@"\Content\OtherFile\RC2.raw"));

            //set default value
            if (!xData.HasValue)
                xData = 0;
            if (!yData.HasValue)
                yData = 1;

            MPPTCalculatorViewModel ViewModel = new MPPTCalculatorViewModel();

            ViewModel.dataSets = dataSetsMember;
            ViewModel.lengthArray = dataSetsMember[0].mag.Count();
            ViewModel.xData = (int)xData;
            ViewModel.yData = (int)yData;
            return Json(ViewModel, JsonRequestBehavior.AllowGet);
        }//takes in varaibles for x-axis and y-axis to display and outputs a Json object with the dataSets as well as 

        /*
        public JsonResult ActOnCircuitParams ()
        {
            return Json();
        }
        */
        public void GenerateAsciiOutputFromNetlist(string path)
        {
            try
            {
                string mypath = "\"" + path + "\"";
                Process.Start(Server.MapPath(@"\Content\OtherFile\scad3.exe"), "-ascii -b " + mypath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }//Take in .cir netlist and generate .raw file with ascii output data in same folder

        public List<ChartPoints> ConvertCircuitAsciiOutputToDataList(string path)
        {
            var dataSets = new List<ChartPoints>();
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader reader = new StreamReader(path);
                String line;
                //find where the "No. Varaibles" text is
                do
                {
                    line = reader.ReadLine();
                } while (!line.Contains("Variables"));
                String[] bits = line.Split(' ');
                int numVar = int.Parse(bits[2]);

                //Go down till you find varaibles,
                do
                {
                    line = reader.ReadLine();
                } while (!line.Contains("Variables"));

                //create list of ChartPoints objects to gather all the x-y data sets

                int i;
                //read the numVar amount of varaibles
                for (i = 0; i < numVar; i++)
                {
                    //parse the characters into spaces, and get the properties
                    line = reader.ReadLine();
                    bits = line.Split('\t'); //depends on bits being size 3
                    dataSets.Add(new ChartPoints(int.Parse(bits[1]), bits[2], bits[3]));  //id, name, type
                }

                //skip one read line to skip to the data points
                line = reader.ReadLine();

                //Continue to read until you reach end of file
                i = 0;
                line = reader.ReadLine();
                Double testNum;
                string testString;
                while (line != null)
                {
                    //for both time and frequency data, the x-axis has 3 strings, and the other points have two string
                    //see if you have x-axis or other params through seeing the number of elements
                    if (line.Split('\t').Count() == 3)
                    {
                        line = line.Split('\t')[2];
                        i = 0;
                    }
                    else
                    {
                        line = line.Split('\t')[1];
                        i++;
                    }
                    if (dataSets.ElementAt(0).type == "frequency")
                    {

                        dataSets.ElementAt(i).mag.Add(Double.Parse(line.Split(',')[0]));
                        dataSets.ElementAt(i).phase.Add(Double.Parse(line.Split(',')[1]));
                    }
                    else
                    {
                        testString = line.Split(',')[0];
                        testNum = Double.Parse(testString);
                        dataSets.ElementAt(i).mag.Add(testNum);// Double.Parse(line.Split(',')[0])
                        //dataSets.ElementAt(i).phase.Add(0); //don't need phase for time so dont even need to waste operations putting a zero
                    }
                    line = reader.ReadLine();
                }

                //close the file
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return dataSets;
        }//Take in data from .raw file generated and format it into Lists of data Sets

        public void generateBuckBoostCircuit()
        {

        }
    }
}