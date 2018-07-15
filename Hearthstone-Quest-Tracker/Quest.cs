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
	/// This class describes a quest
	/// quest name - display name
	/// category = class, minion, cardtype, misc
	/// count = cards played that satisfy that quest
	/// reward not used yet
	/// goal not used yet = will become the upper limit to auto quest completion detection
	/// 
	/// Quest categories:
	/// Class: Playing class cards
	/// Minion: Play minion with tribe (Eg: Murloc) or Ability (Eg: Battlecry, Enrage)
	/// Card Type: Play cards of a type (Eg: Spells, Weapons, Secrets)
	/// Miscellaneous: Rest of play quests (Deal damage, Play cards with X cost, Hero Power)
	/// 
	/// </summary>
	public class Quest
	{
		internal string quest_name {get; set;}
		internal int count {get; set;}
		internal string category {get; set;}
		internal int goal;
		internal int reward;
		
		public Quest(string qname)
		{
			this.quest_name = qname;
			if(qname.Equals("Hero Power") || qname.StartsWith("Minions that"))
				this.category = "other";
			else if(qname.Equals("Spell") || qname.Equals("Weapon"))
				this.category = "cardtype";
			else if(qname.Equals("Beast") || qname.Equals("Demon") || qname.Equals("Murloc") ||  qname.Equals("Pirate") || qname.Equals("Demon") || qname.Equals("Elemental") || qname.Equals("Battlecry") || qname.Equals("Deathrattle") || qname.Equals("Divine Shield") || qname.Equals("Enrage") || qname.Equals("Taunt"))
				this.category = "minion";
			else if(qname.Equals("Warrior") || qname.Equals("Shaman") || qname.Equals("Rogue") || qname.Equals("Paladin") || qname.Equals("Hunter") || qname.Equals("Druid") || qname.Equals("Warlock") || qname.Equals("Mage") || qname.Equals("Priest"))
				this.category = "class";
			
			this.count = 0;
			this.reward = 0;
		}
	}
}
