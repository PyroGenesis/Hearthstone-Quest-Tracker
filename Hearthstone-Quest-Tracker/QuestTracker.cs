/*
 * Created by SharpDevelop.
 * User: Burhanuddin
 * Date: 02-12-2017
 * Time: 02:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;

using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Utility.Logging;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace Hearthstone_Quest_Tracker
{
	/// <summary>
	/// Main class wihich tracks quests
	/// </summary>
	public class QuestTracker
	{
		// The tracker overlay
		internal QuestOverlay overlay;
		// The List of Quests (objects)
		internal List<Quest> quest_list;
		
		// Basic initialization and hiding overlay (triggered on plugin load)
		public QuestTracker(QuestOverlay _overlay)
		{
			this.overlay = _overlay;
			this.quest_list = new List<Quest>();
			
			if (Config.Instance.HideInMenu && CoreAPI.Game.IsInMenu)
				overlay.Hide();
		}
		
		// Adding a quest to the list (triggered by QuestSelection)
		// Quests are set using display name
		// Custom start count is assigned
		// returns status to QuestSelection
		internal bool SetQuest(string qname, int start_number)
		{
			Quest q = new Quest(qname);
			q.count = start_number;
			if(quest_list.Count < 3)
			{
				quest_list.Add(q);
				Log.Info("----- Added quest for "+qname+" -----");
				return true;
			}
			else
			{
				Log.Info("----- Quest list is full! -----");
				return false;
			}
		}
		
		internal void TurnStart(ActivePlayer player)
        {
			Log.Info("Internal TurnStart just triggered !!!");
        }

        internal void GameStart()
        {
        	Log.Info("Internal GameStart just triggered !!!");
        	// TODO: Tracker should not run in Practice or Spectate mode or Dungeon
        	// Puts quests in overlay and displays overlay
        	overlay.UpdateQuests(quest_list);
        }
        
        internal void GameEnd()
        {
        	Log.Info("----- Game has Ended -----");
        	// TODO: Hide overlay only after victory screen
        	overlay.Hide();
        }
        
        internal void CardPlay(Card card)
        {
        	// Booleans to see if tracker should check the card with a particular category of quest
        	bool classQuest = quest_list.Any(quest => quest.category.Equals("class"));
        	bool minionQuest = quest_list.Any(quest => quest.category.Equals("minion"));
        	bool cardTypeQuest = quest_list.Any(quest => quest.category.Equals("cardtype"));
        	bool otherQuest = quest_list.Any(quest => quest.category.Equals("other"));
        	
        	Log.Info("----- This card has Cost: " + card.Cost + " and Type: " + card.Type + " -----" + " and Mechanics: " + string.Join(", ",card.Mechanics));
        	
        	// Triggers based on the category of quests that are being tracked
        	
        	// This includes all the cards with their respective classes:
        	// Druid, Hunter, Mage, Paladin, Priest, Shaman, Rogue, Warlock, Warrior
        	if(classQuest)
        	{
        		// foreach used instead of searching because Tri-class cards
        		foreach(var q in quest_list)
        		{
        			// card.IsClass used over card.GetPlayerClass because Tri-class cardes go confused
        			if(card.IsClass(q.quest_name))
        				q.count++;
        		}
        		
        	}
        	
        	// This includes:
        	// Race quests: Beasts, Demons, Murlocs, Pirates, Elementals
        	// CardType quests: Spells, Weapons
        	// Minion mechanics: Battlecry, Deathrattle, Divine Shield, Enrage, Taunt
        	// CardType quests included in this section as RaceOrType triggers cardtype quests also.
        	if(minionQuest || cardTypeQuest)
        	{
        		foreach(var q in quest_list)
        		{
        			if(card.RaceOrType.Equals(q.quest_name))
        				q.count++;
        			else if(card.Mechanics != null && card.Mechanics.Contains(q.quest_name))
        				q.count++;
        		}
        	}
        	
        	// FIXME: May not work with other CardType quests
//        	if(cardTypeQuest)
//        	{
//        		foreach(var q in quest_list)
//        		{
//        			if(card.Type.Equals(q.quest_name))
//        				q.count++;
//        		}
//        	}
        	
        	// This includes:
        	// Minions with cost <= 2
        	// Minions with cost >= 5
        	if(otherQuest)
        	{
        		foreach(var q in quest_list)
        		{
        			if(q.quest_name.StartsWith("Minions that"))
        			{
        				if(q.quest_name.EndsWith("2"))
        				{
        					if(card.Type.Equals("Minion") && card.Cost<=2)
        						q.count++;
        				}
        				else if(q.quest_name.EndsWith("5"))
        				{
        					if(card.Type.Equals("Minion") && card.Cost>=5)
        						q.count++;
        				}
        			}
        		}
        	}
        	
        	// updating overlay count
        	overlay.UpdateQuests(quest_list);
        }
        
        // Only used for the hero power quest
        internal void HeroPower()
        {
        	bool otherQuest = quest_list.Any(quest => quest.category.Equals("other"));
        	if(otherQuest)
        	{
        		foreach(var q in quest_list)
        		{
        			if(q.quest_name.Equals("Hero Power"))
        				q.count++;
        		}
        	}
        	overlay.UpdateQuests(quest_list);
        }
	}
}
