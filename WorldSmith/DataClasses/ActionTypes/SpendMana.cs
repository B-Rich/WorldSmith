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
	[EditorGrammar("Spend %Mana mana points")]
	public partial class SpendMana : BaseAction
	{
		[Category("Misc")]
		[Description("int")]
		[DefaultValue(typeof(NumberValue), "")]
		public NumberValue Mana
		{
			get;
			set;
		}

	}
}
