﻿using FTN.Common.CE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CASSubscriberProxyNS
{
    public class CASSubscriberProxy
    {
        /// <summary>
        /// Channel for CAS pub/sub
        /// </summary>
        private IControlActiveSettingsSubscriber proxy = null;

        private object instance = null;

        // Broj pokusaja uspostavljanja komunikacije
        private const int maxTry = 5;

        // Spavanje do narednog pokusaja
        private const int sleepTime = 3000;

        /// <summary>
        /// Constructor
        /// </summary>
        public CASSubscriberProxy(object instance)
        {
            this.instance = instance;
            OpenChannel();
        }

        private void OpenChannel()
        {
            DuplexChannelFactory<IControlActiveSettingsSubscriber> factory = new DuplexChannelFactory<IControlActiveSettingsSubscriber>(
              new InstanceContext(instance),
              new NetTcpBinding(),
              new EndpointAddress("net.tcp://localhost:10012/IControlActiveSettingsSubscriber"));

            try
            {
                proxy = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Subscribed()
        {
            int tryCounter = 0;

            while (true)
            {
                try
                {
                    proxy.Subscribed();
                    break;
                }
                catch (Exception)
                {
                    tryCounter++;

                    if (tryCounter.Equals(maxTry))
                    {
                        throw;
                    }

                    Thread.Sleep(sleepTime);
                    OpenChannel();
                }
            }
        }

        public void Unsubscribed()
        {
            int tryCounter = 0;

            while (true)
            {
                try
                {
                    proxy.Unsubscribed();
                    break;
                }
                catch (Exception)
                {
                    tryCounter++;

                    if (tryCounter.Equals(maxTry))
                    {
                        throw;
                    }

                    Thread.Sleep(sleepTime);
                    OpenChannel();
                }
            }
        }
    }
}
