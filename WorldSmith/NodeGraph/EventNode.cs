﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Items;
using WorldSmith.NodeGraph.Items;
using WorldSmith.DataClasses;

namespace WorldSmith.NodeGraph
{
    class EventNode : Node
    {

        public ExecuteNodeItem OutputExecute
        {
            get;
            set;
        }

        public DotaEvent Event
        {
            get;
            set;
        }

        public EventNode(DotaEvent Event)
            : base(Event.ClassName)
        {
            this.Event = Event;

            this.HeaderColor = System.Drawing.Brushes.Brown;

            AddExecPin();
            AddTargetNodes();
            
            
        }

        private void AddTargetNodes()
        {
            if(Event.Targets.HasFlag(DotaEvent.TargetsFlags.ATTACKER))
            {
                var TargetPin = new TargetNodeItem("Attacker", NodeItemType.Output);
                AddItem(TargetPin);
            }
            if (Event.Targets.HasFlag(DotaEvent.TargetsFlags.CASTER))
            {
                var TargetPin = new TargetNodeItem("Caster", NodeItemType.Output);
                AddItem(TargetPin);
            }
            if (Event.Targets.HasFlag(DotaEvent.TargetsFlags.PROJECTILE))
            {
                var TargetPin = new TargetNodeItem("Projectile", NodeItemType.Output);
                AddItem(TargetPin);
            }
            if (Event.Targets.HasFlag(DotaEvent.TargetsFlags.TARGET))
            {
                var TargetPin = new TargetNodeItem("Target", NodeItemType.Output);
                AddItem(TargetPin);
            }
            if (Event.Targets.HasFlag(DotaEvent.TargetsFlags.UNIT))
            {
                var TargetPin = new TargetNodeItem("Unit", NodeItemType.Output);
                AddItem(TargetPin);
            }
        }

        public void AddExecPin()
        {
            OutputExecute = new ExecuteNodeItem("Event", NodeItemType.Output);
            this.AddItem(OutputExecute);          
        }
              
       
    }
}
