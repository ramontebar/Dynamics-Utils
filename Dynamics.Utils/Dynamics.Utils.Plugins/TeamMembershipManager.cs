using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace Dynamics.Utils.Plugins
{
    public class TeamMembershipManager : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));

            tracingService.Trace($"Starting TeamMembership Manager plugin. Message:{context.MessageName}. Entity:{context.PrimaryEntityName}");
        }
    }
}
