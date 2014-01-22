/*****************************  NOTICE  ********************************************
* This file was autogenerated.  Do not edit.                                       *
* Instead, modify the schema in DataSchema related to this class and regenerate it *
***********************************************************************************/
using System;
using System.ComponentModel;
using WorldSmith.Panels;
using WorldSmith.Dialogs;

namespace WorldSmith.DataClasses
{
	[DotaAction]
	[EditorGrammar("Do %Action after a %Delay second delay")]
	public partial class DelayedAction : BaseAction
	{
		[Category("Misc")]
		[Description("No Description Set")]
		[DefaultValue(null)]
		public ActionCollection Action
		{
			get;
			set;
		}

		[Category("Misc")]
		[Description("float")]
		[DefaultValue(typeof(NumberValue), "")]
		public NumberValue Delay
		{
			get;
			set;
		}

	}
}
