/*
 * Created by SharpDevelop.
 * User: Burhanuddin
 * Date: 02-12-2017
 * Time: 01:56 AM
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
	public class QuestPlugin : IPlugin
	{
		private QuestOverlay overlay;
		MenuItem menuItem = null;
		
		public void OnLoad()
	    {
		    //when it's loaded upon each restart/turned on by the user
		    Log.Info("Hello from quest this is load");
		    overlay = new QuestOverlay();
		    CoreAPI.OverlayCanvas.Children.Add(overlay);
		    QuestTracker tracker = new QuestTracker(overlay);
			
		    GameEvents.OnGameStart.Add(tracker.GameStart);
			GameEvents.OnTurnStart.Add(tracker.TurnStart);
			GameEvents.OnPlayerPlay.Add(tracker.CardPlay);
			GameEvents.OnGameEnd.Add(tracker.GameEnd);
			GameEvents.OnPlayerHeroPower.Add(tracker.HeroPower);
			
			this.menuItem = new MenuItem() { Header = "Choose Quests" };
			this.menuItem.Click += (sender, e) => {
				var x = new QuestSelection(tracker);
			};
			
			/*var chooseclass = new MenuItem() {Header = "Class"};
			var warrior = new MenuItem() {Header = "Warrior"};
			warrior.Click += (sender, e) => tracker.SetQuest("Warrior");
			var shaman = new MenuItem() {Header = "Shaman"};
			shaman.Click += (sender, e) => tracker.SetQuest("Shaman");
			var rogue = new MenuItem() {Header = "Rogue"};
			rogue.Click += (sender, e) => tracker.SetQuest("Rogue");
			var paladin = new MenuItem() {Header = "Paladin"};
			paladin.Click += (sender, e) => tracker.SetQuest("Paladin");
			var hunter = new MenuItem() {Header = "Hunter"};
			hunter.Click += (sender, e) => tracker.SetQuest("Hunter");
			var druid = new MenuItem() {Header = "Druid"};
			druid.Click += (sender, e) => tracker.SetQuest("Druid");
			var warlock = new MenuItem() {Header = "Warlock"};
			warlock.Click += (sender, e) => tracker.SetQuest("Warlock");
			var mage = new MenuItem() {Header = "Mage"};
			mage.Click += (sender, e) => tracker.SetQuest("Mage");
			var priest = new MenuItem() {Header = "Priest"};
			priest.Click += (sender, e) => tracker.SetQuest("Priest");
			chooseclass.Items.Add(warrior);
			chooseclass.Items.Add(shaman);
			chooseclass.Items.Add(rogue);
			chooseclass.Items.Add(paladin);
			chooseclass.Items.Add(hunter);
			chooseclass.Items.Add(druid);
			chooseclass.Items.Add(warlock);
			chooseclass.Items.Add(mage);
			chooseclass.Items.Add(priest);
			menuItem.Items.Add(chooseclass);
			*/
	    }

	    public void OnUnload()
	    {
		    // handle unloading here. HDT does not literally unload the assembly
		    CoreAPI.OverlayCanvas.Children.Remove(overlay);
	    }

	    public void OnButtonPress()
	    {
		    //when user presses the menu button
	    }

	    public void OnUpdate()
	    {
		    // called every ~100ms
	    }

	    
	    public string Name
		{
			get { return "Hearthstone Quest Tracker"; }
		}
	    
	    public string Author
		{
			get { return "Burhanuddin M. Lakdawala"; }
		}

		public string ButtonText
		{
			get { return "Settings - Coming Soon!"; }
		}

		public string Description
		{
			get { return "Tracks the daily play quests progress"; }
		}

		public MenuItem MenuItem
		{
			get 
			{
				return this.menuItem; 
			}
		}
		
		public Version Version
		{
			get { return new Version(0, 1, 0); }
		}
	}
}