using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using NUnit.Framework;


public static class GlobalVariable
{

	public static Dictionary<string, string> my_data;
	public static Driver driver;
	public static string pay_grade_id;
	public static string path_to_driver = "/Users/donevd/Downloads/driver" ;


}

public class Driver : ChromeDriver
{
	
	String login;
	String password;
	String dafault_text;


	public Driver(string path, Dictionary<string, string> my_data) : base(path)
	{
		this.Url = "https://opensource-demo.orangehrmlive.com/index.php/auth/login";
		this.login = my_data["login"];
		this.password = my_data["password"];
		this.dafault_text = my_data["dafault_text"];

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

	

	public void ToUsers()
	{
		this.FindElement(By.XPath("/html/body/div[1]/div[2]/ul/li[1]/a/b")).Click();
		this.FindElement(By.XPath("/html/body/div[1]/div[2]/ul/li[1]/ul/li[2]/a")).Click();
		this.FindElement(By.XPath("/html/body/div[1]/div[2]/ul/li[1]/ul/li[2]/ul/li[1]/a")).Click();
	}
	public string AddInfo(string button)
	{
		this.FindElement(By.Name(button)).Click();
		this.FindElement(By.XPath("//*[@id=\"jobTitle_jobTitle\"]")).SendKeys(this.dafault_text);
		this.FindElement(By.XPath("/html/body/div[1]/div[3]/div/div[2]/form/fieldset/ol/li[2]/textarea")).SendKeys(this.dafault_text);
		this.FindElement(By.XPath("/html/body/div[1]/div[3]/div/div[2]/form/fieldset/ol/li[4]/textarea")).SendKeys(this.dafault_text);
		this.FindElement(By.Name("btnSave")).Click();
		string answer = this.Exception("AddUsers");
		 
		return answer;
	}
	public string Check()
	{
		this.FindElement(By.XPath($"//*[text()='{this.dafault_text}']")).Click();
		string[] separator = { "jobTitleId=" };
		string pay_grade_id = this.Url.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries)[1];
		return pay_grade_id;

	}
	public string Delete(string pay_grade_id)
	{
		int id = Int32.Parse(pay_grade_id);
		string answer;
		this.FindElement(By.XPath($"//table[@id='resultTable']/tbody/tr/td/input[@type='checkbox' and @value={id}]")).Click();
		this.FindElement(By.Name("btnDelete")).Click();
		this.FindElement(By.Id("dialogDeleteBtn")).Click();
		try
		{
			this.FindElement(By.XPath($"//*[text()='{this.dafault_text}']"));
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


}




[TestFixture]
class MainClass
{


	[Test]
	public void Test1_LogIn()
	{
		GlobalVariable.my_data = new Dictionary<string, string>();
		GlobalVariable.my_data.Add("login", "Admin");
		GlobalVariable.my_data.Add("password", "admin123");
		GlobalVariable.my_data.Add("dafault_text", "diana");
		GlobalVariable.driver = new Driver(GlobalVariable.path_to_driver, GlobalVariable.my_data);


		Assert.AreEqual(GlobalVariable.driver.LogIn(), "SUCCESS"); }

	[Test]
	public void Test2_AddInfo()
	{
		GlobalVariable.driver.ToUsers();

		Assert.AreEqual(GlobalVariable.driver.AddInfo("btnAdd"), "SUCCESS"); }
	[Test]
	public void Test3_ChangeInfo()
	{

		GlobalVariable.driver.ToUsers();

		GlobalVariable.pay_grade_id = GlobalVariable.driver.Check();
		Assert.AreEqual(GlobalVariable.driver.AddInfo("btnSave"), "SUCCESS"); }
	[Test]
	public void Test4_Delete()
	{
		GlobalVariable.driver.ToUsers();
		Assert.AreEqual(GlobalVariable.driver.Delete(GlobalVariable.pay_grade_id), "SUCCESS"); }
	}
	

