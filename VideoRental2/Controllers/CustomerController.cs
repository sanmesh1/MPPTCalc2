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
    public class CustomerController : Controller
    {
        private ApplicationDbContext _context; //to access database
        public CustomerController()
        {
            _context = new ApplicationDbContext(); //initialize database variable
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose(); //dispose the database variable
        }
        /*public List<Customer> customersList = new List<Customer>()
            {
                new Customer() {name = "Sam", id = 1 },
                new Customer() { name = "Duncan", id = 2 }
            };*/
        public ActionResult Index()
        {
            //var customersList = _context.Customer.Include(c => c.MembershipType).ToList();
            //return View(customersList);
            GenerateCircuitOutputFile(Server.MapPath(@"\Content\OtherFile\RC2.cir"));
            List<ChartPoints> dataSetsMember = getCircuitData(Server.MapPath(@"\Content\OtherFile\RC2.raw"));
            MPPTCalculatorViewModel ViewModel = new MPPTCalculatorViewModel();
            ViewModel.dataSets = dataSetsMember;
            return View(ViewModel);
        }
        public ActionResult Details(int id)
        {
            var customer = _context.Customer.Include(c => c.MembershipType).SingleOrDefault(c => c.id == id);
            if (customer == null)
                return HttpNotFound();
            return View(customer);
        }
        public ActionResult New()
        {
            var membershipTypes = _context.MembershipType.ToList();
            var viewModel = new NewCustomerViewModel()
            {
                MembershipType = membershipTypes,
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new NewCustomerViewModel()
                {
                    Customer = customer,
                    MembershipType = _context.MembershipType.ToList()
                };
                return View("New", viewModel);
            }
            if (customer.id == null)
            {
                if (_context.Customer.Count() == 0)
                {
                    customer.id = 1;
                }
                else
                    customer.id = (byte)(_context.Customer.Max(m => m.id)+1);
                _context.Customer.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customer.Single(c => c.id == customer.id);
                customerInDb.name = customer.name;
                customerInDb.birthday = customer.birthday;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.isSubscribedToNewsLetter = customer.isSubscribedToNewsLetter;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Customer");
        }
        public ActionResult Edit(int id)                                                                                                                                                                        
        {
            var customer = _context.Customer.Include(c => c.MembershipType).SingleOrDefault(c => c.id == id);
            if (customer == null)
                return HttpNotFound();
            var membershipTypes = _context.MembershipType.ToList();
            var viewModel = new NewCustomerViewModel()
            {
                MembershipType = membershipTypes,
                Customer = customer                                                     
            };
            return View("New", viewModel);
        }
        public void GenerateCircuitOutputFile(string path)
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
        }

        public List<ChartPoints> getCircuitData (string path)
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
        }

        public void DisplayChart(int width, int height, string chartType, ShowChartViewModel ViewModel)
        {
            var filePathName = "~/Content/myChart" + ViewModel.sessionId + ".jpeg";
            var filepath2 = "/Content/myChart" + ViewModel.sessionId + ".jpeg";
            System.IO.File.Delete(Server.MapPath(filepath2));
            var myChart = new System.Web.Helpers.Chart(width: width, height: height)
            .AddTitle("Chart Title")
            .AddSeries(chartType: chartType,
                name: "Employee",
                xValue: ViewModel.dataSets.ElementAt(ViewModel.xVal).mag,
                yValues: ViewModel.dataSets.ElementAt(ViewModel.yVal).mag);
            myChart.Save(path: filePathName);
        }
        //[HttpPost]
        public ActionResult TestExecutable(ShowChartViewModel ViewModel)
        {

            //ModelState.Clear();
            DisplayChart(600, 400, "Line", ViewModel);
            //clearDataSetsMember();
            return View("showChartTest", ViewModel);
        }
        public ActionResult showChart()
        {
            GenerateCircuitOutputFile(Server.MapPath(@"\Content\OtherFile\RC2.cir"));
            List<ChartPoints> dataSetsMember = getCircuitData(Server.MapPath(@"\Content\OtherFile\RC2.raw"));
            var ViewModel = new ShowChartViewModel(dataSetsMember, 0, 1, HttpContext.Session.SessionID);
            DisplayChart(600, 400, "Line", ViewModel);
            //ModelState.Clear();
            return View("showChartTest",ViewModel);
        }
        /*
        public List<ChartPoints> dataSetsMember { get; set; }
        public void clearDataSetsMember()
        {
            dataSetsMember.Clear();
            dataSetsMember = null;
        }
        */
        [HttpGet]
        public JsonResult HighChartTest()
        {
            GenerateCircuitOutputFile(Server.MapPath(@"\Content\OtherFile\RC2.cir"));
            List<ChartPoints> dataSetsMember = getCircuitData(Server.MapPath(@"\Content\OtherFile\RC2.raw"));
            var ViewModel = new ShowChartViewModel(dataSetsMember, 0, 1, HttpContext.Session.SessionID);
            return Json(ViewModel.dataSets.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DotNetHighChart(int? xData, int? yData)
        {
            //get data
            GenerateCircuitOutputFile(Server.MapPath(@"\Content\OtherFile\RC2.cir"));
            List<ChartPoints> dataSetsMember = getCircuitData(Server.MapPath(@"\Content\OtherFile\RC2.raw"));

            if (!xData.HasValue)
                xData = 0;
            if (!yData.HasValue)
                yData = 1;

            object[,] object_array1 = new object[dataSetsMember.ElementAt(0).mag.Count(), 2];
            for (int i = 0; i < dataSetsMember.ElementAt(0).mag.Count(); i++)
            {
                object_array1[i, 0] = dataSetsMember.ElementAt((int)xData).mag.ElementAt(i);
                object_array1[i, 1] = dataSetsMember.ElementAt((int)yData).mag.ElementAt(i);
            }
            var series1 = new Series();
            series1.Data = new Data(object_array1);

            var chartSeries = new List<Series>();
            chartSeries.Add(series1);
            MPPTCalculatorViewModel ViewModel = new MPPTCalculatorViewModel();
            /*
            ViewModel.chart = new DotNet.Highcharts.Highcharts("chart")
        .SetSeries(chartSeries.ToArray());
        */
            ViewModel.dataSets = dataSetsMember;
    return View(ViewModel);
        }

        public JsonResult AjaxMethod(int? xData, int? yData)
        {
            //get data
            GenerateCircuitOutputFile(Server.MapPath(@"\Content\OtherFile\RC2.cir"));
            List<ChartPoints> dataSetsMember = getCircuitData(Server.MapPath(@"\Content\OtherFile\RC2.raw"));

            if (!xData.HasValue)
                xData = 0;
            if (!yData.HasValue)
                yData = 1;

            object[,] object_array1 = new object[dataSetsMember.ElementAt(0).mag.Count(), 2];
            for (int i = 0; i < dataSetsMember.ElementAt(0).mag.Count(); i++)
            {
                object_array1[i, 0] = dataSetsMember.ElementAt((int)xData).mag.ElementAt(i);
                object_array1[i, 1] = dataSetsMember.ElementAt((int)yData).mag.ElementAt(i);
            }
            var series1 = new Series();
            series1.Data = new Data(object_array1);

            var chartSeries = new List<Series>();
            chartSeries.Add(series1);
            MPPTCalculatorViewModel ViewModel = new MPPTCalculatorViewModel();
            /*
            ViewModel.chart = new DotNet.Highcharts.Highcharts("chart")
        .SetSeries(chartSeries.ToArray());*/
            ViewModel.dataSets = dataSetsMember;
            ViewModel.lengthArray = dataSetsMember[0].mag.Count();
            ViewModel.xData = (int)xData;
            ViewModel.yData = (int)yData;
            return Json(ViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}                                                                                                                                                                                     