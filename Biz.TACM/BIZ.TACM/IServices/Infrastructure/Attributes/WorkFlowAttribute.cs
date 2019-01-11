using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.TACM.Infrastructure.Attributes
{


    [AttributeUsage(AttributeTargets.Class)]
    public sealed class WorkFlowAttribute : Attribute
    {
        public WorkFlow CurrentStep { get; set; }

        public WorkFlowAttribute(WorkFlow currentStep)
        {
            this.CurrentStep = currentStep;
        }
    }
}