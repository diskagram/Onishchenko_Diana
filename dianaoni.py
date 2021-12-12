from time import sleep
from selenium.webdriver import Chrome
from selenium.webdriver.common.by import By
from selenium.common.exceptions import NoSuchElementException


class MyData:
    def __init__ (self, kwargs):
        self.login = kwargs.get('login')
        self.password = kwargs.get('password')
        self.dafault_text = kwargs.get('dafault_text')


class Driver(Chrome):
    def __init__ (self):
        super().__init__()
        self.get("https://opensource-demo.orangehrmlive.com/index.php/auth/login")

    @staticmethod
    def exception (module, span='.//span[@class = "validation-error"]'):
        err = '___'
        try:
            answer = 'ElementFound'
            err = driver.find_element(By.XPATH, span).text
        except NoSuchElementException:
            answer = f'NoSuchElementException in {module}'
        assert (answer != 'ElementFound' or err == ""), f"ERROR - |{err}| in {module}"

    def login (self):
        self.find_element(By.NAME, "txtUsername").send_keys(my_input.login)
        self.find_element(By.NAME, "txtPassword").send_keys(my_input.password)
        self.find_element(By.NAME, "Submit").click()
        self.exception('login', './/span[@id="spanMessage"]')

    def to_users (self):
        self.find_element(By.XPATH, "/html/body/div[1]/div[2]/ul/li[1]/a/b").click()
        self.find_element(By.XPATH, "/html/body/div[1]/div[2]/ul/li[1]/ul/li[2]/a").click()
        self.find_element(By.XPATH, "/html/body/div[1]/div[2]/ul/li[1]/ul/li[2]/ul/li[1]/a").click()

    def add_users (self, button):
        self.find_element(By.NAME, button).click()
        self.find_element(By.XPATH, '//*[@id="jobTitle_jobTitle"]').send_keys(my_input.dafault_text)
        self.find_element(By.XPATH, "/html/body/div[1]/div[3]/div/div[2]/form/fieldset/ol/li[2]/textarea").send_keys(
            my_input.dafault_text)
        self.find_element(By.XPATH, "/html/body/div[1]/div[3]/div/div[2]/form/fieldset/ol/li[4]/textarea").send_keys(
            my_input.dafault_text)
        self.find_element(By.NAME, "btnSave").click()
        self.exception('dafault_text')

    def check (self):
        self.find_element(By.XPATH, "//*[text()='" + my_input.dafault_text + "']").click()
        pay_grade_id = self.current_url.split('obTitleId=')[1]
        return pay_grade_id

    def delete (self):
        driver.find_element(By.XPATH,
                            f"//table[@id='resultTable']/tbody/tr/td/input[@type='checkbox' and @value={int(pay_grade_id)}]").click()
        driver.find_element(By.NAME, "btnDelete").click()
        driver.find_element(By.ID, "dialogDeleteBtn").click()
        try:
            driver.find_element(By.XPATH, "//*[text()='" + my_input.dafault_text + "']")
            print("didnt delete")
        except NoSuchElementException:
            print("deleted")


secret1 = {"login": 'Admin',
           "password": 'admin123',
           "dafault_text": 'diana_oni',
           }
my_input = MyData(secret1)
driver = Driver()
driver.login()
sleep(1)
driver.to_users()
sleep(1)
driver.add_users("btnAdd")
sleep(1)
driver.to_users()
sleep(1)
pay_grade_id = driver.check()
sleep(1)
driver.add_users("btnSave")
sleep(1)
driver.to_users()
sleep(1)
driver.delete()
sleep(1)
driver.quit()
