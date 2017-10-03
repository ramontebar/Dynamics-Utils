using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace Dynamics.Utils.Plugins
{
    public class DelayOperation : IPlugin
    {
        public string UnsecureConfiguration { get; private set; }

        /// <summary>
        /// Milliseconds for Thread.Sleep
        /// </summary>
        public int DelayTime { get; private set; }
        public DelayOperation(string unsecureConfiguration)
        {
            this.UnsecureConfiguration = unsecureConfiguration;
            int delay;
            if (int.TryParse(unsecureConfiguration, out delay))
                DelayTime = delay;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracingService.Trace("Start sleeping for {0} ms", DelayTime);

            System.Threading.Thread.Sleep(DelayTime);

            tracingService.Trace("End delay of operation");
        }
    }
}
