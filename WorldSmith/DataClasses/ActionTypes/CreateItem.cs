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
	[EditorGrammar("Create %ItemName within a %SpawnRadius unit radius around %Target with %ItemChargeCount charges and belongs to caster %BelongToCaster")]
	public partial class CreateItem : TargetedAction
	{
		[Category("Misc")]
		[Description("No Description Set")]
		[DefaultValue("")]
		public string ItemName
		{
			get;
			set;
		}

		[Category("Misc")]
		[Description("No Description Set")]
		[DefaultValue(typeof(NumberValue), "")]
		public NumberValue SpawnRadius
		{
			get;
			set;
		}

		[Category("Misc")]
		[Description("No Description Set")]
		[DefaultValue(typeof(NumberValue), "")]
		public NumberValue ItemChargeCount
		{
			get;
			set;
		}

		[Category("Misc")]
		[Description("No Description Set")]
		[DefaultValue("")]
		public string BelongsToCaster
		{
			get;
			set;
		}

	}
}
