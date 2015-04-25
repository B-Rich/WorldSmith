﻿using Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSmith.DataClasses;
using System.Reflection;
using Graph.Items;
using WorldSmith.NodeGraph.Items;

namespace WorldSmith.NodeGraph
{
    class ActionNode : Node
    {
        public BaseAction DotaAction
        {
            get;
            private set;
        }

        public ExecuteNodeItem OutputExecute
        {
            get;
            private set;
        }

        public ExecuteNodeItem InputExecute
        {
            get;
            private set;
        }

        public TargetNodeItem TargetPin
        {
            get;
            set;
        }


        public ActionNode(BaseAction action)
            : base(action.ClassName)
        {
            DotaAction = action;
            AddNodeElements();

            this.HeaderColor = System.Drawing.Brushes.SteelBlue;
        }

        public NodeItem GetPinForVariable(string varName)
        {
           
            PropertyInfo[] p = DotaAction.GetType().GetProperties();
            foreach (var prop in p)
            {
                if (prop.PropertyType == typeof(NumberValue))
                {
                    var nv = prop.GetValue(DotaAction) as NumberValue;
                    if (nv.IsVariable && nv.Value == varName)
                        return InputItems.Where(x => x is NodeNumericSliderItem).Cast<NodeNumericSliderItem>().FirstOrDefault(x => x.Text == prop.Name);
                    }
            }

            return null;
        }

        private void AddNodeElements()
        {
            Type t = DotaAction.GetType();

            InputExecute = new ExecuteNodeItem("Execute", NodeItemType.Input);
            this.AddItem(InputExecute);

            OutputExecute = new ExecuteNodeItem("Execute", NodeItemType.Output);
            this.AddItem(OutputExecute);

            //Loop through all of this action's properties and add node elements for each property type
            PropertyInfo[] properties = t.GetProperties();

            //Target should always be ordered first
            var target = properties.FirstOrDefault(x => x.Name == "Target");
            if (target != null)
            {
                TargetPin = new TargetNodeItem(target.Name, NodeItemType.Input);
                this.AddItem(TargetPin);
            }


            foreach (PropertyInfo prop in properties)
            {
                //Skip DotaDataObject's properties as they don't go into the node
                if (prop.Name == "ClassName") continue;
                if (prop.Name == "KeyValue") continue;
                if (prop.Name == "ObjectInfo") continue;
                if (prop.Name == "Target") continue; //Skip target because we handled it already

                NodeItem item = null;
                if (prop.PropertyType == typeof(NumberValue))
                {
                    item = new NodeNumericSliderItem(prop.Name, 20, 20, 0, 100, 0, NodeItemType.Input);
                }
                if(prop.PropertyType == typeof(TargetKey))
                {
                    item = new TargetNodeItem(prop.Name, NodeItemType.Input);
                }
                if(prop.PropertyType == typeof(DotaActionCollection))
                {
                    item = new ExecuteNodeItem(prop.Name, NodeItemType.Output);
                }

                if(item == null) item = new NodeLabelItem(prop.Name, NodeItemType.Input);
                this.AddItem(item);

            }
        }
    }
}
