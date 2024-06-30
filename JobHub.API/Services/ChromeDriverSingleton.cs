using OpenQA.Selenium.Chrome;

namespace JobHub.API.Services
{
	public static class ChromeDriverSingleton
	{
		private static ChromeDriver? _driver;
		private static readonly object _lock = new object();

		public static ChromeDriver Instance
		{
			get
			{
				if (_driver == null)
				{
					lock (_lock)
					{
						if (_driver == null)
						{
							var options = new ChromeOptions();
							options.AddArgument("-ignore-certificate-errors");
							options.AddArgument("-disable-popup-blocking");
							options.AddArgument("-headless=new");

							_driver = new ChromeDriver(options);
						}
					}
				}
				return _driver;
			}
		}

		public static void Quit()
		{
			if (_driver != null)
			{
				lock (_lock)
				{
					if (_driver != null)
					{
						_driver.Quit();
						_driver = null;
					}
				}
			}
		}
	}
}
