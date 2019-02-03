using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace Dynamics.Utils.Plugins
{
    public class ImpersonateRecord : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracingService.Trace("Starting impersonation");

            IPluginExecutionContext context = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext));
 
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                if (entity.Contains("rtb_overriddencreatedby"))
                {
                    entity["createdby"] = entity["rtb_overriddencreatedby"];
                    entity["rtb_overriddencreatedby"] = new EntityReference("systemuser", context.UserId);
                }

                if (entity.Contains("rtb_overriddenmodifiedon"))
                {
                    entity["modifiedon"] = entity["rtb_overriddenmodifiedon"];
                    entity["rtb_overriddenmodifiedon"] = DateTime.Now;
                }

                if (entity.Contains("rtb_overriddenmodifiedby"))
                {
                    entity["modifiedby"] = entity["rtb_overriddenmodifiedby"];
                    entity["rtb_overriddenmodifiedby"] = new EntityReference("systemuser", context.UserId);
                }

                tracingService.Trace("End of impersonation");
            }
            else
                tracingService.Trace("No entity Target was found");
        }
    }
}
