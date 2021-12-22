using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using NUnit.Framework;




 public static class GlobalVariables
 {
  public static Dictionary<string, string> my_data;
  public static Driver driver;
  public static string pay_grade_id;
  public static string path_to_driver = "/Users/donevd/Downloads/driver";
  public static string url = "https://opensource-demo.orangehrmlive.com/index.php/auth/login";


  public static void InitialiseSecret()
    {
   my_data = new Dictionary<string, string>();
   my_data.Add("login", "Admin");
   my_data.Add("password", "admin123");
   my_data.Add("job_title", "Product Analydste1qdsw");
  my_data.Add("job_description", "Analyse data");
  my_data.Add("job_note", "a/b tests");
  my_data.Add("job_title_new", "Just make sql queries");
 }

  public static void InitialiseDriver()
    {
   driver = new Driver(path_to_driver, url, my_data);
  }
 }

public class Driver : ChromeDriver
{

 String login;
 String password;
 String job_title;
 String job_description;
 String job_note;
 String job_title_new;


 public Driver(string path, string url, Dictionary<string, string> my_data) : base(path)
 {
  this.Url = url;
  this.login = my_data["login"];
  this.password = my_data["password"];
  this.job_title = my_data["job_title"];
  this.job_description = my_data["job_description"];
  this.job_title_new = my_data["job_title_new"];
  this.job_note = my_data["job_note"];
 }

 public string Exception(string module, string span = ".//span[@class = \"validation-error\"]")
 {
  string answer, error;
  try
  {
   answer = "ERROR";
   error = this.FindElement(By.XPath(span)).Text;
  }
  catch (NoSuchElementException)
  {
   answer = "SUCCESS";
   error = "";
  }
  Console.Write($"{answer} {error}, in {module} \n");
  return answer;
 }


 public string LogIn()
 {
  this.FindElement(By.Name("txtUsername")).SendKeys(this.login);
  this.FindElement(By.Name("txtPassword")).SendKeys(this.password);
  this.FindElement(By.Name("Submit")).Click();
  string answer = this.Exception("login", ".//span[@id=\"spanMessage\"]");
  return answer;

 }
 public void ToUsersSection()
 {
  this.FindElement(By.Id("menu_admin_viewAdminModule")).Click();
  this.FindElement(By.Id("menu_admin_Job")).Click();
  this.FindElement(By.Id("menu_admin_viewJobTitleList")).Click();
 }
 public string AddJob()
 {
  this.FindElement(By.Name("btnAdd")).Click();
  this.FindElement(By.Name("jobTitle[jobTitle]")).SendKeys(this.job_title);
  this.FindElement(By.Name("jobTitle[jobDescription]")).SendKeys(this.job_description);
  this.FindElement(By.Name("jobTitle[note]")).SendKeys(this.job_note);
  this.FindElement(By.Name("btnSave")).Click();

  string answer = this.Exception("AddUsers");

  return answer;
 }
 public string ChangeInfo()
 {
  string[] separator = { "jobTitleId=" };
  this.FindElement(By.XPath($"//*[text()='{this.job_title}']")).Click();
  GlobalVariables.pay_grade_id = this.Url.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries)[1];
  this.FindElement(By.Name("btnSave")).Click();
  this.FindElement(By.Name("jobTitle[jobTitle]")).Click();
  this.FindElement(By.Name("jobTitle[jobTitle]")).Clear();
  this.FindElement(By.Name("jobTitle[jobTitle]")).SendKeys(this.job_title_new);
  this.FindElement(By.Name("btnSave")).Click();


  string answer = this.Exception("ChangeInfo");

  return answer;
 }
 public string Delete()
 {
  int id = Int32.Parse(GlobalVariables.pay_grade_id);
  string answer;
  this.FindElement(By.XPath($"//table[@id='resultTable']/tbody/tr/td/input[@type='checkbox' and @value={id}]")).Click();
  this.FindElement(By.Name("btnDelete")).Click();
  this.FindElement(By.Id("dialogDeleteBtn")).Click();
  try
  {
   this.FindElement(By.XPath($"//*[text()='{this.job_title}']"));
   answer = "ERROR";
  }
  catch (NoSuchElementException)
  {
   answer = "SUCCESS";
  }
  Console.Write($"Delete: {answer}\n");
  this.Quit();
  return answer;
 }
public string IsChangesVisible(string job_title_)
 {
  string answer;
  
  try
  {
   this.FindElement(By.XPath($"//table[@id='resultTable']/tbody/tr[@class='even']/td[@class='left']/a[text()='" + job_title_ + "']")).Click();
   answer = "ERROR";
  }
  catch (NoSuchElementException)
  {
   answer = "SUCCESS";
  }
  return answer;

 }




}




 [TestFixture]
 class MainClass
 {

  [Test]
  public void Test0_Connection()
  {
   GlobalVariables.InitialiseSecret();
   GlobalVariables.InitialiseDriver();
   var actualUrl = GlobalVariables.driver.Url;
   Assert.AreEqual(actualUrl, GlobalVariables.url);
  }

  [Test]
  public void Test1_LogIn()
  {
   var result = GlobalVariables.driver.LogIn();
   Assert.AreEqual(result, "SUCCESS");
  }

  [Test]
  public void Test2_AddInfo()
  {
   GlobalVariables.driver.ToUsersSection();
  var result = GlobalVariables.driver.AddJob();
  Assert.AreEqual(result, "SUCCESS");
  }
 [Test]
 public void Test2_1_AddInfo_Check()
 {
  GlobalVariables.driver.ToUsersSection();

  var result = GlobalVariables.driver.IsChangesVisible(GlobalVariables.my_data["job_title"]);
  Assert.AreEqual(result, "SUCCESS");
 }



 [Test]
  public void Test3_ChangeInfo()
  {
   GlobalVariables.driver.ToUsersSection();
  var result = GlobalVariables.driver.ChangeInfo( );
   Assert.AreEqual(result, "SUCCESS");
  }

 [Test]
 public void Test3_1_ChangeInfo_Check()
 {
  GlobalVariables.driver.ToUsersSection();
  var result = GlobalVariables.driver.IsChangesVisible(GlobalVariables.my_data["job_title_new"]);
  Assert.AreEqual(result, "SUCCESS");
 }


 [Test]
  public void Test4_Delete()
  {
        GlobalVariables.driver.ToUsersSection();
  var result = GlobalVariables.driver.Delete();
   Assert.AreEqual(result, "SUCCESS");
  }
 }
