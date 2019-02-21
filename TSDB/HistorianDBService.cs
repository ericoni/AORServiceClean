﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TSDB.Access;

namespace TSDB.Services
{
	public class HistorianDBService : IDisposable
	{
		IHistorianDB historianDB = null;
		ServiceHost host = null;
		NetTcpBinding binding = null;
		string address = "net.tcp://localhost:10050/IHistorianDB";

		public HistorianDBService()
		{
			historianDB = HistorianDB.Instance;
			binding = new NetTcpBinding();
			InitializeHosts();
		}

		private void InitializeHosts()
		{
			host = new ServiceHost(historianDB);
		}

		public void Dispose()
		{
			CloseHosts();
			GC.SuppressFinalize(this);
		}

		private void CloseHosts()
		{
			if (host == null)
			{
				throw new Exception("Historian database services can not be closed because it is not initialized.");
			}

			host.Close();

			string message = "The Historian database service is closed.";
			Console.WriteLine("\n\n{0}", message);
		}

		public void Start()
		{
			StartHosts();
		}

		private void StartHosts()
		{
			if (host == null)
			{
				throw new Exception("Historian database can not be opend because it is not initialized.");
			}

			string message = string.Empty;

			try
			{
				host.AddServiceEndpoint(typeof(IHistorianDB), binding, address);
				host.Open();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			message = string.Format("The WCF service {0} is ready.", host.Description.Name);
			Console.WriteLine(message);

			message = "Endpoints: ";
			Console.WriteLine(message + address);

			message = "The Historian database service is started.";
			Console.WriteLine("{0}", message);

			Console.WriteLine("************************************************************************Historian database sterted", message);
		}
	}
}
