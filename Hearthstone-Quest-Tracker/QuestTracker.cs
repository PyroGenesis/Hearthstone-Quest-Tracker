﻿/*
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
	/// Description of QuestTracker.
	/// </summary>
	public class QuestTracker
	{
		internal QuestOverlay overlay;
		internal List<Quest> quest_list;
		
		public QuestTracker(QuestOverlay _overlay)
		{
			this.overlay = _overlay;
			quest_list = new List<Quest>();
			SetQuest("Paladin");
			SetQuest("Hunter");
			SetQuest("Warrior");
			
			if (Config.Instance.HideInMenu && CoreAPI.Game.IsInMenu)
				overlay.Hide();
			
		}
		
		internal void SetQuest(string qname)
		{
			Quest q = new Quest(qname);
			if(quest_list.Count < 3)
			{
				quest_list.Add(q);
				Log.Info("----- Added quest for "+qname+" -----");
			}
			else
			{
				Log.Info("----- Quest list is full! -----");
			}
		}
		
		internal void TurnStart(ActivePlayer player)
        {
			Log.Info("Internal TurnStart just triggered !!!");
        }

        internal void GameStart()
        {
        	Log.Info("Internal GameStart just triggered !!!");
        	// TODO: Tracker should not run in practice or Spectate mode
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
        	bool classQuest = quest_list.Any(quest => quest.category.Equals("class"));
        	bool minionQuest = quest_list.Any(quest => quest.category.Equals("minion"));
        	bool otherQuest = quest_list.Any(quest => quest.category.Equals("other"));
        	
        	if(classQuest)
        	{
        		foreach(var q in quest_list)
        		{
        			if(card.IsClass(q.quest_name))
        				q.count++;
        		}
        		
        	}
        	
        	string oldClass = card.GetPlayerClass;
        	Log.Info("----- You just played " + oldClass + " card -----");
        	overlay.UpdateQuests(quest_list);
        }
	}
}
