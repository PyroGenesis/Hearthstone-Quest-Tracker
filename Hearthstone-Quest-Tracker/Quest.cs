/*
 * Created by SharpDevelop.
 * User: Burhanuddin
 * Date: 02-12-2017
 * Time: 02:05 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;

using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace Hearthstone_Quest_Tracker
{
	/// <summary>
	/// Description of Quest.
	/// </summary>
	public class Quest
	{
		internal string quest_name {get; set;}
		internal int count {get; set;}
		internal string category {get; set;}
		internal int reward;
		
		public Quest(string qname)
		{
			this.quest_name = qname;
			if(qname.Equals("Warrior") || qname.Equals("Shaman") || qname.Equals("Rogue") || qname.Equals("Paladin") || qname.Equals("Hunter") || qname.Equals("Druid") || qname.Equals("Warlock") || qname.Equals("Mage") || qname.Equals("Priest"))
				this.category = "class";
			this.count = 0;
		}
	}
}
